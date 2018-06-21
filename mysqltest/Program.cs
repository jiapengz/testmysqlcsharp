using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;

namespace mysqltest
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnect test = new DBConnect();
            Console.ReadKey();
        }
    }
}

class DBConnect
{
    private MySqlConnection connection;
    private string server;
    private string port;
    private string database;
    private string user;
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
        port = "3306";
        database = "sqltest";
        user = "root";
        password = "root";
        string connectionString;
        connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "port=" + port + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);

        try
        {
            Console.WriteLine("Openning Connection ...");

            connection.Open();

            Console.WriteLine("Connection successful!");

            if (connection.State == ConnectionState.Open)
            {
                // TestInsert();

                TestUpdate();

                Console.WriteLine("Prepared to query.");
                TestQuery();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    private void TestQuery()
    {
        string query = "SELECT * FROM " + "tabletest";

        //Create Command
        MySqlCommand cmd = new MySqlCommand(query, connection);

        //Create a data reader and Execute the command
        MySqlDataReader dataReader = cmd.ExecuteReader();

        while (dataReader.Read())
        {
            Console.WriteLine("{0}, {1}, {2}", dataReader[0], dataReader[1], dataReader[2]);
            Console.WriteLine("{0}, {1}, {2}", dataReader[0].GetType(), dataReader[1].GetType(), dataReader[2].GetType());
        }

        //close Data Reader
        dataReader.Close();
    }

    //Insert statement
    public void TestInsert()
    {
        string query = "INSERT INTO tabletest (idx, country, level) VALUES (7, 'Austria', 7)";

        //create command and assign the query and connection from the constructor
        MySqlCommand cmd = new MySqlCommand(query, connection);

        //Execute command
        cmd.ExecuteNonQuery();
    }

    //Update statement
    public void TestUpdate()
    {
        string query = "UPDATE tabletest SET level=7, country='Austria' WHERE idx=7";

        //create mysql command
        MySqlCommand cmd = new MySqlCommand();
        //Assign the query using CommandText
        cmd.CommandText = query;
        //Assign the connection using Connection
        cmd.Connection = connection;

        //Execute query
        cmd.ExecuteNonQuery();
    }



    private bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    ~DBConnect()
    {
        CloseConnection();
    }
}