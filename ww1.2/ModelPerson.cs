using System;
using System.Collections.Generic;
using System.Linq;

namespace revcom_bot
{
    class ModelPerson
    {
        DaoUser daoUser;
        DaoPerson daoPerson;
        Person person;
        Item item;
        private ModelIteam _modelIteam;

        const double multiplierForOneWork = 1.2;
        const int chanseToFailOneWork = 7;
        const double goldFormWorkToGuildPersent = 0.1;

        public ModelPerson()
        {
            _modelIteam = new ModelIteam();
            daoUser = new DaoUser();
            daoPerson = new DaoPerson();
        }

        public string GetPersonNick(long personId)
        {
            return daoPerson.GetObject(personId).personNick;
        }

        // true exist | false not
        public bool TryIfExist(long id)
        {
            try
            {
                if (daoPerson.GetObject(id) == null)
                    return false;
            }
            catch (Exception) { return false; }
             return true;
        }

        //+
        public Person GetMe(long id)
        {
            person = GetEntety(daoPerson.GetObject(id));
            TimeEnergy(person);
            person.items = GetInventory(id);
            if (person.items != null)
                foreach (Item item in person.items)
                {
                    if (item.eqiup == true)
                    {
                        Item item1 = _modelIteam.GetObject(item.id);
                        person.atackAdditional += item1.attack;
                        person.defAdditional += item1.def;
                    }
                }
            return person;
        }

        internal int GetPersonsGuildCount(long id)
        {
            return daoPerson.GetPersonsGuild(id).Count();
        }

        internal void Update(Person person)
        {
            daoPerson.Update(person);
        }

        public List<Item> GetInventory(long id)
        {
            List<Item> items = _modelIteam.GetPersonInventory(id);
            List<Item> fullitems = new List<Item>();
            if (items != null)
                foreach (Item item in items)
                {
                    Item newitem = _modelIteam.GetObject(item.id);
                    newitem.count = item.count;
                    newitem.eqiup = item.eqiup;
                    fullitems.Add(newitem);
                }
            return fullitems;
        }

        internal void UpdateGuild(Person person)
        {
            daoPerson.UpdateGuild(person);
        }

        public bool LvlUp(long id, int pointSwichBetvinAttackDef)
        {
            person = GetEntety(GetMe(id));
            if (person.LvlUp(pointSwichBetvinAttackDef))
            {
                daoPerson.Update(person);
                return true;
            }
            return false;
        }

        delegate Person ForEnteties();
        //+
        public Person CreateNewUser(long id, string userName, string race)
        {
            ForEnteties forEnteties = delegate ()
            {
                switch (race)
                {
                    case "Person":
                        return new People(id, userName);
                    case "Gnome":
                        return new Gnom(id, userName);
                    case "Orc":
                        return new Orc(id, userName);
                    case "Elf":
                        return new Elf(id, userName);
                    default:
                        return new Person(id, userName);
                }
            };
            daoUser.Add(new User(id, userName));
            daoPerson.Add(forEnteties());
            person = daoPerson.GetObject(id);
            return person;
        }

        public int AddToPersonExp(int lvl, double GetMultiplierExp, int useEnergy = 1)
        {
            double myltE = (double)new Random().Next(lvl * 2, lvl * 2 + (lvl + 1)) * GetMultiplierExp;
            myltE = (useEnergy == 1) ? myltE : myltE * (useEnergy * multiplierForOneWork);
            return Convert.ToInt32(myltE);
        }

        internal Person GetObjectByPersonNick(string personNick)
        {
            return daoPerson.GetObjectByPersonNick(personNick);
        }

        public int AddToPersonGold(int lvl, double GetMultiplierGold, int useEnergy = 1)
        {
            double myltG = (double)new Random().Next(lvl * 2, lvl * 2 + (lvl + 1)) * GetMultiplierGold;
            myltG = (useEnergy == 1) ? myltG : myltG * (useEnergy * multiplierForOneWork);
            return Convert.ToInt32(myltG);
        }

        //+
        public Person Work(long id, out int goldOld, out int expOld, out bool lvlUp, int useEnergy = 1, Guild guild = null)
        {
            person = GetMe(id);
            goldOld = person.gold;
            expOld = person.exp;
            lvlUp = false;
            int lvl = person.lvl;
            if ((person.energy - useEnergy) >= 0)
            {
                if (person.energy >= person.maxenergy)
                    person.energytime = DateTime.Now.AddHours(useEnergy);
                else person.energytime = person.energytime.AddHours(useEnergy);

                person.energy -= useEnergy;

                if (useEnergy != 1 && new Random().Next(0, 100) <= (chanseToFailOneWork * useEnergy))
                {
                    daoPerson.UpdateWork(person);
                    return null;
                }

                int sallary = AddToPersonGold(person.lvl, person.GetMultiplierGold(), useEnergy);
                if (guild != null)
                {
                    person.gold += Convert.ToInt32(sallary - sallary * goldFormWorkToGuildPersent);    // from [lvl, lvl * 2 + 10] exmpl: 1lvl [1, 12] 2lvl [2, 14] 
                    guild.gold += Convert.ToInt32(sallary * goldFormWorkToGuildPersent);
                    new DaoGuild().UpdateGold(guild);
                }
                else person.gold += sallary;

                person.exp += AddToPersonExp(person.lvl, person.GetMultiplierExp(), useEnergy);

                if ((person.exp - person.GetExpToNextLVL()) >= 0)                    //lvl up  when: exp < 4(max multiplier) * 6(energy + 1) * lvl
                {
                    person.lvl += 1;
                    lvlUp = true;
                }
                daoPerson.UpdateWork(person);
            }
            if (lvl != person.lvl)
            {
                daoPerson.UpdateLvl(person);
                LvlUp(person.id, -1);
            }
            TimeEnergy(person);
            return person;
        }

        internal List<Person> GetPersonsGuild(long v)
        {
            return daoPerson.GetPersonsGuild(v);
        }

        public List<Person> GetPersonThatHaveFullEnergy()
        {
            return null;
        }

        //+
        public bool TimeEnergy(Person person)
        {
            bool state = true;
            int changes = 0;
            while (state)
            {
                int z = (person.maxenergy - person.energy);
                DateTime now = DateTime.Now;
                DateTime expectedTime = person.energytime.AddHours(-z + 1);
                int timestate = (expectedTime).CompareTo(now);
                if (person.energy < person.maxenergy && timestate <= 0)
                    person.energy += 1;
                else break;
                changes += 1;
            }
            if (changes != 0)
            {
                daoPerson.UpdateTime(person);
                return true;
            }
            return false;
        }

        private bool CompareDateTime(DateTime dateTime1, DateTime dateTime2)
        {
            if (dateTime1.CompareTo(dateTime2) <= 0)
                return false;
            else return true;
        }

        public Person GetEntety(Person person)
        {
            switch (person.race)
            {
                case "Peopl":
                    return new People(person);
                case "Gnom":
                    return new Gnom(person);
                case "Orc":
                    return new Orc(person);
                case "Elf":
                    return new Elf(person);
                default:
                    return new Person(person);
            }
        }

        public bool ByItem(long id, int idIteam)
        {
            person = daoPerson.GetObject(id);
            item = _modelIteam.GetObject(Convert.ToInt64(idIteam));
            if (item != null)
                if (person.gold > item.cost)
                {
                    person.gold -= item.cost;
                    daoPerson.Update(person);
                    AddItemToPerson(id, Convert.ToInt64(idIteam));
                    return true;
                }
            return false;
        }

        public bool AddItemToPerson(long id, long idIteam)
        {
            List<Item> items = _modelIteam.GetPersonInventory(id);
            if (items != null)
                foreach (Item item in items)
                    if (item.id == idIteam)
                    {
                        item.count += 1;
                        _modelIteam.UpdatePersonInventory(id, item);
                        return true;
                    }
            _modelIteam.AddToInventory(id, idIteam);
            return true;
        }

        public bool SellItem(long idperson, long iditem)
        {
            List<Item> items = _modelIteam.GetPersonInventory(idperson);
            if (items != null)
                foreach (Item item in items)
                    if (item.id == iditem)
                    {
                        item.count -= 1;
                        if (item.count == 0) _modelIteam.DeleteIteamFromInventory(idperson, item.id);
                        else _modelIteam.UpdatePersonInventory(idperson, item);
                        Item itemInfo = _modelIteam.GetObject(item.id);
                        person = daoPerson.GetObject(idperson);     // we need just add (item cost) / 2. to person's gold
                        person.gold += itemInfo.cost / 2;
                        daoPerson.Update(person);     // it update exp and gold
                        return true;
                    }
            return false;
        }

        public bool EquipItem(long idperson, long iditem)
        {
            Item item = _modelIteam.GetPersonItem(iditem, idperson);
            if (item == null) return false;
            person = GetMe(idperson);
            var type = (item.type == "swords" || item.type == "bones" || item.type == "mace" || item.type == "axe")? true : false;
            var wepon = false;
            Item itemweapon = null;
            foreach (var personsItem in person.items)
            {
                if (type)
                {
                    // equip weapon
                    if ((personsItem.type == "swords" || personsItem.type == "bones" || personsItem.type == "mace" ||
                         personsItem.type == "axe") && personsItem.eqiup == true)
                    {
                        itemweapon = personsItem;
                        wepon = true;
                    }
                }
                else if (personsItem.id == iditem) // equip amo
                {
                    Item itemEquipped = _modelIteam.GetIfHadeSameTypeEquipped(idperson, item.type);
                    if (itemEquipped != null)
                    {
                        itemEquipped.eqiup = false;
                        _modelIteam.UpdatePersonInventory(idperson, itemEquipped);
                    }
                }
            }

            if (wepon)
            {
                itemweapon.eqiup = false;
                _modelIteam.UpdatePersonInventory(idperson, itemweapon);
            }

            item.eqiup = true;
            _modelIteam.UpdatePersonInventory(idperson, item);

            return true;
        }

        public bool SetFraction(long idperson, int allianceOrRepublic)
        {
            try
            {
                person = daoPerson.GetObject(idperson);
                person.fraction = allianceOrRepublic;
                daoPerson.Update(person);
                return true;
            }
            catch (Exception e) { return false; }
        }

        public List<Person> GetObjects()
        {
            return daoPerson.GetObjects();
        }

        public void UpdateWork(Person person1)
        {
            daoPerson.UpdateWork(person1);
        }

        public void UpdateStateOfWarNull(long personId)
        {
            daoPerson.UpdateStateOfWarNULL(personId);
        }
    }
}
