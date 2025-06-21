using EventBankingCo.Core.DataAccess.DataRequests;

namespace EventBankingCo.Core.DataAccess.CommonRequests
{
    public abstract class DeleteById : SqlCommand
    {
        protected DeleteById(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public abstract string GetTableName();

        public override object? GetParameters() => new { Id };

        public override string GetSql() => $"DELETE FROM {GetTableName()} WHERE Id = @Id;";
    }
}
