using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale.Structure
{
	[Serializable]
	public struct Map
	{
		public string Name { get; set; }
		public List<Weapon> Weapons { get; set; }
		public List<Vehicle> Vehicles { get; set; }
		public List<Object> Objects { get; set; }
		public List<Vector3> Spawns { get; set; }

		public int CountdownTime { get; set; }
		public float SphereScale { get; set; }
		public Vector3 SphereLocation { get; set; }
		public Vector3 WaitingPosition { get; set; }
		public int PlayersToStart { get; set; }
	}
}
