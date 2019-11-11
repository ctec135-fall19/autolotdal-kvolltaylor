/*
 Katrina Voll-Taylor
 CTEC #135
 Module 7, DAL Example
 11 November 2019
 
 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoLotClientApp.DataOperations;
using AutoLotClientApp.Models;

namespace AutoLotClientApp
{
    class Program
    {
        static void Main(string[] args)
        {

            InventoryDAL dal = new InventoryDAL();
            var list = dal.GetAllInventory();

            // list all cars in the inventory
            Console.WriteLine((new string('*', 15)) + " All Cars " + (new string('*', 15)));
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            foreach (var itm in list)
            {
                Console.WriteLine($"{itm.CarId}\t{itm.Make}\t{itm.Color}\t{itm.PetName}");
            }
            Console.WriteLine();

            // list the first car in order of color
            var car = dal.GetCar(list.OrderBy(x => x.Color).Select(x => x.CarId).First());
            Console.WriteLine((new string('*', 15)) + " First Car By Color " + (new string('*', 15)));
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            Console.WriteLine($"{car.CarId}\t{car.Make}\t{car.Color}\t{car.PetName}");
            Console.WriteLine();

            // try/catch logic to delete a car in inventory
            try
            {
                dal.DeleteCar(5);
                Console.WriteLine("Car deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
            }
            Console.WriteLine();

            // insert a new car object into the inventory
            dal.InsertAuto(new Car { Color = "Blue", Make = "Pilot", PetName = "TowMonster" });
            list = dal.GetAllInventory();
            var newCar = list.First(x => x.PetName == "TowMonster");
            Console.WriteLine((new string('*', 15)) + " New Car " + (new string('*', 15)));
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            Console.WriteLine($"{newCar.CarId}\t{newCar.Make}\t{newCar.Color}\t{newCar.PetName}");
            dal.DeleteCar(newCar.CarId);
            Console.WriteLine();

            // list the inventory again
            Console.WriteLine((new string('*', 15)) + " All Cars After an Insertion & a Deletion " + (new string('*', 15)));
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            foreach (var itm in list)
            {
                Console.WriteLine($"{itm.CarId}\t{itm.Make}\t{itm.Color}\t{itm.PetName}");
            }
            Console.WriteLine();

            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();

        }
    }
}
