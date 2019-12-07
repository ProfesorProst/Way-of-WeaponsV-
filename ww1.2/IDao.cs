using System.Collections.Generic;

namespace revcom_bot
{
    interface IDao<T>
    {
        void Add(T obj);
        void Update(T obj);
        void Delet(T obj);
        T GetObject(long id);
        List<T> GetObjects();
    }
}
