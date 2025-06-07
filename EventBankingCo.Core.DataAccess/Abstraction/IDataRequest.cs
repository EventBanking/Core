using System.Data;

namespace EventBankingCo.Core.DataAccess.Abstraction
{
    /// <summary>
    /// This interface is used for data requests that will be executed against the database (ie SELECT, INSERT, UPDATE, DELETE).
    /// </summary>
    /// <typeparam name="TResponse">The type of data that is returned by the sql transaction.</typeparam>
    public interface IDataRequest<TResponse>
    {
        /// <summary>
        /// Gets the SQL (InlineSql or NameOfStoredProcedure) that will be executed against the database.
        /// </summary>
        public string GetSql();

        /// <summary>
        /// Gets the parameters that will be used in the SQL transaction. This can be null if there are no parameters.
        /// </summary>
        public object? GetParameters();

        /// <summary>
        /// Gets the command type that will be used to execute the SQL transaction. This is typically CommandType.Text for inline SQL or CommandType.StoredProcedure for stored procedures.
        /// </summary>
        public CommandType GetCommandType();
    }

    /// <summary>
    /// This interface is used for data requests which will execute commands and return the number of affected rows (ie INSERT, UPDATE, DELETE).
    /// </summary>
    public interface IDataExecute : IDataRequest<int>
    {
    }

    /// <summary>
    /// This interface is used for data requests which will fetch a single response (ie SELECT WHERE Id = 1).
    /// </summary>
    /// <typeparam name="TResponse">The type of object being returned from the database.</typeparam>
    public interface IDataFetch<TResponse> : IDataRequest<TResponse>
    {
    }

    /// <summary>
    /// This interface is used for data requests which will fetch a list of responses (ie SELECT WHERE Id IN (1, 2, 3)).
    /// </summary>
    /// <typeparam name="TResponse">The type of object being returned from the database.</typeparam>
    public interface IDataFetchList<TResponse> : IDataRequest<IEnumerable<TResponse>>
    {
    }
}
