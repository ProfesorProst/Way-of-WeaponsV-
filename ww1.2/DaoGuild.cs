using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace revcom_bot
{
    class DaoGuild : IDao<Guild>
    {
        DBConnect dBConnect = new DBConnect();

        // Inser master, name
        public void Add(Guild guild)
        {
            string query = "INSERT INTO guild ( master, name, fraction) VALUES(@param_val_1, @param_val_2, " + guild.fraction + ");";
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.Parameters.AddWithValue("@param_val_1", guild.master);
                    cmd.Parameters.AddWithValue("@param_val_2", guild.name);
                    /*
                    cmd.Parameters.Add("@master", SqlDbType.VarChar);
                    cmd.Parameters["@master"].Value = guild.master;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar);
                    cmd.Parameters["@name"].Value = guild.name;
                    */

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { }
            finally
            {
                dBConnect.CloseConnection();
            }
        }

        // update gold where ifguild
        public void UpdateGold(Guild guild)
        {
            string query = "UPDATE guild SET gold = " + guild.gold + " WHERE guild.idguild = " + guild.id + ";";
            dBConnect.CloseConnection();
            if (dBConnect.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = dBConnect.connection;
                cmd.ExecuteNonQuery();
                dBConnect.CloseConnection();
            }
        }

        public void Update(Guild guild)
        {
            string query = "UPDATE guild SET master = @master, name = @name, gold = " + guild.gold + ", hire = " + guild.hire + ", maxplayers = "
                + guild.maxplayers + ", fraction = " + guild.fraction; 
            if (guild.chatUrl != null || guild.chatUrl != "") query += ", chat = @chatUrl";
            query += " WHERE idguild = " + guild.id;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.Parameters.AddWithValue("@master", guild.master);
                    cmd.Parameters.AddWithValue("@name", guild.name);
                    if (guild.chatUrl != null || guild.chatUrl != "") cmd.Parameters.AddWithValue("@chatUrl", guild.chatUrl);
                    cmd.ExecuteNonQuery();
                    dBConnect.CloseConnection();
                }
            }
            catch (Exception e) { throw new Exception(e.ToString()); }
        }

        public void Delet(Guild obj) { }

        //select * where idguild = 
        public Guild GetObject(long id)
        {
            string query = "SELECT * FROM guild where idguild = " + id.ToString();
            Guild guild = new Guild();
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        string name = Convert.ToString(dataReader["name"]);
                        string master = Convert.ToString(dataReader["master"]);
                        int gold = Convert.ToInt32(dataReader["gold"]);
                        guild = new Guild( id, name, master, gold);
                        guild.hire = Convert.ToBoolean(dataReader["hire"]);
                        guild.maxplayers = Convert.ToInt32(dataReader["maxplayers"]);
                        guild.fraction = Convert.ToInt32(dataReader["fraction"]);
                        guild.chatUrl = Convert.ToString(dataReader["chat"]);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return guild;
                }
                    return null;
            }
            catch (Exception e) { return null; }
        }

        //select * where name = 
        public Guild GetGuildByName(string name)
        {
            string query = "SELECT * FROM guild where guild.name = @name";
            Guild guild = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.Parameters.AddWithValue("@name", name);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        string master = Convert.ToString(dataReader["master"]);
                        int gold = Convert.ToInt32(dataReader["gold"]);
                        long id = Convert.ToInt64(dataReader["idguild"]);
                        guild = new Guild(id, name, master, gold);
                        guild.hire = Convert.ToBoolean(dataReader["hire"]);
                        guild.maxplayers = Convert.ToInt32(dataReader["maxplayers"]);
                        guild.fraction = Convert.ToInt32(dataReader["fraction"]);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return guild;
                }
                return null;
            }
            catch (Exception e) { return null; }
        }

        //select where hire == true
        public List<Guild> GetOpenGuild()
        {
            string query = "SELECT * FROM guild where guild.hire = 1";
            List<Guild> guilds = null;
            Guild guild = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        string name = Convert.ToString(dataReader["name"]);
                        string master = Convert.ToString(dataReader["master"]);
                        int gold = Convert.ToInt32(dataReader["gold"]);
                        guild = new Guild(Convert.ToInt64(dataReader["idguild"]), name, master, gold);
                        guild.hire = Convert.ToBoolean(dataReader["hire"]);
                        guild.maxplayers = Convert.ToInt32(dataReader["maxplayer"]);
                        guild.fraction = Convert.ToInt32(dataReader["fraction"]);
                        guilds.Add(guild);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return guilds;
                }
                return null;
            }
            catch (Exception e) { return null; }
        } 

        public List<Guild> GetObjects()
        {
            string query = "SELECT * FROM guild";
            List<Guild> guilds = new List<Guild>();
            Guild guild = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        long id = Convert.ToInt64(dataReader["idguild"]);
                        string name = Convert.ToString(dataReader["name"]);
                        string master = Convert.ToString(dataReader["master"]);
                        int gold = Convert.ToInt32(dataReader["gold"]);
                        guild = new Guild(id, name, master, gold);
                        guild.hire = Convert.ToBoolean(dataReader["hire"]);
                        guild.maxplayers = Convert.ToInt32(dataReader["maxplayers"]);
                        guild.fraction = Convert.ToInt32(dataReader["fraction"]);
                        guilds.Add(guild);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return guilds;
                }
                return null;
            }
            catch (Exception e) { return null; }
        }
    }
}
