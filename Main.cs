using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale
{
	public struct GlobalSettings
	{
		public Vector3 WaitingPosition { get; set; }
		public int PlayersToStart { get; set; }
	}

	public class Main : Script
	{
		public delegate void CheckBattleRoyale();
		public static CheckBattleRoyale onCheckBattleRoyale;


		public static BattleRoyale battleRoyale;
		public static GlobalSettings globalSettings;

		public Main()
		{
			API.onResourceStart += OnResourceStart;
			onCheckBattleRoyale += CheckStartConditions;
			API.onPlayerDeath += OnPlayerDeath;
		}

		private void LoadSettings()
		{
			string json = File.ReadAllText(API.getResourceFolder() + "/Settings/globaldata.json");
			globalSettings = API.fromJson(json).ToObject<GlobalSettings>();
		}

		public void DisplaySettings()
		{
			API.consoleOutput("~~~ GLOBAL ROYALE SETTINGS ~~~");
			API.consoleOutput("Players needed to start: " + globalSettings.PlayersToStart);
			API.consoleOutput("Waiting location: " + globalSettings.WaitingPosition.ToString());
			API.consoleOutput("~~~ GLOBAL ROYALE SETTINGS ~~~");
		}

		private void OnResourceStart()
		{
			API.consoleOutput("BattleRoyale has been started!");
			LoadSettings();
			DisplaySettings();

			API.consoleOutput("Minimum player count has been reached");
			API.sendChatMessageToAll("Starting new BattleRoyale round");
			onCheckBattleRoyale();
			//battleRoyale = new BattleRoyale();
		}
		public void CheckStartConditions()
		{
			if (API.getAllPlayers().Count == Main.globalSettings.PlayersToStart)
			{
				if (battleRoyale.currentlyRunning == false)
				{
					API.consoleOutput("Minimum player count has been reached");
					API.sendChatMessageToAll("Starting new BattleRoyale round");
					battleRoyale = new BattleRoyale();
				}
			}
		}


		private void OnPlayerDeath(Client player, NetHandle entityKiller, int weapon)
		{
			foreach (Player p in battleRoyale.players)
			{
				if (p.client == player)
				{
					if (p.inBattleRoyale)
					{
						API.sendChatMessageToAll(player.Name + " has been killed.");
						p.inBattleRoyale = false;
					}
					break;
				}
			}

			CheckBattleRoyaleWin();
		}

		private void CheckBattleRoyaleWin()
		{
			int playersLeft = 0;
			foreach (Player p in battleRoyale.players)
			{
				if (p.inBattleRoyale)
					playersLeft++;
			}
			if (playersLeft <= 1)
			{
				API.sendChatMessageToAll("BattleRoyale has been won.");
				Main.onCheckBattleRoyale();
			}

		}
	}
}
