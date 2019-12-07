using System;
using System.Collections.Generic;

namespace revcom_bot
{
    class Person
    {
        public long id;
        public int hp;
        public int energy;
        public DateTime energytime;
        public int maxenergy;
        public int lvl;
        public int exp;
        public int gold;
        public int def;
        public int atack;
        public string personNick { get; set; }
        public string race;

        public int atackAdditional;
        public int defAdditional;
        public long? guild { get; set; }
        public int? fraction { get; set; } // 0 - Alliance  1 - Republic 
        public bool? attackOrDef { get; set; }
        public List<Item> items { get; set; }

        public const int republic = 1;
        public const int alliance = 0;

        public int GetAlliance() { return alliance; }
        public int GetRepublic() { return republic; }

        public Person(long id, string personnick)
        {
            this.id = id;
            this.personNick = personnick;
        }

        public Person(Person person) { }

        public Person(long id, string personnick, string race)
        {
            this.id = id;
            this.personNick = personnick;
            this.race = race;
        }

        public int GetExpToNextLVL()
        {
            double z = Math.Pow(lvl+1, 3);
            double d = Math.Exp((lvl + 2.0) / 20.0);
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
