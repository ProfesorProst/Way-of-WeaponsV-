using System;
using System.Collections.Generic;
using System.Linq;
using wayofweapon.CRUD;
using wayofweapon.Entities;

namespace wayofweapon.Model
{
    class ModelGuild
    {
        CRUDGuild crudguild;
        ModelPerson modelPerson;

        public ModelGuild()
        {
            crudguild = new CRUDGuild();
            modelPerson = new ModelPerson();
        }

        public bool TryIfGuildMaster(long userId)
        {
            Guild g = GuildGet(userId);
            Person person = modelPerson.GetPerson(userId);
            if (g == null) return false;
            if (g.master == person.personNick) return true;
            return false;
        }

        public Guild GuildGet(long personId)
        {
            Person person = modelPerson.GetPerson(personId);
            if (person.guild == null) return null;
            Guild guild = crudguild.Read(person.guild.id);
            if (guild.master == null) return null;
            guild.count = modelPerson.GetPersonsGuildCount(guild.id);
            return guild;
        }

        //Create a guild if person have 1000 gold && lvl >= 15.
        public Guild GuildCreate(long idperson, string name, out bool lvlIsSmal, out bool notEnouthGold)
        {
            Person person = modelPerson.GetPerson(idperson);
            lvlIsSmal = (person.lvl < 15) ? true : false;
            notEnouthGold = (person.gold <= 1000) ? true : false;
            if (lvlIsSmal || notEnouthGold) return null;
            Guild guild = new Guild(person.personNick, name)
            {
                fraction = person.fraction.GetValueOrDefault()
            };
            crudguild.Create(guild);
            guild = crudguild.GetObjects().Where(x => x.name == guild.name).FirstOrDefault();
            if (guild == null) return null;
            person.gold -= 1000;
            person.guild = guild;
            person.guildId = guild.id;
            modelPerson.Update(person);
            return guild;
        }

        public Guild GuildJoinOut(long idperson, long idguild)
        {
            Person person = modelPerson.GetPerson(idperson);
            if (person.lvl < 10) return null;
            Guild guild = crudguild.Read(idguild);
            person.guild = guild;
            if (person.fraction != guild.fraction) return null;
            modelPerson.Update(person);
            guild = GuildGet(idperson);
            return guild;
        }

        public bool GuildLeave(long id)
        {
            Person person = modelPerson.GetPerson(id);
            if (person.guild == null) return false;
            person.guild = null;
            modelPerson.Update(person);
            return true;
        }

        public List<Guild> GetOpenGuild(long id)
        {
            Person person = new ModelPerson().GetPerson(id);
            List<Guild> guilds = crudguild.GetObjects().Where(x => x.hire = true).ToList();
            if (guilds == null) return null;
            List<Guild> guildsFraction = new List<Guild>();
            foreach (Guild guild in guilds)
                if (guild.fraction == person.fraction)
                    guildsFraction.Add(guild);
            return guildsFraction;
        }

        public void ChangeHire(long userId)
        {
            Guild guild = GuildGet(userId);
            guild.hire = !guild.hire;
            crudguild.Update(guild);
        }

        public string GetChatLink(long userId)
        {
            return GuildGet(userId).chatUrl;
        }

        public bool SetChatLink(long userId, string chatUrl)
        {
            Guild guild = GuildGet(userId);
            String s = guild.chatUrl + "";
            guild.chatUrl = chatUrl;
            crudguild.Update(guild);
            Guild guild1 = crudguild.Read(guild.id);
            if (guild1.chatUrl != "" && guild1.chatUrl != s) return true;
            return false;
        }

        public Person InvitePerson(long userId, string personNick, out Guild guild)
        {
            Person person = modelPerson.GetObjectByPersonNick(personNick);
            guild = crudguild.Read(modelPerson.GetPerson(userId).guild.id);
            if (person != null || person.fraction == guild.fraction) return person;
            return null;
        }

        public Person ExcludePerson(long userId, string personNick, out bool gildMaster)
        {
            gildMaster = false;
            Person personToExlude = modelPerson.GetObjectByPersonNick(personNick);
            Person personMaster = modelPerson.GetPerson(userId);
            if (personToExlude.personNick == personMaster.personNick || personToExlude.guildId != personMaster.guild.id) { gildMaster = true; return null; }
            personToExlude.guild = null;
            personToExlude.guildId = null;
            modelPerson.Update(personToExlude);
            personToExlude = modelPerson.GetPerson(personToExlude.id);
            return personToExlude.guild == null ? personToExlude : null;
        }

        public List<Person> GetMembers(long userId)
        {
            Person person = new ModelPerson().GetPerson(userId);
            return modelPerson.GetPersonsGuild(person.guild.id);
        }

        public Person Work(long id, int energyused, out int goldOld, out int expOld, out bool lvlUp)
        {
            Person person = modelPerson.GetPerson(id);
            Guild guild = crudguild.Read(person.guild.id);
            person = modelPerson.Work(id, out goldOld, out expOld, out lvlUp, energyused, guild);
            return person;
        }

        public bool WarIsStarting(long userId, bool attackOrDef)
        {
            if (!StateIfReadyToWar()) return false;
            Person person = modelPerson.GetPerson(userId);
            //person.attackOrDef = attackOrDef;
            modelPerson.Update(person);
            return true;
        }

        private bool StateIfReadyToWar()
        {
            DateTime localDate = DateTime.Now;
            DateTime startReadyWar = new DateTime(2018, 9, 9, 18, 50, 0);
            DateTime endReadyWar = new DateTime(2018, 9, 9, 19, 0, 0);
            return localDate.Hour >= startReadyWar.Hour && localDate.Hour < endReadyWar.Hour && localDate.Minute >= startReadyWar.Minute;
        }

        public List<Guild> GetObjects()
        {
            return crudguild.GetObjects();
        }

        internal void Update(Guild guild)
        {
            crudguild.Update(guild);
        }
    }
}
