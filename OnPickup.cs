using GTANetworkServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkShared;
namespace BattleRoyale
{
	class OnPickup : Script
	{
		public OnPickup()
		{
			API.onPlayerPickup += OnPlayerPickup;
		}

		private void OnPlayerPickup(Client pickupee, NetHandle pickupHandle)
		{
			API.givePlayerWeapon(pickupee, WeaponHash.MicroSMG, 50, false, true);
		}

	}
}
