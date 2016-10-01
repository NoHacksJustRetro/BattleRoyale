using BattleRoyale.Structure;
using GTANetworkServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale.Events
{
	class OnDisconnect : MainEntryPoint
	{
		public OnDisconnect()
		{
			API.onPlayerDisconnected += OnPlayerDisconnect;
		}

		private void OnPlayerDisconnect(Client player, string reason)
		{
			foreach(Player p in Players)
			{
				if (p.client == player)
				{
					Players.Remove(p);
					break;
				}			
			}
		}
	}
}

