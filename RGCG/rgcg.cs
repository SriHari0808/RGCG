using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace RGCG
{
    class rgcg
    {
        // Connection String connects the source code with the rgcg database
        string ConnectionString = @"server=localhost;user id=root;password=Omadhish@1;database=rgcg";

        public void menu()                           //Menu System for the RGCG 
        {
            bool runstate = true;
            while (runstate != false)
            {
                Console.Clear();
                Console.WriteLine("\t\t$$ Welcome to Random Gift Card Generator $$");
                Console.WriteLine("1.Create Random Gift Card List.");
                Console.WriteLine("2.Display Random Gift Card List");
                Console.WriteLine("3.Average Gift Card Amount");
                Console.WriteLine("4.Total Gift Card Amount");
                Console.WriteLine("5.High and Low Gift Card Information");
                Console.WriteLine("6.Exit Application");
                Console.WriteLine("Enter the option for above number:");
                int option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        {
                            Create_Random_Gift_Card_List();
                            Console.WriteLine("A new list of 20 Gift Cards is created successfully");
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                    case 2:
                        {
                            Display_Random_Gift_Card_List();
                            Console.WriteLine("\n\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                    case 3:
                        {
                            Average_Gift_Card_Amount();
                            Console.WriteLine("\n\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                    case 4:
                        {
                            Total_Gift_Card_Amount();
                            Console.WriteLine("\n\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                    case 5:
                        {
                            High_and_Low_Gift_Card_Information();
                            Console.WriteLine("\n\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                    case 6:
                        {
                            Console.WriteLine("Thank you, Visit again!!");
                            runstate = false;
                            break;
                        }

                    default:
                        Console.WriteLine("Enter the above options only (1 - 6): ");

                        break;
                }
            }
        }

        // Create 20 random record by obtaining first name & card name from the database.
        // Create a random price amount from 10 - 100
        // Create a table and insert these values into the table.
        private void Create_Random_Gift_Card_List()   
        {
            // A MySQLConnection object is created. This object is used to open a connection to a database.
            using var con = new MySqlConnection(ConnectionString);
            con.Open(); //This line opens the database connection.

            string[] FirstName = Read_Data("SELECT * FROM Name"); //Reading the first Name from Database
            string[] GiftCard = Read_Data("SELECT * FROM GiftCard"); //Reading the gift card names from Database

            Random random_FirstName = new Random();   //Selecting random first name
            int[] fname_index = new int[20];

            Random random_GiftCard = new Random();    //Selecting random gift card name
            int[] GiftCard_index = new int[20];

            Random random_amount = new Random();      // Creating a random price amount from 10 - 100
            int[] Amount = new int[20];

            for (int i = 0; i < 20; i++)              // Running for loop for 20 times to create 20 records
            {
                fname_index[i] = random_FirstName.Next(FirstName.Length);
                GiftCard_index[i] = random_GiftCard.Next(GiftCard.Length);
                Amount[i] = random_amount.Next(10, 100);
            }

            using var cmd = new MySqlCommand();
            cmd.Connection = con;

            cmd.CommandText = "DROP TABLE IF EXISTS RANDOM"; // Drop the table if its already exist
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE RANDOM           
            (NAME VARCHAR(25), GIFTCARD VARCHAR(25), AMOUNT INT)"; // Creating a table with 3 columns: name, gift card, amount
            cmd.ExecuteNonQuery();

            for (int i = 0; i < 20; i++)            // Inserting the 20 records into the database table
            {
                cmd.Parameters.AddWithValue("@FirstName", FirstName[fname_index[i]]);
                cmd.Parameters.AddWithValue("@GiftCard", GiftCard[GiftCard_index[i]]);
                cmd.Parameters.AddWithValue("@amount", Amount[i]);
                cmd.CommandText = "INSERT INTO RANDOM(NAME, GIFTCARD, AMOUNT) VALUES(@FirstName, @GiftCard, @amount)";
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
        }


        // Read first name and giftcard names from the database
        private string[] Read_Data(string command)
        {
            using var con = new MySqlConnection(ConnectionString);
            con.Open();

            using var cmd = new MySqlCommand(command, con);

            //To create a MySQLDataReader, we call the ExecuteReader method of the MySqlCommand object.
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                string[] str = new string[10];
                int i = 0;
                while (rdr.Read())  //The Read method advances the data reader to the next record
                {
                    str[i] = rdr.GetString(0); //Storing the data into a string
                    i++;
                }
                return str;
            }
        }

        // Display the created 20 recods of gifft card list
        private void Display_Random_Gift_Card_List()
        {
            using var con = new MySqlConnection(ConnectionString);
            con.Open();

            using var cmd = new MySqlCommand("SELECT * FROM RANDOM", con); // Reading from the random table
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("NAME\t AMOUNT\t GIFTCARD\n");
                while (rdr.Read()) // Printing the list to output
                {
                    Console.WriteLine("{0}\t {1}$\t {2}", rdr.GetString(0), rdr.GetInt32(2), rdr.GetString(1));
                }
            }
        }


        // Calculating the average of the gift card amount
        private int[] Average_Gift_Card_Amount()
        {
            using var con = new MySqlConnection(ConnectionString);
            con.Open();

            using var cmd = new MySqlCommand("SELECT * FROM RANDOM", con);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                int[] str = new int[20];
                int i = 0;
                while (rdr.Read())
                {
                    str[i] = rdr.GetInt32(2);
                    i++;
                }
                double average = Queryable.Average(str.AsQueryable());
                Console.WriteLine("Average Gift Card Amount = {0}$ ",average);
                return str;
            }
        }

        //Calculating the total gift card amount
        private int[] Total_Gift_Card_Amount()
        {
            using var con = new MySqlConnection(ConnectionString);
            con.Open();

            using var cmd = new MySqlCommand("SELECT * FROM RANDOM", con);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                int[] str = new int[20];
                int i = 0;
                while (rdr.Read())
                {
                    str[i] = rdr.GetInt32(2);
                    i++;
                }
                int sum = str.Sum();

                Console.WriteLine("Total Gift Card Amount = {0}$ ", sum);
                return str;
            }
        }

        // Calculating the high and low gift card information
        private void High_and_Low_Gift_Card_Information()
        {
            using var con = new MySqlConnection(ConnectionString);
            con.Open();

            using var cmd = new MySqlCommand("SELECT * FROM RANDOM", con);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                string[] FirstName = new string[20];
                string[] GiftCard = new string[20];
                int[] Amount = new int[20];
                int i = 0;
                while (rdr.Read())
                {
                    FirstName[i] = rdr.GetString(0);
                    GiftCard[i] = rdr.GetString(1);
                    Amount[i] = rdr.GetInt32(2);
                    i++;
                }

                int max_index = Array.IndexOf(Amount, Amount.Max());
                int min_index = Array.IndexOf(Amount, Amount.Min());

                Console.WriteLine("The Lowest Gift Card Record Is {0}, {1}, {2}$", FirstName[min_index], GiftCard[min_index], Amount[min_index]);
                Console.WriteLine("The Highest Gift Card Is {0}, {1}, {2}$", FirstName[max_index], GiftCard[max_index], Amount[max_index]);
            }
        }
    }
}
