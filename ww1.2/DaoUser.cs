using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace revcom_bot
{
    class DaoUser : IDao<User>
    {
        DBConnect dBConnect = new DBConnect();

        public void Add(User user)
        {
            string query = "INSERT INTO user (iduser, username) VALUES(@id, @username)";

            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.Parameters.AddWithValue("@id", user.id);
                    cmd.Parameters.AddWithValue("@username", user.username);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            finally
            {
                dBConnect.CloseConnection();
            }
        }

        public void Update(User obj) { }

        public void Delet(User obj) { }

        public User GetObject(long id)
        {
            string query = "SELECT * FROM user where user.iduser = " + id.ToString();
            User user = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        user = new User(Convert.ToInt64(dataReader["iduser"]), null);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e) { return null; }
        }

        public List<User> GetObjects() { return null; }
    }
}
