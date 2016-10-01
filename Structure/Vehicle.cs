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
	public struct Vehicle
	{
		public Vector3 Position { get; set; }
		public Vector3 Rotation { get; set; }
		public VehicleHash Hash { get; set; }
	}
}
