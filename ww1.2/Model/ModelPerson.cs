using System;
using System.Collections.Generic;
using System.Linq;
using wayofweapon.Entities;
using wayofweapon.CRUD;

namespace wayofweapon.Model
{
    class ModelPerson
    {
        CRUDPerson crudPerson;
        ModelInventory modelInventory;
        ModelItem modelItem;

        const double multiplierForOneWork = 1.2;
        const int chanseToFailOneWork = 7;
        const double goldFormWorkToGuildPersent = 0.1;

        public ModelPerson()
        {
            modelInventory = new ModelInventory();
            crudPerson = new CRUDPerson();
            modelItem = new ModelItem();
        }

        private Person GetObject(long personId)
        {
            Person person = crudPerson.Read(personId);
            if (person.guildId != null)
                person.guild = new CRUDGuild().Read(person.guildId.GetValueOrDefault());
            return person;
        }

        public string GetPersonNick(long personId)
        {
            return GetPerson(personId).personNick;
        }

        // true exist | false not
        public bool TryIfExist(long id)
        {
            try
            {
                if (GetPerson(id) == null)
                    return false;
            }
            catch (Exception) { return false; }
             return true;
        }

        //+
        public Person GetPerson(long id)
        {
            Person person = GetEntety(GetObject(id));
            TimeEnergy(person);
            return person;
        }

        internal int GetPersonsGuildCount(long id)
        {
            return crudPerson.GetPersonsGuild(id).Count();
        }

        internal void Update(Person person)
        {
            crudPerson.Update(person);
        }

        public List<Inventory> GetInventory(long id)
        {
            List<Inventory> inventory = modelInventory.GetPersonInventory(id);
            List<Inventory> fullitems = new List<Inventory>();
            if (inventory != null)
                foreach (Inventory item in inventory)
                {
                    Inventory newitem = modelInventory.GetObject(item.id);
                    newitem.item = modelItem.GetObject(item.itemId);
                    newitem.person = GetObject(item.personId);
                    newitem.count = item.count;
                    newitem.eqiup = item.eqiup;
                    fullitems.Add(newitem);
                }
            return fullitems;
        }

        internal int atackAdditional(long personId)
        {
            List<int> col = Inventories(personId)?.Where(x => x.eqiup == true).Select(x => x.item.attack).ToList();
            return col == null ? 0 : col.Sum();
        }

        public List<Inventory> Inventories(long personId)
        {
            return modelInventory.GetPersonInventory(personId);
        }

        internal int defAdditional(long personId)
        {
            List<int> col = Inventories(personId)?.Where(x => x.eqiup == true).Select(x => x.item.def).ToList();
            return col == null ? 0 : col.Sum();
        }

        public bool LvlUp(long id, int pointSwichBetvinAttackDef)
        {
            Person person = GetPerson(id);
            if (person.LvlUp(pointSwichBetvinAttackDef))
            {
                crudPerson.Update(person);
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
            new CRUDUser().Create(new User(id, userName));
            Person person = forEnteties();
            crudPerson.Create(person);
            return GetPerson(person.id);
        }

        public int AddToPersonExp(int lvl, double GetMultiplierExp, int useEnergy = 1)
        {
            double myltE = (double)new Random().Next(lvl * 2 + 1, lvl * 2 + (lvl + 2)) * GetMultiplierExp;
            myltE = (useEnergy == 1) ? myltE : myltE * (useEnergy * multiplierForOneWork);
            return Convert.ToInt32(myltE);
        }

        internal Person GetObjectByPersonNick(string personNick)
        {
            return crudPerson.GetObjects().Where(x=> x.personNick == personNick).FirstOrDefault();
        }

        public int AddToPersonGold(int lvl, double GetMultiplierGold, int useEnergy = 1)
        {
            double myltG = (double)new Random().Next(lvl * 2 + 1, lvl * 2 + (lvl + 2)) * GetMultiplierGold;
            myltG = (useEnergy == 1) ? myltG : myltG * (useEnergy * multiplierForOneWork);
            return Convert.ToInt32(myltG);
        }

        //+
        public Person Work(long id, out int goldOld, out int expOld, out bool lvlUp, int useEnergy = 1, Guild guild = null)
        {
            Person person = GetPerson(id);
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
                    return null;
                }

                int sallary = AddToPersonGold(person.lvl, person.GetMultiplierGold(), useEnergy);
                if (guild != null)
                {
                    person.gold += Convert.ToInt32(sallary - sallary * goldFormWorkToGuildPersent);    // from [lvl, lvl * 2 + 10] exmpl: 1lvl [1, 12] 2lvl [2, 14] 
                    guild.gold += Convert.ToInt32(sallary * goldFormWorkToGuildPersent);
                    new CRUDGuild().Update(guild);
                }
                else person.gold += sallary;

                person.exp += AddToPersonExp(person.lvl, person.GetMultiplierExp(), useEnergy);

                if ((person.exp - person.GetExpToNextLVL()) >= 0)                    //lvl up  when: exp < 4(max multiplier) * 6(energy + 1) * lvl
                {
                    person.lvl += 1;
                    lvlUp = true;
                }
                crudPerson.Update(person);
            }
            if (lvl != person.lvl)
            {
                crudPerson.Update(person);
                LvlUp(person.id, -1);
            }
            TimeEnergy(person);
            return person;
        }

        internal List<Person> GetPersonsGuild(long v)
        {
            return crudPerson.GetPersonsGuild(v);
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
                crudPerson.Update(person);
                return true;
            }
            return false;
        }
        
        public Person GetEntety(Person person)
        {
            switch (person.race)
            {
                case "Person":
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
            Person person = crudPerson.Read(id);
            Item item = modelItem.GetObject(Convert.ToInt64(idIteam));
            if (item != null)
                if (person.gold > item.cost)
                {
                    person.gold -= item.cost;
                    crudPerson.Update(person);
                    AddItemToPerson(id, Convert.ToInt64(idIteam));
                    return true;
                }
            return false;
        }

        public bool AddItemToPerson(long id, long idIteam)
        {
            Person person = crudPerson.Read(id);
            Item item = modelItem.GetObject(idIteam);
            modelInventory.AddToInventory(person,item);
            return true;
        }

        public bool SellItem(long idperson, long iditem)
        {
            modelInventory.DeleteIteamFromInventory(idperson,iditem);
            return true;
        }


        public bool EquipItem(long idperson, long iditem)
        {
            Inventory inventoryToChange = modelInventory.GetPersonItem(iditem, idperson);
            if (inventoryToChange == null) return false;
            Person person = GetObject(idperson);
            var type = (inventoryToChange.item.type == "swords" || inventoryToChange.item.type == "bones" || inventoryToChange.item.type == "mace"
                || inventoryToChange.item.type == "axe") ? true : false;
            var wepon = false;
            Inventory inventotyNeeded = null;
            List<Inventory> inv = modelInventory.GetPersonInventory(person.id);
            foreach (var inventory in inv)
            {
                if (type)
                {
                    // equip weapon
                    if ((inventory.item.type == "swords" || inventory.item.type == "bones" || inventory.item.type == "mace" ||
                         inventory.item.type == "axe") && inventory.eqiup == true)
                    {
                        inventotyNeeded = inventory;
                        wepon = true;
                    }
                }
                else if (inventory.id == iditem) // equip amo
                {
                    Inventory itemEquipped = modelInventory.GetIfHadeSameTypeEquipped(idperson, inventoryToChange.item.type);
                    if (itemEquipped != null)
                    {
                        itemEquipped.eqiup = false;
                        modelInventory.UpdatePersonInventory(itemEquipped);
                    }
                }
            }

            if (wepon)
            {
                inventotyNeeded.eqiup = false;
                modelInventory.UpdatePersonInventory(inventotyNeeded);
            }

            inventoryToChange.eqiup = true;
            modelInventory.UpdatePersonInventory(inventoryToChange);

            return true;
        }

        /*
        public bool EquipItem(long idperson, long iditem)
        {
            Item item = modelInventory.GetPersonItem(iditem, idperson);
            if (item == null) return false;
            Inventory itemEquipped = modelInventory.GetIfHadeSameTypeEquipped(idperson, item.type);
            if (itemEquipped != null)
                itemEquipped.eqiup = false;
            itemEquipped.eqiup = true;
            modelInventory.UpdatePersonInventory(itemEquipped);

            return true;
            /*
            List<Inventory> inventories = modelInventory.GetPersonInventory(idperson);
            var type = (item.type == "swords" || item.type == "bones" || item.type == "mace" || item.type == "axe")? true : false;
            var wepon = false;
            Inventory itemweapon = null;
            foreach (var inven in inventories)
            {
                
                if (type)
                {
                    // equip weapon
                    if ((inven.item.type == "swords" || inven.item.type == "bones" || inven.item.type == "mace" ||
                         inven.item.type == "axe") && inven.eqiup == true)
                    {
                        itemweapon = inven;
                        itemweapon.eqiup = false;
                        modelInventory.UpdatePersonInventory(idperson, itemweapon);
                    }
                }
                else if (inven.id == iditem) // equip amo
                {
             
                Inventory itemEquipped = modelInventory.GetIfHadeSameTypeEquipped(idperson, item.type);
                if (itemEquipped != null)
                {
                    itemEquipped.eqiup = false;
                    modelInventory.UpdatePersonInventory(itemEquipped);
                }
            }
            }
               

        }
    */

        public bool SetFraction(long idperson, int allianceOrRepublic)
        {
            try
            {
                Person person = GetPerson(idperson);
                person.fraction = allianceOrRepublic;
                crudPerson.Update(person);
                return true;
            }
            catch (Exception e) { return false; }
        }

        public List<Person> GetObjects()
        {
            return crudPerson.GetObjects();
        }
    }
}
