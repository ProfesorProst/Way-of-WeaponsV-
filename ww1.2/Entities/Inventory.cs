using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wayofweapon.Entities
{
    class Inventory
    {
        [Key]
        public long id { get; private set; }

        [Index("UniqInv", 1, IsUnique = true)]
        public long personId { get; set; }
        //[ForeignKey("personId")]
        public Person person { get; set; }

        [Index("UniqInv", 2, IsUnique = true)]
        public long itemId { get; set; }
        //[ForeignKey("itemId")]
        public Item item { get; set; }
        public bool eqiup { get; set; }
        public int count { get; set; }

        public Inventory()
        {

        }

        public Inventory(Person personId, Item itemId)
        {
            this.person = personId;
            this.item = itemId;
            this.eqiup = false;
            this.count = 1;
        }
    }
}
