using System.Collections.Generic;

namespace revcom_bot
{
    class ModelIteam
    {
        readonly DaoItem _daoItem = new DaoItem();

        public void DeleteIteamFromInventory(long personId, long itemId)
        {
            _daoItem.DeleteIteamFromInventory(personId, itemId);
        }

        public void UpdatePersonInventory(long personId, Item item)
        {
            _daoItem.UpdatePersonInventory(personId, item);
        }

        public Item GetObject(long itemId)
        {
            return _daoItem.GetObject(itemId);
        }

        public List<Item> GetPersonInventory(long id)
        {
            return _daoItem.GetPersonInventory(id); 
        }

        public void AddToInventory(long id, long idIteam)
        {
            _daoItem.AddToInventory(id, idIteam);
        }

        public Item GetPersonItem(long iditem, long idperson)
        {
            return  _daoItem.GetPersonItem(iditem, idperson);
        }

        public Item GetIfHadeSameTypeEquipped(long idperson, string itemType)
        {
            return _daoItem.GetIfHadeSameTypeEquipped(idperson, itemType);
        }
    }
}
