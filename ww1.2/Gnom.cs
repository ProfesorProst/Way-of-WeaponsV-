﻿namespace revcom_bot
{
    class Gnom : Person
    {
        public Gnom(long id, string personnick) : base(id, personnick)
        {
            this.energy = 5;
            this.maxenergy = 5;
            this.def = 2;
            this.atack = 2;
            this.race = "Gnom";
        }

        public Gnom(Person person) : base(person)
        {
            this.id = person.id;
            this.hp = person.hp;
            this.energy = person.energy;
            this.energytime = person.energytime;
            this.maxenergy = person.maxenergy;
            this.lvl = person.lvl;
            this.exp = person.exp;
            this.gold = person.gold;
            this.def = person.def;
            this.atack = person.atack;
            this.personNick = person.personNick;
            this.guild = person.guild;
            this.fraction = person.fraction;
            this.race = "Gnom";
        }

        public override double GetMultiplierExp()
        {
            return 0.9;
        }

        public override double GetMultiplierGold()
        {
            return 1 + (lvl / 7) * 0.05;
        }

        public override bool LvlUp(int state)
        {
            int startCountOfParams = 4;// start parametr atak + def = 6 - 1lvl = 5;
            int each7Lvl = 7; // on each 7lvl + 1 point; -1 point to use it
            int countOfAttDef = startCountOfParams + (lvl - 1); // we start from 1 and dont add any 
            int countOfAttDefANDspechialparams = startCountOfParams + (lvl - 1) + (lvl / each7Lvl);

            if (state != -1 && (atack + def) < countOfAttDef)  // start parametr atak + def = 6 - 1lvl = 5; + on each 7lvl + 1 point
            {
                if (state == 0) atack += 1;
                else def += 1;
                changes = true;
            }
            if (lvl % 14 == 0 && maxenergy < 5 + lvl / 14)
            {
                maxenergy += 1;
                changes = true;
            }
            return changes;
        }
    }
}