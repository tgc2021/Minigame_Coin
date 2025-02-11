using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minigame_coin.Models
{
    public class CoinsGameLog
    {
        public int org_id { get; set; }
        public int id_game { get; set; }
        public int Xps { get; set; }
        public int Time { get; set; }
        public int id_user { get; set; }
        public string Status { get; set; }
    }
}