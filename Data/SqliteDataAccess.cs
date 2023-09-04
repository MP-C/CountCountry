using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace CountCountry.Data
{
    public class SqliteDataAccess
    {
		public static List<CountryEntity> CheckCountryExist(CountryEntity countryEntity)
        {
            // var CountryName = countryEntity.CountryName;
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                OpenDB();
                var id = cnn.Query<CountryEntity>("SELECT Id FROM CountryRegister WHERE CountryName = (@CountryName);", new DynamicParameters());
                CloseDB();
                return id.ToList();
            }
        }

        public static List<CountryEntity> LoadAllInfoCountries()
        {
            try
            {
                //string AssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
                //string databaseName = "CountCountryDB.db";// connectionStringParts[0];
                //string directory = AssemblyPath + "\\" + databaseName;//IConfiguration configuration = new ConfigurationBuilder()

                //using (IDbConnection cnn = new SqliteConnection(directory)
                //{

                //    cnn.Open();
                //    var output = cnn.Query<CountryEntity>("SELECT * from test;", new DynamicParameters());
                //    Close();
                //    return output.ToList();

                //}

                //using (var connection = new SqliteConnection(@"Data Source=example.db3"))

                List<CountryEntity> listResults = new List<CountryEntity>();
                using (var connection = new SqliteConnection(@"Data Source=C:\Users\capelam\source\repos\CountCountry\bin\Debug\net6.0-windows10.0.19041.0\win10-x64\CountCountryDB.db3"))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = @"SELECT * FROM CountryRegister;";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var output = new CountryEntity();
                            output.Id = reader.GetInt32(0);
                            output.CountryName = reader.GetString(1);
                            output.CityName = reader.GetString(2);
                            output.StartDateCount = DateTime.ParseExact(reader.GetString(3).ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            output.EndDateCount = DateTime.ParseExact(reader.GetString(4).ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            output.InsertDate = DateTime.ParseExact(reader.GetString(5).ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            output.BucketList = reader.GetString(6);
                            output.TotalDaysStay = reader.GetString(7);
                            output.SharedStayCount = reader.GetString(8);

                            listResults.Add(output);
                        }
                    }
                }
                return listResults;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



		public static List<CountryEntity> GetTotalCountries()
        {
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT count(*) from CountryRegister", new DynamicParameters());
                return output.ToList();
            }
        }

		public static List<CountryEntity> GetOrderCountriesNames()
        {
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName from CountryRegister Group By CountryName;", new DynamicParameters());
                return output.ToList();
            }
        }

		public static List<CountryEntity> GetBucketListWantVisit()
        {
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName \"BucketListWantVisit\" FROM CountryRegister WHERE BucketList = '1';", new DynamicParameters());
                return output.ToList();
            }
        }

		public static List<CountryEntity> GetBucketListNotVisitedYet()
        {
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName \"BucketListNotVisitedYet\" FROM CountryRegister WHERE BucketList = '1' AND StartDateCount = '31/12/2999' Group By CountryName;", new DynamicParameters());
                return output.ToList();
            }
        }

		public static List<CountryEntity> GetBucketListAlreadyVisited()
        {
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName \"BucketListAlreadyVisited\" FROM CountryRegister WHERE BucketList = '1' AND StartDateCount != '31/12/2999' Group By CountryName;", new DynamicParameters());
                return output.ToList();
            }
        }

		public static List<CountryEntity> GetLongerStayCountry()
        {
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName \"LongerStayCountry\", TotalDaysStay FROM CountryRegister ORDER BY TotalDaysStay DESC LIMIT 1;", new DynamicParameters());
                return output.ToList();
            }
        }

		public static List<CountryEntity> GetAllStaysCountryDetails()
        {
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName, Min(StartDateCount), Max(EndDateCount) From CountryRegister Group By CountryName;", new DynamicParameters());
                return output.ToList();
            }
        }

		public static void SaveCountryRegister(CountryEntity country)
        {
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT into CountryRegister (CountryName, CityName, StartDateCount, EndDateCount, InsertDate, BucketList, TotalDaysStay, SharedStayCount)" +
                    "values (@CountryName, @CityName, @StartDateCount, @EndDateCount, @InsertDate, @BucketList, @TotalDaysStay, @SharedStayCount)", country);
            }
        }

        // Allows to stay open correctly, and close proprely the DB even when something missing, or goes wrong
        public static string LoadConnectionString(string id = "Default")
        {

            try
            {
                string AssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
                //IConfiguration configuration = new ConfigurationBuilder()
                //.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                //.AddJsonFile("App.config", optional: true)  //App.config
                //.Build();
                string connectionString = LoadConnectionString();
                string[] connectionStringParts = connectionString.Split(";");
                string databaseName = connectionStringParts[0];
                string directory = AssemblyPath + databaseName;
                return directory;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            /**
          
          
         try
         {
             string AssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
             //IConfiguration configuration = new ConfigurationBuilder()
             //.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             //.AddJsonFile("App.config", optional: true)  //App.config
             //.Build();
             //string connectionString = ConfigurationManager.ConnectionStrings["Data Source"].ToString();
             //string[] connectionStringParts = connectionString.Split(";");
             string databaseName = "CountCountryDB.db";// connectionStringParts[0];
             string directory = AssemblyPath + "\\" + databaseName;

             return directory.ToString();
         }
         catch (Exception e)
         {
             throw new Exception(e.Message);
         }
         //try
         //{
         //    IConfiguration configuration = new ConfigurationBuilder()
         //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
         //        .AddXmlFile("App.config", optional: true) // Use AddXmlFile for XML-based configuration files like App.config
         //        .Build();

         //    // Use the "id" parameter to select the appropriate connection string if you have multiple
         //    string connectionString = configuration.GetConnectionString(id);

         //    // Parse the connection string to get the Data Source
         //    var builder = new System.Data.Common.DbConnectionStringBuilder();
         //    builder.ConnectionString = connectionString;

         //    if (builder.ContainsKey("Data Source"))
         //    {
         //        return builder["Data Source"].ToString();
         //    }
         //    else
         //    {
         //        // Handle the case where "Data Source" is not found in the connection string
         //        throw new Exception("Data Source not found in connection string.");
         //    }
         //}
         //catch (Exception e)
         //{
         //    throw new Exception(e.Message);
         //}
          **/
        }

        public static void OpenDB()
        {
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                cnn.Open();
            }
        }

        public static void CloseDB()
        {
            using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                cnn.Close();
            }
        }
    }
}
