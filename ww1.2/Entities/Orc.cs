namespace wayofweapon.Entities
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

        public Orc()
        {

        }
        public Orc(Person person) : base(person)
        {
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
            int countOfAttDef = startCountOfParams + (lvl); // we start from 1 and dont add any 
            int countOfAttDefANDspechialparams = startCountOfParams + (lvl) + (lvl/each7Lvl) ;

            if (lvl % each7Lvl == 0)
            {
                if (countOfAttDefANDspechialparams > (atack + def) && state != -1)
                {
                    if (state == 0) atack += 1;
                    else def += 1;
                    changes = true;
                }

                if (countOfAttDefANDspechialparams - 1 == (atack + def ))
                {
                    def += 1;
                    changes = true;
                }
            }
            else
            {
                if(countOfAttDefANDspechialparams >= (atack+def) && state != -1)
                {
                    if (state == 0) atack += 1;
                    else def += 1;
                    changes = true;
                }
            }


            /*
            if (state != -1 && (atack + def) <= countOfAttDef)  // start parametr atak + def = 6 - 1lvl = 5; + on each 7lvl + 1 point
            {
                if (state == 0) atack += 1;
                else def += 1;
                changes = true;
            }

            if (lvl % each7Lvl == 0 && (atack + def) == countOfAttDefANDspechialparams)
            {
                def += 1;
                changes = true;
            }
            */
            if (lvl % 14 == 0 && maxenergy < (6 + lvl / 14))
            {
                maxenergy += 1;
                changes = true;
            }
            return changes;
        }
    }
}
