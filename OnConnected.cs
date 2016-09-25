using GTANetworkServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale
{
	class OnConnected : Script
	{
		public OnConnected()
		{
			API.onPlayerConnected += OnPlayerConnect;
		}

		private void OnPlayerConnect(Client player)
		{
			API.setEntityPosition(player.CharacterHandle, Main.globalSettings.WaitingPosition);
			API.setEntityInvincible(player.CharacterHandle, true);
			Main.onCheckBattleRoyale();
		}

	}
}
