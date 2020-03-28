using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wayofweapon.Entities
{
    class Guild
    {
        [Key]
        public long id { get; set; }
        [MaxLength(128), Index(IsUnique = true)]
        public string name { get; set; }
        [MaxLength(128), Index(IsUnique = true)]
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
            this.maxplayers = 10;
            this.gold = gold;
        }

        public Guild(string master, string name)
        {
            this.name = name;
            this.master = master;
            this.maxplayers = 10;
        }
    }
}
