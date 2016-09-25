using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale
{
	public class Player 
	{
		public NetHandle netHandle { get; set; }
		public bool inBattleRoyale { get; set; }
		public Client client { get; set; }

		public Player() { }
		public Player(NetHandle netHandle, bool inBattleRoyale, Client client)
		{
			this.netHandle = netHandle;
			this.inBattleRoyale = inBattleRoyale;
			this.client = client;
		}
	}
}
