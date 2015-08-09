using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace TamuCourseCheck
{
    class Program
    {
        

        static void Main(string[] args)
        {
            MySqlConnection connection;
      
            var department = "";
            var courseNum = "";
            var crn = "";

            IsOpen driver = new IsOpen();
            driver.Setup();
            driver.GetToLogIn();
            driver.LogIn();
            
            using (MySqlConnection conn = new MySqlConnection("Database=TamuCourseCheck;Data Source=us-cdbr-azure-southcentral-e.cloudapp.net;User Id=b80168a661b9d1;Password=e27079dc"))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT * FROM queue;";
                    
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.HasRows) return;
                    Console.WriteLine("I got to this point.");
                    while (reader.Read())
                    {
                        department = reader.GetString("department");
                        courseNum = reader.GetString("course");
                        crn = reader.GetString("crn");
                        Console.WriteLine("Read the entry: ");
                        Console.Write(department);
                        Console.Write(courseNum);
                        Console.WriteLine(crn);
                        driver.GetToLookUp(department, courseNum, crn);
                    }
                    reader.Close();
                    conn.Close();
                }
            }



        }
        
    }
}
