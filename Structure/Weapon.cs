using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale.Structure
{
	[Serializable]
	public struct Weapon
	{
		public Vector3 Position { get; set; }
		public PickupHash Hash { get; set; }
		public int Ammo { get; set; }
	}
}
