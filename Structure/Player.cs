using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale.Structure
{
	public class Player 
	{
		public bool inBattleRoyale { get; set; }
		public Client client { get; set; }

		public Player() { }
		public Player(bool inBattleRoyale, Client client)
		{
			this.inBattleRoyale = inBattleRoyale;
			this.client = client;
		}
	}
}
