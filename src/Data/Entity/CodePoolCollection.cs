namespace mssql_cassandra_benchmark.src.Data.Entity
{
    public class CodePoolCollection
    {
        public long ID { get; set; }

        public string Code { get; set; }

        public byte Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
