using System;
using System.Collections.Generic;
using System.Linq;

namespace revcom_bot
{
    class ModelGuild
    {
        readonly DaoGuild daoGuild;
        Person person;
        Guild guild;
        private readonly ModelPerson _modelPerson;

        public ModelGuild()
        {
            daoGuild = new DaoGuild();
            _modelPerson = new ModelPerson();
        }

        public bool TryIfGuildMaster(long userId)
        {
            Guild g = GuildGet(userId);
            person = _modelPerson.GetMe(userId);
            if (g == null) return false;
            if (g.master == person.personNick) return true;
            return false;
        }

        public Guild GuildGet(long id)
        {
            person = _modelPerson.GetMe(id);
            if (person == null) return null;
            guild = daoGuild.GetObject(person.guild.GetValueOrDefault());
            if (guild.master == null) return null;
            guild.count = _modelPerson.GetPersonsGuildCount(guild.id);
            return guild;
        }

        //Create a guild if person have 1000 gold && lvl >= 15.
        public Guild GuildCreate(long idperson, string name, out bool lvlIsSmal, out bool notEnouthGold)
        {
            person = _modelPerson.GetMe(idperson);
            lvlIsSmal = (person.lvl < 15) ? true : false;
            notEnouthGold = (person.gold <= 1000) ? true : false;
            if (lvlIsSmal || notEnouthGold) return null;
            Guild guild = new Guild(person.personNick, name)
            {
                fraction = person.fraction.GetValueOrDefault()
            };
            daoGuild.Add(guild);
            guild = daoGuild.GetGuildByName(name);
            if (guild == null) return null;
            person.gold -= 1000;
            person.guild = guild.id;
            _modelPerson.UpdateGuild(person);
            return guild;
        }

        public Guild GuildJoinOut(long idperson, long idguild)
        {
            person = _modelPerson.GetMe(idperson);
            if (person.lvl < 10) return null;
            person.guild = idguild;
            Guild guild = daoGuild.GetObject(idguild);
            if (person.fraction != guild.fraction) return null;
            _modelPerson.UpdateGuild(person);
            return guild;
        }

        public bool GuildLeave(long id)
        {
            person = _modelPerson.GetMe(id);
            if (person.guild == null) return false;
            person.guild = null;
            _modelPerson.UpdateGuild(person);
            return true;
        }

        public List<Guild> GetOpenGuild(long id)
        {
            person = new ModelPerson().GetMe(id);
            List<Guild> guilds = daoGuild.GetOpenGuild();
            if (guilds == null) return null;
            List<Guild> guildsFraction = new List<Guild>();
            foreach (Guild guild in guilds)
                if (guild.fraction == person.fraction)
                    guildsFraction.Add(guild);
            return guildsFraction;
        }

        public void ChangeHire(long userId)
        {
            guild = GuildGet(userId);
            guild.hire = !guild.hire;
            daoGuild.Update(guild);
        }

        public string GetChatLink(long userId)
        {
            return GuildGet(userId).chatUrl;
        }

        public bool SetChatLink(long userId, string chatUrl)
        {
            guild = GuildGet(userId);
            String s = guild.chatUrl + "";
            guild.chatUrl = chatUrl;
            daoGuild.Update(guild);
            Guild guild1 = daoGuild.GetObject(guild.id);
            if (guild1.chatUrl != "" && guild1.chatUrl != s) return true;
            return false;
        }

        public Person InvitePerson(long userId, string personNick, out Guild guild)
        {
            person = _modelPerson.GetObjectByPersonNick(personNick);
            guild = daoGuild.GetObject(_modelPerson.GetMe(userId).guild.GetValueOrDefault());
            if (person != null || person.fraction == guild.fraction) return person;
            return null;
        }

        public Person ExcludePerson(long userId, string personNick, out bool gildMaster)
        {
            gildMaster = false;
            Person personToExlude = _modelPerson.GetObjectByPersonNick(personNick);
            Person personMaster = _modelPerson.GetMe(userId);
            if (personToExlude.personNick == personMaster.personNick || personToExlude.guild != personMaster.guild){ gildMaster = true; return null;}
            personToExlude.guild = null;
            _modelPerson.Update(person);
            personToExlude = _modelPerson.GetMe(personToExlude.id);
            return personToExlude.guild == null ? personToExlude : null;
        }

        public List<Person> GetMembers(long userId)
        {
            person = new ModelPerson().GetMe(userId);
            return _modelPerson.GetPersonsGuild(person.guild.GetValueOrDefault());
        }

        public Person Work(long id, int energyused, out int goldOld, out int expOld, out bool lvlUp)
        {
            person = _modelPerson.GetMe(id);
            Guild guild = daoGuild.GetObject(person.guild.GetValueOrDefault());
            person = _modelPerson.Work(id, out goldOld, out expOld, out lvlUp, energyused, guild);
            return person;
        }

        public bool WarIsStarting(long userId, bool attackOrDef)
        {
            if (!StateIfReadyToWar()) return false;
            person = _modelPerson.GetMe(userId);
            person.attackOrDef = attackOrDef;
            _modelPerson.Update(person);
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
            return daoGuild.GetObjects();
        }

        public void UpdateGold(Guild guild1)
        {
            daoGuild.UpdateGold(guild);
        }
    }
}
