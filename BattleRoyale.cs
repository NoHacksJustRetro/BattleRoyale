using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace BattleRoyale
{
	[Serializable]
	public struct BattleRoyaleSettings
	{
		public List<Vector3> spawnPositions { get; set; }
		public List<Vector3> weaponPositions { get; set; }
		public List<Vector3> vehiclePositions { get; set; }
		public int countdownTime { get; set; }
		public float SphereScale { get; set; }
		public Vector3 SphereLocation { get; set; }	
	}

	public class BattleRoyale : Script
	{
		public BattleRoyaleSettings settings;
		private Timer countdownTimer;
		private Timer sphereScaleTimer;
		private int currentScalePercantage = 100;
		public bool currentlyRunning = false;
		public List<Player> players = new List<Player>();

		public BattleRoyale()
		{
			API.onResourceStart += API_onResourceStart;
		}

		private void API_onResourceStart()
		{
			StartBattleRoyale();

		}

		public void StartBattleRoyale()
		{
			API.consoleOutput("Starting battle royale");
			currentlyRunning = true;
			LoadSettings();
			DisplaySettings();
			GetPlayers();
			ResetStats();
			SpawnPlayers();
			SpawnWeapons();
			SpawnVehicles();

			StartCountdown();
		}

		private void DisplaySettings()
		{
			API.consoleOutput("~~~ BATTLE ROYALE SETTINGS ~~~");
			API.consoleOutput("Spawn Positions: ");
			foreach(Vector3 p in settings.spawnPositions)
			{
				API.consoleOutput(p.ToString());
			}
			API.consoleOutput("Sphere spawn location: " + settings.SphereLocation.ToString());
			API.consoleOutput("Sphere scale: " + settings.SphereScale);
			API.consoleOutput("Countdown time: " + settings.countdownTime);
			API.consoleOutput("~~~ BATTLE ROYALE SETTINGS ~~~");
		}

		private void GiveWeapons()
		{
			foreach(Player p in players)
			{
				API.removeAllPlayerWeapons(p.client);
				API.givePlayerWeapon(p.client, WeaponHash.Pistol, 60, true, true);
			}
		}

		private void SpawnVehicles()
		{
			API.consoleOutput("Spawning vehicles: ");
			foreach (NetHandle h in API.getAllVehicles())
			{
				API.deleteEntity(h);
			}

			foreach (Vector3 l in settings.vehiclePositions)
			{
				API.createVehicle(VehicleHash.Dubsta, l, new Vector3(0, 0, 0), 0, 0);
			}

		}

		private void SpawnWeapons()
		{
			API.consoleOutput("Spawning weapons: ");
			foreach (NetHandle h in API.getAllPickups())
			{
				API.deleteEntity(h);
			}

			foreach(Vector3 l in settings.weaponPositions)
			{
				API.createPickup(PickupHash.PICKUP_WEAPON_MICROSMG, l, new Vector3(0, 0, 0), 50, 60000);
			}
		
		}

		private void GetPlayers()
		{
			API.consoleOutput("Getting players");
			players.Clear();
			foreach (NetHandle p in API.getAllPlayers())
			{
				players.Add(new Player(p, true));
			}
		}

		private void LoadSettings()
		{
			API.consoleOutput("Loading settings");
			string json = File.ReadAllText(API.getResourceFolder() + "/Settings/data.json");
			settings = API.fromJson(json).ToObject<BattleRoyaleSettings>();
		}

		private void StartCountdown()
		{
			API.sendChatMessageToAll(settings.countdownTime + " seconds till the next Battle Royale");
			countdownTimer = new Timer();
			countdownTimer.Elapsed += new ElapsedEventHandler(countdownTimer_Tick);
			countdownTimer.Interval = 1000;
			countdownTimer.Start();
			API.consoleOutput("Countdown start, " + settings.countdownTime + " left.");
		}

		private void countdownTimer_Tick(object sender, ElapsedEventArgs e)
		{
			API.consoleOutput("Countdown timer tick");
			settings.countdownTime--;
			if (settings.countdownTime <= 0)
			{
				countdownTimer.Stop();
				CountdownFinished();
			}
			else
			{
				countdownTimer.Stop();
				StartCountdown();
			}		
		}

		private void CountdownFinished()
		{
			API.consoleOutput("Countdown Finished, Battle Royale has been started");
			API.sendChatMessageToAll("Battle Royale has been started!");
			foreach (Player p in players)
			{
				if(p.inBattleRoyale)
				{				
					API.freezePlayer(p.client, false);
					API.triggerClientEvent(p.client, "pennedin_roundstart", settings.SphereLocation, settings.SphereScale);
				}	
			}

			sphereScaleTimer = new Timer();
			sphereScaleTimer.Elapsed += new ElapsedEventHandler(sphereScaleTimer_tick);
			sphereScaleTimer.Interval = 30000; // 2.5 minutes
			sphereScaleTimer.Start();
		}

		private void sphereScaleTimer_tick(object sender, ElapsedEventArgs e)
		{
			
			API.sendChatMessageToAll("Scaling down playable area!");
			currentScalePercantage -= 15;
			var currentSphereScale = (settings.SphereScale / 100) * (currentScalePercantage + 15);
			var nextSphereScale = (settings.SphereScale / 100) * currentScalePercantage;
			API.consoleOutput("Scaling down playable area to " + nextSphereScale);
			API.triggerClientEventForAll("pennedin_setscaledestination", currentSphereScale, nextSphereScale , 20000);
		}


		public void SpawnPlayers() 
		{
			API.consoleOutput("Spawning players");
			for (int i = 0; i < players.Count; i++)
			{
				Player p = players[i];
				if(p.inBattleRoyale)
				{
					API.setEntityPosition(p.netHandle, settings.spawnPositions[i]);
					API.freezePlayer(p.client, true);
					API.setEntityInvincible(p.netHandle, false);
				}
			}
		}

		public void ResetStats()
		{
			API.consoleOutput("Reseting stats");
			foreach (Player p in players)
			{
				if(p.inBattleRoyale)
				{
					API.sendChatMessageToPlayer(p.client, "Your Health and Armor has been restored");
					API.setPlayerArmor(p.client, 100);
					API.setPlayerHealth(p.client, 100);
				}	
			}
		}
	}

	

	public class Player : Script
	{
		public NetHandle netHandle { get; set; }
		public bool inBattleRoyale { get; set; }
		public Client client { get { return API.getPlayerFromHandle(netHandle); } }

		public Player() { }
		public Player(NetHandle netHandle, bool inBattleRoyale)
		{
			this.netHandle = netHandle;
			this.inBattleRoyale = inBattleRoyale;
		}
	}
}
