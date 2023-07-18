using Bogus;

namespace mssql_cassandra_benchmark.src.Model
{
    public class CodePoolCollectionModel
    {
        public long ID { get; set; }

        public string Code { get; set; }

        public int Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        private static Faker<CodePoolCollectionModel> GetCodePoolCollectionGenerator()
        {
            return new Faker<CodePoolCollectionModel>()
                .RuleFor(v => v.ID, f => f.IndexGlobal)
                .RuleFor(v => v.Code, f => f.Name.FirstName())
                .RuleFor(v => v.Status, f => f.PickRandom(new[] { 0, 1, 2 }))
                .RuleFor(v => v.CreatedAt, f => f.Date.Between(new DateTime(2010, 1, 1), new DateTime(2015, 7, 1)))
                .RuleFor(v => v.ModifiedAt, f => f.Date.Between(new DateTime(2015, 1, 1), new DateTime(2023, 7, 1)));
        }

        public static List<CodePoolCollectionModel> GenerateCodePoolCollection(int number)
        {
            return GetCodePoolCollectionGenerator().Generate(number);
        }
    }
}
