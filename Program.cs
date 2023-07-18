using Cassandra;
using System.Data.SqlClient;

//DatabaseBenchMark.InitMsSql();
DatabaseBenchMark.Select(10);


class DatabaseBenchMark
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

    public static void Select(int count)
    {
        // Configure the builder with your cluster's contact points
        var cluster = Cluster.Builder()
                             .AddContactPoints("127.0.0.1")
                             .Build();

        // Connect to the nodes using a keyspace
        var session = cluster.Connect("codepool");

        // Execute a query on a connection synchronously
        var rs = session.Execute("SELECT * FROM CodePoolCollection");

        // Iterate through the RowSet
        foreach (var row in rs)
        {
            var value = row.GetValue<int>("sample_int_column");

            // Do something with the value
        }
    }
}