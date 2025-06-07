namespace EventBankingCo.Core.DataAccess.Abstraction
{
    public interface IDataAccess
    {
        public int Execute(IDataExecute request);

        public Task<int> ExecuteAsync(IDataExecute request);

        public TResponse? Fetch<TResponse>(IDataFetch<TResponse> request);

        public Task<TResponse?> FetchAsync<TResponse>(IDataFetch<TResponse> request);

        public IEnumerable<TResponse> FetchList<TResponse>(IDataFetchList<TResponse> request);

        public Task<IEnumerable<TResponse>> FetchListAsync<TResponse>(IDataFetchList<TResponse> request);
    }
}
