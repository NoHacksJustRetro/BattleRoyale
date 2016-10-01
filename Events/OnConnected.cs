using BattleRoyale.Structure;
using GTANetworkServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale.Events
{
	class OnConnected : MainEntryPoint
	{
		public OnConnected()
		{
			API.onPlayerConnected += OnPlayerConnect;
		}

		private void OnPlayerConnect(Client player)
		{
			API.setEntityPosition(player.CharacterHandle, Map.WaitingPosition);
			API.setEntityInvincible(player.CharacterHandle, true);
			Players.Add(new Player(false, player));
			onCheckBattleRoyale();
		}

	}
}
