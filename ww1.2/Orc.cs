namespace revcom_bot
{
    class Orc : Person
    {
        public Orc(long id, string personnick) : base(id, personnick)
        { 
            this.energy = 6;
            this.maxenergy = 6;
            this.def = 4;
            this.atack = 2;
            this.race = "Orc";
        }

        public Orc(Person person) : base(person)
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
            this.race = "Orc";
        }

        public override double GetMultiplierExp()
        {
            return 1.15;
        }

        public override double GetMultiplierGold()
        {
            return 0.75;
        }

        public override bool LvlUp(int state)
        {
            int startCountOfParams = 6;// start parametr atak + def = 6 - 1lvl = 5;
            int each7Lvl = 7; // on each 7lvl + 1 point; -1 point to use it
            int countOfAttDef = startCountOfParams + (lvl - 1); // we start from 1 and dont add any 
            int countOfAttDefANDspechialparams = startCountOfParams + (lvl - 1) + (lvl/each7Lvl);

            if (state != -1 && (atack + def) < countOfAttDef)  // start parametr atak + def = 6 - 1lvl = 5; + on each 7lvl + 1 point
            {
                if (state == 0) atack += 1;
                else def += 1;
                changes = true;
            }

            if (lvl % each7Lvl == 0 && (atack + def) < countOfAttDefANDspechialparams)
            {
                def += 1;
                changes = true;
            }

            if (lvl % 14 == 0 && maxenergy < (6 + lvl / 14))
            {
                maxenergy += 1;
                changes = true;
            }
            return changes;
        }
    }
}
