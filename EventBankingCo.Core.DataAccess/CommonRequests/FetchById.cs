using EventBankingCo.Core.DataAccess.DataRequests;

namespace EventBankingCo.Core.DataAccess.CommonRequests
{
    public abstract class FetchById<T> : SqlFetch<T>
    {
        public int Id { get; set; }

        public FetchById(int id)
        {
            Id = id;
        }

        public abstract string GetTableName();

        public virtual string GetColmuns() => "*";

        public override object? GetParameters() => new { Id };

        public override string GetSql() => $"SELECT {GetColmuns()} FROM {GetTableName()} WITH(NOLOCK) WHERE Id = @Id;";
    }
}
