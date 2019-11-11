using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add .NET libraries
using System.Data;
using System.Data.SqlClient;

// add car model
using AutoLotClientApp.Models;

namespace AutoLotClientApp.DataOperations
{
    public class InventoryDAL
    {
        // naming convention is to use underscore as prefix for
        //      variables that are private to the class
        // readonly indicates can only occur as part of the declaration 
        //      or in constructor in same class.
        private readonly string _connectionString;

        public InventoryDAL() : this(@"Data Source = (localdb)\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot")
        {
        }
        public InventoryDAL(string connectionString) =>
            _connectionString = connectionString;

        private SqlConnection _sqlConnection = null;
        private void OpenConnection()
        {
            _sqlConnection = new SqlConnection { ConnectionString = _connectionString };
            _sqlConnection.Open();
        }
        private void CloseConnection()
        {
            if (_sqlConnection?.State != ConnectionState.Closed)
            {
                _sqlConnection.Close();
            }
        }

        public List<Car> GetAllInventory()
        {
            OpenConnection();
            // this holds the records
            List<Car> inventory = new List<Car>();

            // prep command object
            string sql = "SELECT * FROM Inventory";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    inventory.Add(new Car
                    {
                        CarId = (int)dataReader["CarId"],
                        Color = (string)dataReader["Color"],
                        Make = (string)dataReader["Make"],
                        PetName = (string)dataReader["PetName"]
                    });
                }
                dataReader.Close();
            }
            return inventory;
        }

        // selection method gets a single car based on the CarId
        public Car GetCar(int id)
        {
            OpenConnection();
            Car car = null;
            string sql = $"SELECT * FROM Inventory WHERE CarId = {id}";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    car = new Car
                    {
                        CarId = (int)dataReader["CarId"],
                        Color = (string)dataReader["Color"],
                        Make = (string)dataReader["Make"],
                        PetName = (string)dataReader["PetName"]
                    };
                }
                dataReader.Close();
            }
            return car;
        }
        // insert logic
        // takes parameters that map to table columns
        public void InsertAuto(string color, string make, string petName)
        {
            OpenConnection();
            // format and execute sql statement
            string sql = $"INSERT INTO Inventory (Make, Color, PetName) VALUES ('{make}','{color}','{petName}')";
            // execute using connection
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // insert logic
        // takes Car model as a parameter
        public void InsertAuto(Car car)
        {
            OpenConnection();
            // format and execute sql statement
            string sql = $"INSERT INTO Inventory (Make, Color, PetName) VALUES ('{car.Make}','{car.Color}','{car.PetName}')";
            // execute using connection
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        //deletion logic
        public void DeleteCar(int id)
        {
            OpenConnection();
            // get id of car to delete, then do so
            string sql = $"DELETE FROM Inventory WHERE CarId = '{id}'";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Sorry! That car is on order!", ex);
                    throw error;
                }
            }
            CloseConnection();
        }

        // update logic
        public void UpdateCarPetName(int id, string newPetName)
        {
            OpenConnection();
            // get id of car to modify pet name of
            string sql = $"UPDATE Inventory SET PetName = '{newPetName}' WHERE CarId = '{id}'";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

    }
}
