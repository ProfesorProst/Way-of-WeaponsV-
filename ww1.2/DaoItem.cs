using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace revcom_bot
{
    class DaoItem : IDao<Item>
    {
        DBConnect dBConnect = new DBConnect();

        // select * item where (person, item) 
        public Item GetPersonItem(long fkitem, long fkperson)
        {
            string query = "SELECT * FROM items join inventory on items.iditems = inventory.fkitem where items.iditems = @fkitem "
                + " AND inventory.fkperson = @fkperson";
            Item item = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.Parameters.AddWithValue("@fkitem", fkitem);
                    cmd.Parameters.AddWithValue("@fkperson", fkperson);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int attack = Convert.ToInt32(dataReader["attack"]);
                        int def = Convert.ToInt32(dataReader["def"]); 
                        int cost = Convert.ToInt32(dataReader["cost"]); 
                        item = new Item(Convert.ToInt64(dataReader["iditems"]), Convert.ToString(dataReader["name"]), cost, def, attack);
                        item.type = Convert.ToString(dataReader["type"]);
                        item.count = Convert.ToInt32(dataReader["count"]);
                        item.eqiup = Convert.ToBoolean(dataReader["equip"]);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return item;
                }
                return null;
            }
            catch (Exception e) { return null; }
        }

        //select * from items
        public Item GetObject(long id)
        {
            string query = "SELECT * FROM items where items.iditems = @id";
            Item item = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int attack = Convert.ToInt32(dataReader["attack"]);
                        int def = Convert.ToInt32(dataReader["def"]); ;
                        int cost = Convert.ToInt32(dataReader["cost"]); ;
                        item = new Item(Convert.ToInt64(dataReader["iditems"]), Convert.ToString(dataReader["name"]), cost, def, attack);
                        item.type = Convert.ToString(dataReader["type"]);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return item;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e) { return null; }
        }

        //???
        public List<Item> GetObjects()
        {
            string query = "SELECT * FROM items";
            List<Item> items = null;
            Item item = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int attack = Convert.ToInt32(dataReader["attack"]);
                        int def = Convert.ToInt32(dataReader["def"]); ;
                        int cost = Convert.ToInt32(dataReader["cost"]); ;
                        item = new Item(Convert.ToInt64(dataReader["iditems"]), Convert.ToString(dataReader["name"]), cost, def, attack);
                        items.Add(item);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return items;
                }
                    return null;
            }
            catch (Exception e) { return null; }
        }

        // return item if have same type equipped
        public Item GetIfHadeSameTypeEquipped(long id, string type)
        {
            string query = "Select * from inventory join items on inventory.fkitem = items.iditems where inventory.equip = true and type = @type " +
                "and inventory.fkperson = @id";
            Item item = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int attack = Convert.ToInt32(dataReader["attack"]);
                        int def = Convert.ToInt32(dataReader["def"]); 
                        int cost = Convert.ToInt32(dataReader["cost"]); 
                        item = new Item(Convert.ToInt64(dataReader["iditems"]), Convert.ToString(dataReader["name"]), cost, def, attack);
                        item.eqiup = Convert.ToBoolean(dataReader["equip"]);
                        item.count = Convert.ToInt32(dataReader["count"]);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                }
                return item;
            }
            catch (Exception e) { return null; }
        }

        public void Add(Item obj) { }
        public void Delet(Item obj) { }
        public void Update(Item obj) { }


        //Add iteam to player inventory
        public void AddToInventory(long personID, long itemID)
        {
            string query = "INSERT INTO inventory (fkperson, fkitem) VALUES(@personID, @itemID);";
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.Parameters.AddWithValue("@personID", personID);
                    cmd.Parameters.AddWithValue("@itemID", itemID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { }
            finally
            {
                dBConnect.CloseConnection();
            }
        }

        //Update iteam in player inventory | count, equip
        public void UpdatePersonInventory(long personID, Item item)
        {
            int z = 1;
            if (item.eqiup == false) z = 0;
            string query = "UPDATE inventory SET count = '" + item.count + "', equip='" + z + "' WHERE fkperson = " + personID + " AND fkitem = " + item.id;
            try
            {
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
            catch (Exception e) { }
            finally
            {
                dBConnect.CloseConnection();
            }
        }

        //Get inventory of player
        public List<Item> GetPersonInventory(long Id)
        {
            string query = "SELECT * FROM inventory where inventory.fkperson = " + Id;
            List<Item> items = new List<Item>();
            Item item = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int count = Convert.ToInt32(dataReader["count"]);
                        bool equip = Convert.ToBoolean(dataReader["equip"]); 
                        item = new Item(Convert.ToInt64(dataReader["fkitem"]), count, equip);
                        items.Add(item);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return items;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e) { return null; }
        }

        // delete if count = 0
        public void DeleteIteamFromInventory(long idPerson, long idItem)
        {
            string query = "DELETE FROM inventory WHERE fkperson = " + idPerson + " AND fkitem = " + idItem + ";";
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) { }
            finally
            {
                dBConnect.CloseConnection();
            }
        }
    }
}
