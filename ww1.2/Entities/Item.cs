namespace wayofweapon.Entities
{
    class Item
    {
        public long id { get; set; }
        public string name { get; set; }
        public int cost { get; set; }
        public int def { get; set; }
        public int attack { get; set; }
        public string type { get; set; }

        public Item()
        {

        }
        public Item(long id, string name, int cost, int def, int attack)
        {
            this.id = id;
            this.name = name;
            this.cost = cost;
            this.def = def;
            this.attack = attack;
        }

    }
}
