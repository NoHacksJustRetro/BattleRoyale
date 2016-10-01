using BattleRoyale.Structure;
using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale.Events
{
	class OnDeath : MainEntryPoint
	{
		public OnDeath()
		{
			API.onPlayerDeath += OnPlayerDeath;
		}

		private void OnPlayerDeath(Client player, NetHandle entityKiller, int weapon)
		{
			foreach (Player p in Players)
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
			onCheckBattleRoyale();
		}
	}
}
