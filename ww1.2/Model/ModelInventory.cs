using System.Collections.Generic;
using System.Linq;
using wayofweapon.CRUD;
using wayofweapon.Entities;

namespace wayofweapon.Model
{
    class ModelInventory
    {
        CRUDItem crudIteam;
        CRUDPerson crudPerson;
        CRUDInventory crudInventory;

        public ModelInventory()
        {
            crudIteam = new CRUDItem();
            crudPerson = new CRUDPerson();
            crudInventory = new CRUDInventory();
        }

        public void DeleteIteamFromInventory(long personId, long itemId)
        {
            Person person = crudPerson.Read(personId);
            Item item = crudIteam.Read(itemId);
            Inventory inv = crudInventory.Read(personId, itemId);
            inv = GetObject(inv.id);
            if (inv.count <= 1)
                crudInventory.Delet(inv);
            else
            {
                inv.count -= 1;
                crudInventory.Update(inv);
            }
        }

        public void UpdatePersonInventory(Inventory inventory)
        {
            crudInventory.Update(inventory);
        }

        public Inventory GetObject(long itemId)
        {
            Inventory inventory = crudInventory.Read(itemId);
            inventory.item = crudIteam.Read(inventory.itemId);
            inventory.person = crudPerson.Read(inventory.personId);
            return inventory;
        }

        public List<Inventory> GetPersonInventory(long id)
        {
            List<Inventory> inv = crudInventory.GetInventories(id);
            List<Inventory> invNew = new List<Inventory>();
            foreach (Inventory inventory in inv)
                invNew.Add(GetObject(inventory.id));
            return invNew;
        }

        public void AddToInventory(Person person, Item item)
        {
            Inventory inv = crudInventory.Read(person.id, item.id);
            if (inv != null)
            {
                inv = GetObject(inv.id);
                inv.count += 1;
                crudInventory.Update(inv);
            }
            else
            {
                inv = new Inventory(person, item);
                crudInventory.Create(inv);
            }
        }

        public Inventory GetPersonItem(long iditem, long idperson)
        {
            Inventory inv = crudInventory.Read(idperson, iditem);
            return inv == null ? null : GetObject(inv.id);
        }

        public Inventory GetIfHadeSameTypeEquipped(long idperson, string itemType)
        {

            List<Inventory> invNew = GetPersonInventory(idperson);
            return invNew.Where(x => x.item.type == itemType && x.eqiup == true).FirstOrDefault();
        }
    }
}
