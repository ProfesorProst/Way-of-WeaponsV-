using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wayofweapon.CRUD;
using wayofweapon.Entities;

namespace wayofweapon.Model
{
    class ModelItem
    {
        CRUDItem cRUDItem;

        public ModelItem()
        {
            cRUDItem = new CRUDItem();
        }

        internal Item GetObject(long v)
        {
            return cRUDItem.Read(v);
        }
    }
}
