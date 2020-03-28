using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace wayofweapon.Entities
{
    class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long id { get; private set; }
        [MaxLength(128), Index(IsUnique = true)]
        public string personNick { get; set; }

        public int hp { get; set; }
        public int energy { get; set; }
        public DateTime energytime { get; set; }
        public int maxenergy { get; set; }
        public int def { get; set; }
        public int atack { get; set; }
        public int gold { get; set; }
        public int lvl { get; set; }
        public int exp { get; set; }
        [MaxLength(16)]
        public string race { get; set; }

        //public int atackAdditional { get; set; }
        //public int defAdditional { get; set; }
        public long? guildId { get; set; }
        public Guild guild { get; set; }
        public int? fraction { get; set; } // 0 - Alliance  1 - Republic 
        public bool? stateOfWar { get; set; }


        public const int republic = 1;
        public const int alliance = 0;

        public int GetAlliance() { return alliance; }
        public int GetRepublic() { return republic; }

        public Person() { }
        public Person(long id, string personnick)
        {
            this.id = id;
            this.personNick = personnick;
            this.energytime = DateTime.Now;
        }

        public Person(Person person) 
        {
            this.id = person.id;
            this.hp = person.hp;
            this.energy = person.energy;
            this.energytime = person.energytime;
            this.maxenergy = person.maxenergy;
            this.exp = person.exp;
            this.gold = person.gold;
            this.def = person.def;
            this.atack = person.atack;
            this.personNick = person.personNick;
            this.guild = person.guild;
            this.fraction = person.fraction;
            this.lvl = person.lvl;
            this.guild = person.guild;
        }

        public Person(long id, string personnick, string race)
        {
            this.id = id;
            this.personNick = personnick;
            this.race = race;
            this.energytime = DateTime.Now;
            this.hp = 10;
        }

        private const int magic1 = 1;
        private const int magic2 = 3;
    
        private const int magic3 = 2;
        private const int magic4 = 20;
        public int GetExpToNextLVL()
        {
            double z = Math.Pow(lvl + magic1, magic2);
            double d = Math.Exp((lvl + magic3) / magic4);
            return (int)(z * d);
        }

        public virtual double GetMultiplierExp()
        {
            return 1;
        }

        public virtual double GetMultiplierGold()
        {
            return 1;
        }

        public bool changes = false;
        public virtual bool LvlUp(int state)
        {

            return false;
        }
    }
}
