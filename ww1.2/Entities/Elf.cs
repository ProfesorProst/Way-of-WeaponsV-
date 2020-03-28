namespace wayofweapon.Entities
{
    class Elf : Person
    {
        public Elf(long id, string personnick) : base (id, personnick)
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
            int startCountOfParams = 6;// start parametr atak + def = 6 - 1lvl = 5;
            int each7Lvl = 7; // on each 7lvl + 1 point; -1 point to use it
            int countOfAttDef = startCountOfParams + (lvl); // we start from 1 and dont add any 
            int countOfAttDefANDspechialparams = startCountOfParams + (this.lvl) + (lvl / each7Lvl) - 1;

            if (state != -1 && (atack + def) < countOfAttDef)  // start parametr atak + def = 6 - 1lvl = 5; + on each 7lvl + 1 point
            {
                if (state == 0) atack += 1;
                else def += 1;
                changes = true;
            }

            if (lvl % each7Lvl == 0 && (atack + def) == countOfAttDefANDspechialparams)
            {
                atack += 1;
                changes = true;
            }
            if (lvl % 14 == 0 && maxenergy < 4 + lvl/14 )
            {
                maxenergy += 1;
                changes = true;
            }
            return changes;
        }
    }
}
