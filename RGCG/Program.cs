using System;
using MySql.Data.MySqlClient;


namespace RGCG
{
    class Program
    {
        const string ConnectionString = @"server=localhost;user id=root;password=Omadhish@1;database=rgcg";

        static void Main(string[] args)
        {
            rgcg r = new();
             r.menu();
        }
    }
}
