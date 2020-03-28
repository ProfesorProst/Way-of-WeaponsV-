using System.Collections.Generic;
using wayofweapon.Data;

namespace wayofweapon.CRUD
{
    interface CRUD<T>
    {
        void Create(T obj);
        T Read(long id);
        void Update(T item);
        void Delet(T obj);
    }
}
