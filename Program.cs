using mssql_cassandra_benchmark.src.DatabaseBenchMark;

while (true)
{
    Console.WriteLine("Enter a number to start bench test");
    var isSuccess = Int32.TryParse(Console.ReadLine(), out var number);
    if (isSuccess)
        DatabaseBenchMark.Start(number);
}
