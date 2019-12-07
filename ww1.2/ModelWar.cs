﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace revcom_bot
{
    class ModelWar
    {
        readonly ModelPerson _modelPerson;
        private ModelGuild modelGuild;
        private ModelIteam _modelIteam;

        public ModelWar()
        {
            _modelPerson = new ModelPerson();
            modelGuild = new ModelGuild(); 
            _modelIteam = new ModelIteam();
        }

        public List<Person> WarStart(out List<Person> peopleOld)
        {
            peopleOld = _modelPerson.GetObjects();
            List<Person> people = _modelPerson.GetObjects();

            List<Person> fractionImpire = new List<Person>();
            List<Person> fractionImpireAtt = new List<Person>();
            List<Person> fractionImpireDef = new List<Person>();
            int impLvlAtt = 0, impLvlDef = 0, impAtt = 0, impDef = 0;

            List<Person> fractionRepublic = new List<Person>();
            List<Person> fractionRepublicAtt = new List<Person>();
            List<Person> fractionRepublicDef = new List<Person>();

            List<Person> winer1 = new List<Person>();
            List<Person> loser1 = new List<Person>();
            List<Person> winer2 = new List<Person>();
            List<Person> loser2 = new List<Person>();

            Item[] items1 = null;
            Item[] items2 = null;

            List<Guild> allGuilds = modelGuild.GetObjects();
            List<Guild> impGuilds = new List<Guild>();
            List<Guild> repGuilds = new List<Guild>();

            List<Guild> winer1Guilds = new List<Guild>();
            List<Guild> winer2Guilds = new List<Guild>();
            int repLvlAtt = 0, repLvlDef = 0, repAtt = 0, repDef = 0;

            int cash1 = 0, cash2 = 0, winer1Lvl = 1, winer2Lvl = 1;

            foreach (Person person in people)
            {
                if (person.fraction == null || person.lvl < 10) continue;
                Person personFull = _modelPerson.GetMe(person.id);
                if (person.fraction == person.GetAlliance())
                {
                    fractionImpire.Add(personFull);
                    if (personFull.attackOrDef == true)
                    {
                        impAtt += personFull.atack + personFull.atackAdditional;
                        impLvlAtt += personFull.lvl;
                        fractionImpireAtt.Add(person);
                    }
                    else if (personFull.attackOrDef == false)
                    {
                        impDef += personFull.def + personFull.defAdditional;
                        impLvlDef += personFull.lvl;
                        fractionImpireDef.Add(person);
                    }
                }
                else if (person.fraction == person.GetRepublic())
                {
                    fractionRepublic.Add(personFull);
                    if (personFull.attackOrDef == true)
                    {
                        repAtt += personFull.atack + personFull.atackAdditional;
                        repLvlAtt += personFull.lvl;
                        fractionRepublicAtt.Add(person);
                    }
                    else if(personFull.attackOrDef == false)
                    {
                        repDef += personFull.def + personFull.defAdditional;
                        repLvlDef += personFull.lvl;
                        fractionRepublicDef.Add(person);
                    }
                }
                //personFull.exp += _modelPerson.AddToPersonExp(person.lvl, person.GetMultiplierExp()) * 2;
                //person.changes = true;
            }

            foreach (Guild guild in allGuilds)
            {
                if (guild.fraction == 0)
                    impGuilds.Add(guild);
                else if (guild.fraction == 1) repGuilds.Add(guild);
            }

            {
                if (impAtt >= repDef)
                {
                    cash1 = GetGoldPerson(ref fractionRepublic);
                    cash1 += GetGoldGuild(ref repGuilds);
                    items1 = null;
                    winer1 = fractionImpireAtt;
                    winer1Lvl = (impLvlAtt == 0)? 1 : impLvlAtt;
                    winer1Guilds = impGuilds;
                }
                else
                {
                    cash1 = GetGoldPerson(ref fractionImpireAtt);
                    items1 = GetIteams(ref fractionImpireAtt);
                    winer1 = fractionRepublicDef;
                    winer1Lvl = (repLvlDef == 0) ? 1 : repLvlDef;
                    winer1Guilds = repGuilds;
                }

                if (repAtt >= impDef)
                {
                    cash2 = GetGoldPerson(ref fractionImpire);
                    cash2 += GetGoldGuild(ref impGuilds);
                    items2 = null;
                    winer2 = fractionRepublicAtt;
                    winer2Lvl = (repLvlAtt == 0) ? 1 : repLvlAtt;
                    winer2Guilds = repGuilds;
                }
                else
                {
                    cash2 = GetGoldPerson(ref fractionRepublicAtt);
                    items2 = GetIteams(ref fractionRepublicAtt);
                    winer2 = fractionImpireDef;
                    winer2Lvl = (impLvlDef == 0) ? 1 : impLvlDef;
                    winer2Guilds = impGuilds;
                }
            }

            SetGoldForWiner(cash1/ winer1Lvl, items1, winer1, winer1Guilds);
            SetGoldForWiner(cash2/ winer2Lvl, items2, winer2, winer2Guilds);

            CleanWarStatus(people);

            return _modelPerson.GetObjects();
        }

        private void SetGoldForWiner(double moneyForLvl, Item[] items, List<Person> winer, List<Guild> guilds)
        {
            foreach(Person person in winer)
            {
                person.exp += _modelPerson.AddToPersonExp(person.lvl, person.GetMultiplierExp()) * 2;
                person.gold += Convert.ToInt32(person.lvl * moneyForLvl - person.lvl * moneyForLvl * 0.1);
                SetGuildGold(ref guilds, person.guild.GetValueOrDefault(), Convert.ToInt32(person.lvl * moneyForLvl * 0.1));
                _modelPerson.UpdateWork(person);
            }

            if(items != null)
                foreach(Item item in items)
                {
                    Random random = new Random();
                    int number = random.Next(0,winer.Count());
                    Person person = winer[number];
                    _modelPerson.AddItemToPerson(person.id, item.id);
                    winer.Remove(person);
                }

        }

        private void SetGuildGold(ref List<Guild> guilds, long guildId, int gold)
        {
            foreach(Guild guild in guilds)
            {
                if(guild.id == guildId)
                {
                    guild.gold += gold;
                }
            }
        }

        private int GetGoldGuild(ref List<Guild> guilds)
        {
            int cash = 0;
            if (!guilds.Any()) return 0;
            foreach (Guild guild in guilds)
            {
                guild.gold -= Convert.ToInt32(guild.gold * 0.6);
                cash += Convert.ToInt32(guild.gold * 0.6);
                modelGuild.UpdateGold(guild);
            }
            return cash;
        }

        private int GetGoldPerson(ref List<Person> people)
        {
            int cash = 0;
            if(!people.Any()) return 0;
            foreach (Person person in people)
            {
                person.gold -= Convert.ToInt32(person.gold * 0.5);
                cash += Convert.ToInt32(person.gold * 0.5);
                _modelPerson.UpdateWork(person);
            }
            return cash;
        }

        private Item[] GetIteams(ref List<Person> people)
        {
            Item[] items = new Item[0];
            if (!people.Any()) return null;
            foreach (Person person in people)
            {
                if(person.items != null)
                {
                    var rnd = new Random();
                    int randind = rnd.Next(0, 100);
                    if(randind <= 10)
                    {
                        var rand = new Random();
                        int randIteam = rand.Next(0, person.items.Count());
                        Item item = person.items[randIteam];
                        item.count -= 1;
                        if (item.count == 0) _modelIteam.DeleteIteamFromInventory(person.id, item.id);
                        else _modelIteam.UpdatePersonInventory(person.id, item);
                        Array.Resize(ref items, items.Length + 1);
                        items[items.Length - 1] = item;
                    }
                }
            }
            return items;
        }

        private void CleanWarStatus(List<Person> people)
        {
            foreach(Person person in people)
                _modelPerson.UpdateStateOfWarNull(person.id);
        }
    }
}
