using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using wayofweapon.Data;
using wayofweapon.Entities;

namespace wayofweapon.CRUD
{
    class CRUDGuild : CRUD<Guild>
    {
        Context context;

        public CRUDGuild()
        {
            context = new Context();
        }

        public void Create(Guild guild)
        {
            context.guilds.Add(guild);
            context.SaveChanges();
        }

        public void Update(Guild guild)
        {
            var entity = context.guilds.Find(guild.id);
            if (entity == null)
                return;

            context.Entry(entity).CurrentValues.SetValues(guild);
            context.SaveChanges();
        }

        public void Delet(Guild obj) 
        {
            context.guilds.Remove(obj);
            context.SaveChanges();
        }

        //select * where idguild = 
        public Guild Read(long id)
        {
            return context.guilds.Find(id);
        }

        public List<Guild> GetObjects()
        {
            return context.guilds.ToList();
        }
    }
}
