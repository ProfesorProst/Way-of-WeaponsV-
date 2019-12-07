using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace revcom_bot
{
    class DaoPerson : IDao<Person>
    {
        DBConnect dBConnect = new DBConnect();

        //Insert idperson, personnick, race, energy, maxenergy, def, atack
        public void Add(Person person)
        {
            string query = "INSERT INTO person (idperson, personnick, race, energy, maxenergy, def, atack) VALUES(" + person.id + ", '"
                + person.personNick +"','"+ person.race +"',"+ person.energy +","+ person.maxenergy +","+ person.def+","+person.atack+");";
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) {  }
            finally {
                dBConnect.CloseConnection();
            }
        }

        //Update energy, energytime
        public void UpdateTime(Person person)
        {
            string theTime = person.energytime.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "UPDATE person SET energy = '" + person.energy + "', energytime='" + theTime
                + "' WHERE idperson = " + person.id;
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
            
        } 

        //Update gold, exp 
        public void UpdateWork(Person person)
        {
            UpdateTime(person);
            string query = "UPDATE person SET gold = " + person.gold + ", exp='" + person.exp 
                + "' WHERE idperson = " + person.id;
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

        //Update gold, guild 
        public void UpdateGuild(Person person)
        {
            UpdateTime(person);
            string query = "UPDATE person SET gold = " + person.gold + ", fkguild=";
            query += (person.guild == null) ? "NULL" : person.guild+"";
            query += " WHERE idperson = " + person.id;
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

        //UPDATE lvl def atack
        public void UpdateLvl(Person person)
        {
            UpdateTime(person);
            string query = "UPDATE person SET lvl = '" + person.lvl + "', atack = " + person.atack +", def = " 
                + person.def + " WHERE idperson = " + person.id;
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

        //Select idperson, energy, lvl, exp, gold, maxenergy, energytime, race
        public Person GetEnergy(long id)
        {
            string query = "SELECT idperson, energy, lvl, exp, gold, maxenergy, energytime, race FROM person where person.idperson = " + id.ToString();
            Person person = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        person = new Person(Convert.ToInt64(dataReader["idperson"]), null);
                        person.energy = Convert.ToInt32(dataReader["energy"]);
                        person.lvl = Convert.ToInt32(dataReader["lvl"]);
                        person.exp = Convert.ToInt32(dataReader["exp"]);
                        person.gold = Convert.ToInt32(dataReader["gold"]);
                        person.maxenergy = Convert.ToInt32(dataReader["maxenergy"]);
                        person.energytime = DateTime.Parse(dataReader["energytime"].ToString(),
                            System.Globalization.CultureInfo.InvariantCulture);
                        person.race = Convert.ToString(dataReader["race"]);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return person;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e) { return null; }
        }

        //SELECT idperson, energy, energytime, maxenergy
        public Person GetEnergyTime(long id)
        {
            string query = "SELECT idperson, energy, energytime, maxenergy FROM person where person.idperson = " + id.ToString();
            Person person = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        person = new Person(Convert.ToInt64(dataReader["idperson"]), null);
                        person.energy = Convert.ToInt32(dataReader["energy"]);
                        person.energytime = DateTime.Parse(dataReader["energytime"].ToString(),
                            System.Globalization.CultureInfo.InvariantCulture);
                        person.maxenergy = Convert.ToInt32(dataReader["maxenergy"]);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return person;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e) { return null; }
        }

        //SELECT *
        public Person GetObject(long id)
        {
            string query = "SELECT * FROM person where person.idperson = " + id.ToString();
            Person person = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        person = new Person(Convert.ToInt64(dataReader["idperson"]), null);
                        person.hp = Convert.ToInt32(dataReader["hp"]);
                        person.energy = Convert.ToInt32(dataReader["energy"]);
                        person.energytime = DateTime.Parse(dataReader["energytime"].ToString());
                        person.maxenergy = Convert.ToInt32(dataReader["maxenergy"]);
                        person.gold = Convert.ToInt32(dataReader["gold"]);
                        person.lvl = Convert.ToInt32(dataReader["lvl"]);
                        person.exp = Convert.ToInt32(dataReader["exp"]);
                        person.def = Convert.ToInt32(dataReader["def"]);
                        person.atack = Convert.ToInt32(dataReader["atack"]);
                        person.personNick = Convert.ToString(dataReader["personnick"]);
                        person.race = Convert.ToString(dataReader["race"]);
                        var outputParamguild = dataReader["fkguild"];
                        if (!(outputParamguild is DBNull))
                            person.guild = Convert.ToInt64(outputParamguild);
                        var outputParamfraction = dataReader["fraction"];
                        if (!(outputParamfraction is DBNull))
                            person.fraction = Convert.ToInt32(outputParamfraction);
                        var outputParamWar = dataReader["stateOfWar"];
                        if (!(outputParamWar is DBNull))
                            person.attackOrDef = Convert.ToBoolean(outputParamguild);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return person;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e) { return null; }
        }

        //SELECT *
        public Person GetObjectByPersonNick(string personNick)
        {
            string query = "SELECT * FROM person where person.personnick = @personNick";
            Person person = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    cmd.Parameters.AddWithValue("@personNick", personNick);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        person = new Person(Convert.ToInt64(dataReader["idperson"]), null);
                        person.hp = Convert.ToInt32(dataReader["hp"]);
                        person.energy = Convert.ToInt32(dataReader["energy"]);
                        person.energytime = DateTime.Parse(dataReader["energytime"].ToString());
                        person.maxenergy = Convert.ToInt32(dataReader["maxenergy"]);
                        person.gold = Convert.ToInt32(dataReader["gold"]);
                        person.lvl = Convert.ToInt32(dataReader["lvl"]);
                        person.exp = Convert.ToInt32(dataReader["exp"]);
                        person.def = Convert.ToInt32(dataReader["def"]);
                        person.atack = Convert.ToInt32(dataReader["atack"]);
                        person.personNick = Convert.ToString(dataReader["personnick"]);
                        person.race = Convert.ToString(dataReader["race"]);
                        var outputParamguild = dataReader["fkguild"];
                        if (!(outputParamguild is DBNull))
                            person.guild = Convert.ToInt64(outputParamguild);
                        var outputParamfraction = dataReader["fraction"];
                        if (!(outputParamfraction is DBNull))
                            person.fraction = Convert.ToInt32(outputParamfraction);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return person;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e) { return null; }
        }

        //SELECT guild
        public Person GetGuild(long id)
        {
            string query = "SELECT fkguild FROM person where person.idperson = " + id.ToString();
            Person person = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        person = new Person(id, null);
                        person.guild = Convert.ToInt64(dataReader["fkguild"]);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return person;
                }
                    return null;
            }
            catch (Exception e) { return null; }
        }

        // Select * where guild =
        public List<Person> GetPersonsGuild(long idguild)
        {
            string query = "SELECT * FROM person where fkguild = " + idguild;
            List<Person> people = new List<Person>();
            Person person = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        person = new Person(Convert.ToInt64(dataReader["idperson"]), null);
                        person.hp = Convert.ToInt32(dataReader["hp"]);
                        person.energy = Convert.ToInt32(dataReader["energy"]);
                        person.energytime = DateTime.Parse(dataReader["energytime"].ToString());
                        person.maxenergy = Convert.ToInt32(dataReader["maxenergy"]);
                        person.gold = Convert.ToInt32(dataReader["gold"]);
                        person.lvl = Convert.ToInt32(dataReader["lvl"]);
                        person.exp = Convert.ToInt32(dataReader["exp"]);
                        person.def = Convert.ToInt32(dataReader["def"]);
                        person.atack = Convert.ToInt32(dataReader["atack"]);
                        person.personNick = Convert.ToString(dataReader["personnick"]);
                        person.race = Convert.ToString(dataReader["race"]);
                        person.guild = Convert.ToInt64(dataReader["fkguild"]);
                        people.Add(person);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return people;
                }
                return null;
            }
            catch (Exception e) { return null; }
        }

        //update *
        public void Update(Person person)
        {
            string query = "UPDATE person SET hp = " + person.hp + ", energy = " + person.energy + ", maxenergy = " + person.maxenergy + ", def = " + person.def + ", atack = " + person.atack
                + ", gold = " + person.gold + ", lvl = " + person.lvl + ", exp = " + person.exp;
            if (person.guild != null)
                query += ", fkguild = " + person.guild;
            if (person.fraction != null) query += ", fraction =" + person.fraction;
            if (person.attackOrDef != null) query += ", stateOfWar =" + person.attackOrDef;
            query += " WHERE idperson = " + person.id;
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

        public void UpdateStateOfWarNULL(long id)
        {
            string query = "UPDATE person SET stateOfWar = NULL  WHERE idperson = " + id;
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

        public void Delet(Person person) { }

        public List<Person> GetObjects()
        {
            string query = "SELECT * FROM person";
            List<Person> people = new List<Person>();
            Person person = null;
            try
            {
                dBConnect.CloseConnection();
                if (dBConnect.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, dBConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        person = new Person(Convert.ToInt64(dataReader["idperson"]), null);
                        person.hp = Convert.ToInt32(dataReader["hp"]);
                        person.energy = Convert.ToInt32(dataReader["energy"]);
                        person.energytime = DateTime.Parse(dataReader["energytime"].ToString());
                        person.maxenergy = Convert.ToInt32(dataReader["maxenergy"]);
                        person.gold = Convert.ToInt32(dataReader["gold"]);
                        person.lvl = Convert.ToInt32(dataReader["lvl"]);
                        person.exp = Convert.ToInt32(dataReader["exp"]);
                        person.def = Convert.ToInt32(dataReader["def"]);
                        person.atack = Convert.ToInt32(dataReader["atack"]);
                        person.personNick = Convert.ToString(dataReader["personnick"]);
                        person.race = Convert.ToString(dataReader["race"]);
                        var outputParamguild = dataReader["fkguild"];
                        if (!(outputParamguild is DBNull))
                            person.guild = Convert.ToInt64(outputParamguild);
                        var outputParamfraction = dataReader["fraction"];
                        if (!(outputParamfraction is DBNull))
                            person.fraction = Convert.ToInt32(outputParamfraction);
                        var outputParamWar = dataReader["stateOfWar"];
                        if (!(outputParamWar is DBNull))
                            person.attackOrDef = Convert.ToBoolean(outputParamguild);
                        people.Add(person);
                    }
                    dataReader.Close();
                    dBConnect.CloseConnection();
                    return people;
                }
                return null;
            }
            catch (Exception e) { return null; }
        }
    }
}
