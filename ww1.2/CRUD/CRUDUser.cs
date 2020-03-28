using System.Collections.Generic;
using wayofweapon.Data;
using wayofweapon.Entities;

namespace wayofweapon.CRUD
{
    class CRUDUser : CRUD<User>
    {
        Context context;

        public CRUDUser()
        {
            context = new Context();
        }

        public void Create(User user)
        {
            context = new Context();
            context.users.Add(user);
            context.SaveChanges();
        }

        public void Update(User obj) { }

        public void Delet(User obj) { }

        public User Read(long id)
        {
            return context.users.Find(id);
        }

        public List<User> GetObjects() { return null; }
    }
}
