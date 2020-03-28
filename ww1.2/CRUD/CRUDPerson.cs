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

        public void Update(Person person)
        {
            context = new Context();
            var entity = context.people.Find(person.id);
            if (entity == null)
                return;

            if (person.guild != null)
            {
                person.guild = context.guilds.SingleOrDefault(x => x.id == person.guild.id);
                person.guildId = person.guild.id;
            }
            else if (person.guildId != null)
            {
                long z = person.guildId.Value;
                person.guild = context.guilds.SingleOrDefault(x => x.id == z);
                person.guildId = entity.guild.id;
            }

            context.Entry(entity).CurrentValues.SetValues(person);
            context.SaveChanges();
            return;
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
