using BattleRoyale.Structure;
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
	public class BattleRoyale : Script
	{
		private Timer countdownTimer;
		private Timer sphereScaleTimer;
		private int currentScalePercantage = 100;
		public bool currentlyRunning = false;

		public BattleRoyale() { }

		public void StartBattleRoyale()
		{
			API.consoleOutput("Starting battle royale");
			currentlyRunning = true;
			GetPlayers();
			ResetStats();
			SpawnPlayers();
			SpawnWeapons();
			SpawnVehicles();
			SpawnObjects();

			StartCountdown();
		}

		private void GiveWeapons()
		{
			foreach(Player p in MainEntryPoint.Players)
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

			foreach (Structure.Vehicle l in MainEntryPoint.Map.Vehicles)
			{
				API.createVehicle(l.Hash, l.Position, l.Rotation, 0, 0);
			}

		}

		private void SpawnObjects()
		{
			API.consoleOutput("Spawning objects: ");
			foreach (NetHandle h in API.getAllObjects())
			{
				API.deleteEntity(h);
			}

			foreach (Structure.Object w in MainEntryPoint.Map.Objects)
			{
				API.createObject(w.Model, w.Position, w.Rotation);
			}

		}

		private void SpawnWeapons()
		{
			API.consoleOutput("Spawning weapons: ");
			foreach (NetHandle h in API.getAllPickups())
			{
				API.deleteEntity(h);
			}

			foreach(Weapon w in MainEntryPoint.Map.Weapons)
			{
				API.createPickup(w.Hash, w.Position, new Vector3(0,0,0), w.Ammo, 240 * 1000);
			}
		
		}

		private void GetPlayers()
		{
			API.consoleOutput("Getting players");
			foreach (Player p in MainEntryPoint.Players)
			{
				p.inBattleRoyale = true;
			}
		}

		private void StartCountdown()
		{
			API.sendChatMessageToAll(MainEntryPoint.Map.CountdownTime + " seconds till the next Battle Royale");
			countdownTimer = new Timer();
			countdownTimer.Elapsed += new ElapsedEventHandler(countdownTimer_Tick);
			countdownTimer.Interval = 1000;
			countdownTimer.Start();
			API.consoleOutput("Countdown start, " + MainEntryPoint.Map.CountdownTime + " left.");
		}

		private void countdownTimer_Tick(object sender, ElapsedEventArgs e)
		{
			API.consoleOutput("Countdown timer tick");
			MainEntryPoint.Map.CountdownTime--;
			if (MainEntryPoint.Map.CountdownTime <= 0)
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
			foreach (Player p in MainEntryPoint.Players)
			{
				if(p.inBattleRoyale)
				{				
					API.freezePlayer(p.client, false);
					API.triggerClientEvent(p.client, "pennedin_roundstart", MainEntryPoint.Map.SphereLocation, MainEntryPoint.Map.SphereScale);
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
			var currentSphereScale = (MainEntryPoint.Map.SphereScale / 100) * (currentScalePercantage + 15);
			var nextSphereScale = (MainEntryPoint.Map.SphereScale / 100) * currentScalePercantage;
			API.consoleOutput("Scaling down playable area to " + nextSphereScale);
			API.triggerClientEventForAll("pennedin_setscaledestination", currentSphereScale, nextSphereScale , 20000);
		}


		public void SpawnPlayers() 
		{
			API.consoleOutput("Spawning players");
			for (int i = 0; i < MainEntryPoint.Players.Count; i++)
			{
				Player p = MainEntryPoint.Players[i];
				if(p.inBattleRoyale)
				{
					API.setEntityPosition(p.client.handle, MainEntryPoint.Map.Spawns[i]);
					API.freezePlayer(p.client, true);
					API.setEntityInvincible(p.client.handle, false);
				}
			}
		}

		public void ResetStats()
		{
			API.consoleOutput("Reseting stats");
			foreach (Player p in MainEntryPoint.Players)
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
}
