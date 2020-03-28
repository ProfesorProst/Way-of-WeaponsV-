using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wayofweapon.Entities
{
    class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long id { get; set; }
        [MaxLength(128)]
        [Index("NameANDFileName", 1, IsUnique = true)]
        public string username { get; set; }
        [MaxLength(128)]
        public string nickname { get; set; }

        public User()
        {

        }
        public User(long id, string username)
        {
            this.id = id;
            this.username = username;
        }
    }
}