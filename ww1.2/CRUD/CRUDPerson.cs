using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using MySql.Data.MySqlClient;
using wayofweapon.Data;
using wayofweapon.Entities;

namespace wayofweapon.CRUD
{
    class CRUDPerson : CRUD<Person>
    {
        Context context;

        public CRUDPerson()
        {
            context = new Context();
        }
        public void Create(Person person)
        {
            context = new Context();
            context.people.Add(person);
            context.SaveChanges();
        }
        
        public Person Read(long id)
        {
            context = new Context();
            return context.people.Find(id);
        }
        
        public void Update1(Person person)
        {
            var entity = context.people.Find(person.id);
            if (entity == null)
                return;
            if (person.guild != null)
            {
                person.guild = context.guilds.SingleOrDefault(x => x.id == person.guild.id);
                //person.guildId = person.guild.id;
                //context.Entry(person.guild).CurrentValues.SetValues(context.guilds.SingleOrDefault(x => x.id == person.guild.id));
            }
            else if (person.guildId != null)
            {
                person.guild = context.guilds.Where(x => x.id == person.guildId.GetValueOrDefault()).SingleOrDefault();
                person.guildId = entity.guild.id;
            }

            context.Entry(entity).CurrentValues.SetValues(person);
            //context.people.AddOrUpdate(person);
            context.SaveChanges();
        }

        public void Update(Person person)
        {
            Context context = new Context();
            context.SaveChanges();
            var entity = context.people.Find(person.id);
            if (person.guild != null)
            {
                person.guild = context.guilds.SingleOrDefault(x => x.id == person.guild.id);
                person.guildId = person.guild.id;
                //context.Entry(person.guild).CurrentValues.SetValues(context.guilds.SingleOrDefault(x => x.id == person.guild.id));
            }
            else if (person.guildId != null)
            {
                long z = person.guildId.Value;
                person.guild = context.guilds.SingleOrDefault(x => x.id == z);
                person.guildId = entity.guild.id;
            }
            //entity.guild = context.guilds.Where(x=> x.id == person.guild.id).SingleOrDefault();
            //entity.guildId = context.guilds.Where(x => x.id == person.guild.id).SingleOrDefault().id;
            if (entity == null)
                return;

            context.Entry(entity).CurrentValues.SetValues(person);
            context.SaveChanges();
            return;
            entity.hp = person.hp;
            entity.energy = person.energy;
            entity.energytime = person.energytime;
            entity.maxenergy = person.maxenergy;
            entity.exp = person.exp;
            entity.gold = person.gold;
            entity.def = person.def;
            entity.atack = person.atack;
            entity.personNick = person.personNick;
            entity.fraction = person.fraction;
            entity.lvl = person.lvl;
            //context.Entry(entity.guild).State = EntityState.Unchanged;
            //context.Entry(entity.guildId).State = EntityState.Unchanged;
            context.SaveChanges();
        }

        public List<Person> GetPersonsGuild(long idguild)
        {
            return context.people.Where(x => x.guild.id == idguild).ToList();
        }

        public void Delet(Person person) { }

        public List<Person> GetObjects()
        {
            return context.people.ToList();
        }
    }
}
