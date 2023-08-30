using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CountCountry.Data
{
    public class SqliteDataAccess
    {
        private static List<CountryEntity> CheckCountryExist()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var id = cnn.Query<CountryEntity>("SELECT Id FROM CountryRegister WHERE CountryName = (@CountryName)", new DynamicParameters());
                return id.ToList();
            }
        }

        private static List<CountryEntity> LoadAllInfoCountries()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT * from CountryRegister", new DynamicParameters());
                return output.ToList();
            } 
        }

        private static List<CountryEntity> GetTotalCountries()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT count(*) from CountryRegister", new DynamicParameters());
                return output.ToList();
            }
        }

        private static List<CountryEntity> GetOrderCountriesNames()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName from CountryRegister Group By CountryName;", new DynamicParameters());
                return output.ToList();
            }
        }

        private static List<CountryEntity> GetBucketListWantVisit()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName \"BucketListWantVisit\" FROM CountryRegister WHERE BucketList = '1';", new DynamicParameters());
                return output.ToList();
            }
        }

        private static List<CountryEntity> GetBucketListNotVisitedYet()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName \"BucketListNotVisitedYet\" FROM CountryRegister WHERE BucketList = '1' AND StartDateCount = '31/12/2999' Group By CountryName;", new DynamicParameters());
                return output.ToList();
            }
        }
        
        private static List<CountryEntity> GetBucketListAlreadyVisited()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName \"BucketListAlreadyVisited\" FROM CountryRegister WHERE BucketList = '1' AND StartDateCount != '31/12/2999' Group By CountryName;", new DynamicParameters());
                return output.ToList();
            }
        }

        private static List<CountryEntity> GetLongerStayCountry()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName \"LongerStayCountry\", TotalDaysStay FROM CountryRegister ORDER BY TotalDaysStay DESC LIMIT 1;", new DynamicParameters());
                return output.ToList();
            }
        }

        private static List<CountryEntity> GetAllStaysCountryDetails()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CountryEntity>("SELECT CountryName, Min(StartDateCount), Max(EndDateCount) From CountryRegister Group By CountryName;", new DynamicParameters());
                return output.ToList();
            }
        }

        private static void SaveCountryRegister(CountryEntity country)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT into CountryRegister (CountryName, CityName, StartDateCount, EndDateCount, InsertDate, BucketList, TotalDaysStay, SharedStayCount)" +
                    "values (@CountryName, @CityName, @StartDateCount, @EndDateCount, @InsertDate, @BucketList, @TotalDaysStay, @SharedStayCount)", country);
            }
        }

        // Allows to stay open correctly, and close proprely the DB even when something missing, or goes wrong
        private static string LoadConnectionString(string id = "Default")
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)  //App.config
                .Build();

            return configuration.GetConnectionString(id);
        }
    }
}
