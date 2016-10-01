using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale.Structure
{
	[Serializable]
	public struct Object
	{
		public Vector3 Position { get; set; }
		public Vector3 Rotation { get; set; }
		public int Model { get; set; }
	}
}
