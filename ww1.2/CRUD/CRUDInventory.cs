using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using wayofweapon.CRUD;
using wayofweapon.Data;
using wayofweapon.Entities;

namespace wayofweapon.CRUD
{
    class CRUDInventory : CRUD<Inventory>
    {

        Context context;

        public CRUDInventory()
        {
            context = new Context();
        }
        public void Create(Inventory obj)
        {
            context = new Context();
            context.inventories.Add(obj);
            context.Entry(obj.person).State = EntityState.Unchanged;
            context.Entry(obj.item).State = EntityState.Unchanged;
            context.SaveChanges();
        }

        public void Delet(Inventory obj)
        {
            context = new Context();
            obj = context.inventories.Find(obj.id);
            if (obj == null)
                return;
            //context.Entry(obj.person).State = EntityState.Unchanged;
            //context.Entry(obj.item).State = EntityState.Unchanged;
            context.inventories.Remove(obj);
            context.SaveChanges();
        }

        public Inventory Read(long id)
        {
            return context.inventories.Find(id);
        }

        public Inventory Read(long personId, long itemId)
        {
            return context.inventories.Where(x => x.person.id == personId && x.item.id == itemId).SingleOrDefault();
        }

        public void Update(Inventory item)
        {
            context = new Context();
            var entity = context.inventories.Find(item.id);
            if (entity == null)
                return;

            context.Entry(item.person).State = EntityState.Unchanged;
            context.Entry(item.item).State = EntityState.Unchanged;
            context.Entry(entity).CurrentValues.SetValues(item);
            context.SaveChanges();
        }

        public List<Inventory> GetInventories(long id)
        {
            return context.inventories.Where(x=> x.person.id == id).ToList();
        }
    }
}
