using Cassandra;
using Microsoft.EntityFrameworkCore;
using mssql_cassandra_benchmark.src.Data;
using mssql_cassandra_benchmark.src.Data.Entity;
using mssql_cassandra_benchmark.src.Model;
using System.Data.SqlClient;
using System.Diagnostics;

namespace mssql_cassandra_benchmark.src.DatabaseBenchMark
{
    public class DatabaseBenchMark
    {
        public static void InitMsSql()
        {
            SqlConnection con = new SqlConnection(@"Data Source=localhost;Initial Catalog=CodePool;User Id=sa; Password=Keyboard_cat1234;");
            string query =
            @"CREATE TABLE dbo.CodePoolCollection
                (
                    ID bigint IDENTITY(1,1) NOT NULL,
                    Code nvarchar(12) NOT NULL,
                    Status tinyint NOT NULL,
                    CreatedAt datetime NOT NULL,
                    ModifiedAt datetime NOT NULL,
                );";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Table Created Successfully");
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }
            finally
            {
                con.Close();
                Console.ReadKey();
            }
        }

        public static void InitCassandra()
        {
            // TODO
        }

        public static void SelectMsSQL()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            using (var context = new CodePoolStore())
            {
                var data = context.CodePoolCollection.ToList();
            }

            stopWatch.Stop();
            Console.WriteLine($"[SELECT] MsSQL took {stopWatch.ElapsedMilliseconds} ms");
        }

        public static void SelectCassandra()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Configure the builder with your cluster's contact points
            var cluster = Cluster.Builder()
                                 .AddContactPoints("127.0.0.1")
                                 .Build();

            // Connect to the nodes using a keyspace
            var session = cluster.Connect("codepool");

            // Execute a query on a connection synchronously
            var rs = session.Execute($"SELECT * FROM CodePoolCollection");

            stopWatch.Stop();
            Console.WriteLine($"[SELECT] Cassandra took {stopWatch.ElapsedMilliseconds} ms");
        }

        public static void Start(int number)
        {
            ClearTables();
            var fakeData = CodePoolCollectionModel.GenerateCodePoolCollection(number);

            InsertMsSQL(fakeData);
            InsertCassandra(fakeData);
            SelectMsSQL();
            SelectCassandra();
        }

        public static void InsertMsSQL(List<CodePoolCollectionModel> codePoolCollections)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            using (var context = new CodePoolStore())
            {
                context.CodePoolCollection.AddRange(codePoolCollections.Select(item => new CodePoolCollection
                {
                    Code = item.Code,
                    Status = (byte)item.Status,
                    CreatedAt = item.CreatedAt,
                    ModifiedAt = item.ModifiedAt
                }));

                context.SaveChanges();
            }

            stopWatch.Stop();
            Console.WriteLine($"[INSERT] MsSQL took {stopWatch.ElapsedMilliseconds} ms");
        }

        public async static void InsertCassandra(List<CodePoolCollectionModel> codePoolCollections)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Configure the builder with your cluster's contact points
            var cluster = Cluster.Builder()
                                 .AddContactPoints("127.0.0.1")
                                 .Build();

            // Connect to the nodes using a keyspace
            var session = cluster.Connect("codepool");

            // Execute a query on a connection synchronously
            // Iterate through the RowSet
            foreach (var row in codePoolCollections)
            {
                var query = ($"INSERT INTO CodePoolCollection (id, code, status, createdat, modifiedat) " +
                   $"VALUES ({row.ID}, '{row.Code.Replace('\'', ' ')}', {row.Status}, '{row.CreatedAt.ToString("yyyy-MM-dd")}', '{row.ModifiedAt.ToString("yyyy-MM-dd")}')");
                try
                {
                    session.Execute(query);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            stopWatch.Stop();
            Console.WriteLine($"[INSERT] Cassandra took {stopWatch.ElapsedMilliseconds} ms");
        }

        private static void ClearTables()
        {
            using (var context = new CodePoolStore())
            {
                context.Database.ExecuteSql($"TRUNCATE TABLE CodePoolCollection");
            }

            // Configure the builder with your cluster's contact points
            var cluster = Cluster.Builder()
                                 .AddContactPoints("127.0.0.1")
                                 .Build();

            // Connect to the nodes using a keyspace
            var session = cluster.Connect("codepool");
            session.Execute("TRUNCATE TABLE CodePoolCollection");
        }
    }
}
