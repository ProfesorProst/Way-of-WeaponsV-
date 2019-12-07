using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace revcom_bot
{
    class Guild
    {
        public long id { get; set; }
        public string name { get; set; }
        public string master{ get; set; }
        public int gold { get; set; }
        public int maxplayers { get; set; }
        public bool hire { get; set; } // true  hire - open\
        public int count { get; set; }
        public int fraction { get; set; }
        public string chatUrl { get; set; }

        public Guild() { }

        public Guild(long id, string name, string master, int gold)
        {
            this.id = id;
            this.name = name;
            this.master = master;
            this.gold = gold;
        }

        public Guild(string master, string name)
        {
            this.name = name;
            this.master = master;
        }
    }
}
