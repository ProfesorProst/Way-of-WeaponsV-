using System;
using MySql.Data.MySqlClient;

namespace revcom_bot
{
    class DBConnect
    {
        public MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "ww";
            uid = "root";
            password = "112233eE";
            string connectionString;
            if (IsLinux)
                connectionString = "Server = " + server + "; User Id = " + uid + 
                    "; Password = " + password + "; Database = " + database +  "; SslMode = none";
            else
                connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                    database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator"+ ex.ToString());
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again" + ex.ToString());
                        break;
                    default:
                        Console.WriteLine("Invalid string to connect" + ex.ToString());
                        break;
                }
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        private static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
    }
}
