using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace revcom_bot
{
    class Item
    {
        public long id { get; set; }
        public string name { get; set; }
        public int cost { get; set; }
        public int def { get; set; }
        public int attack { get; set; }
        public bool eqiup { get; set; }
        public int count { get; set; }
        public string type { get; set; }

        public Item(long id, string name, int cost, int def, int attack)
        {
            this.id = id;
            this.name = name;
            this.cost = cost;
            this.def = def;
            this.attack = attack;
        }

        public Item(long id,  int count, bool eqiup)
        {
            this.id = id;
            this.count = count;
            this.eqiup = eqiup;
        }
    }
}
