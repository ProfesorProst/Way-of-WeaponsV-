namespace wayofweapon.Entities
{
    class Elf : Person
    {
        public Elf(long id, string personnick) : base(id, personnick)
        {
            this.energy = 4;
            this.maxenergy = 4;
            this.def = 2;
            this.atack = 4;
            this.race = "Elf";
        }
        public Elf()
        {

        }

        public Elf(Person person) : base(person)
        {
            this.race = "Elf";
        }

        public override double GetMultiplierExp()
        {
            return 1;
        }

        public override double GetMultiplierGold()
        {
            return 1.1;
        }

        public override bool LvlUp(int state)
        {
            //state = 0 - attack
            //state = 1 - def
            //state = -1 - none
            int startCountOfParams = 6;// start parametr atak + def = 6 
            int each7Lvl = 7; // on each 7lvl + 1 point; -1 point to use it
            int countOfAttDefANDspechialparams = startCountOfParams + (this.lvl) + (lvl / each7Lvl);

            if (state != -1)
                if (lvl % each7Lvl == 0)
                {
                    if (countOfAttDefANDspechialparams > (atack + def))
                    {
                        if (state == 0) atack += 1;
                        else def += 1;
                        changes = true;
                    }

                    if (countOfAttDefANDspechialparams - 1 == (atack + def))
                    {
                        atack += 1;
                        changes = true;
                    }
                }
                else
                {
                    if (countOfAttDefANDspechialparams >= (atack + def))
                    {
                        if (state == 0) atack += 1;
                        else def += 1;
                        changes = true;
                    }
                }

            if (lvl % 14 == 0 && maxenergy < 4 + lvl / 14)
            {
                maxenergy += 1;
                changes = true;
            }
            return changes;
        }
    }
}
