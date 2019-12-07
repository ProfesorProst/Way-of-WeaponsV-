namespace revcom_bot
{
    class User
    {
        public long id { get; set; }
        public string username { get; set; }
        public string nickname { get; set; }

        public User(long id, string username)
        {
            this.id = id;
            this.username = username;
        }
    }
}