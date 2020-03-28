using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using wayofweapon.Data;
using wayofweapon.Entities;

namespace wayofweapon.CRUD
{
    class CRUDItem : CRUD<Item>
    {
        Context context;

        public CRUDItem()
        {
            context = new Context();
        }

        public void Create(Item item) { }
        
        public Item Read(long id)
        {
            return context.items.Find(id);
        }

        //???
        public List<Item> GetObjects()
        {
            return context.items.ToList();
        }
        public void Delet(Item obj) { }
        public void Update(Item obj) { }

    }
}
