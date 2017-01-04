using BattleRoyale.Structure;
using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BattleRoyale
{
	public class MainEntryPoint : Script
	{
		public delegate void CheckBattleRoyale();
		public static CheckBattleRoyale onCheckBattleRoyale;

		#region Variables
		public static List<Player> Players = new List<Player>();
		public static BattleRoyale BattleRoyale;
		public static Map Map;
		#endregion

		public MainEntryPoint()
		{
			API.onResourceStart += OnResourceStart;
			onCheckBattleRoyale += CheckStartConditions;
		}


		public void DisplaySettings()
		{
			API.consoleOutput("~~~ BATTLE ROYALE SETTINGS ~~~");
			API.consoleOutput("Players needed to start: " + Map.PlayersToStart);
			API.consoleOutput("Waiting location: " + Map.WaitingPosition.ToString());
			API.consoleOutput("Spawn Positions: ");

			foreach (Structure.Vehicle p in Map.Vehicles)
				API.consoleOutput(p.Hash + " at " + p.Position);
			foreach (Structure.Object o in Map.Objects)
				API.consoleOutput(o.Model + " at " + o.Position);
			foreach (Weapon w in Map.Weapons)
				API.consoleOutput(w.Hash + " at " + w.Position);

			API.consoleOutput("Sphere spawn location: " + Map.SphereLocation.ToString());
			API.consoleOutput("Sphere scale: " + Map.SphereScale);
			API.consoleOutput("Countdown time: " + Map.CountdownTime);
			API.consoleOutput("~~~ BATTLE ROYALE SETTINGS ~~~");
		}

		private void OnResourceStart()
		{
			API.consoleOutput("BattleRoyale has been started!");
			LoadMap();
			DisplaySettings();
			//onCheckBattleRoyale();
		}
		public void CheckStartConditions()
		{
			if (API.getAllPlayers().Count == MainEntryPoint.Map.PlayersToStart)
			{
				if (BattleRoyale.currentlyRunning == false)
				{
					API.consoleOutput("Minimum player count has been reached");
					API.sendChatMessageToAll("Starting new BattleRoyale round");
					BattleRoyale = new BattleRoyale();
					BattleRoyale.StartBattleRoyale();
				}
			}
		}

		private void LoadMap(string name = "default")
		{
			string rootPath = API.getResourceFolder() + "/" + name;
			string mapJSON = File.ReadAllText(rootPath +"/map.json");	
			Map map = JsonConvert.DeserializeObject<Map>(mapJSON);

			string vehiclesJSON = File.ReadAllText(rootPath + "/vehicles.json");	
			map.Vehicles = JsonConvert.DeserializeObject<List<Structure.Vehicle>>(vehiclesJSON);

			string objectsJSON = File.ReadAllText(rootPath + "/objects.json");		
			map.Objects = JsonConvert.DeserializeObject<List<Structure.Object>>(objectsJSON);

			string weaponsJSON = File.ReadAllText(rootPath + "/weapons.json");
			map.Weapons = JsonConvert.DeserializeObject<List<Weapon>>(weaponsJSON);

			string spawnsJSON = File.ReadAllText(rootPath + "/spawns.json");
			map.Spawns = JsonConvert.DeserializeObject<List<Vector3>>(spawnsJSON);

			MainEntryPoint.Map = map;
		}

		private void CheckBattleRoyaleWin()
		{
			int playersLeft = 0;
			foreach (Player p in Players)
			{
				if (p.inBattleRoyale)
					playersLeft++;
			}
			if (playersLeft <= 1)
			{
				API.sendChatMessageToAll("BattleRoyale has been won.");
				MainEntryPoint.onCheckBattleRoyale();
			}

		}
	}
}
