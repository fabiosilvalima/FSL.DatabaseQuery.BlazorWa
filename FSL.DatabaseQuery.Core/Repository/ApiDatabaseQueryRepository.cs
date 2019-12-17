using FSL.DatabaseQuery.Core.Models;
using FSL.Framework.Core.ApiClient.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSL.DatabaseQuery.Core.Repository
{
    public sealed class ApiDatabaseQueryRepository : IDatabaseQueryRepository
    {
        private readonly IApiClientService _apiClientService;

        public ApiDatabaseQueryRepository(
            IApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IEnumerable<TableColumn>> GetAllColumnsFromTable(
            DatabaseRequest request, 
            string tableName)
        {
            var apiClient = await _apiClientService.CreateInstanceAsync();
            var result = await apiClient.PostAsync<List<TableColumn>>($"database/columns", request);

            return result.Data;
        }

        public async Task<IEnumerable<string>> GetAllTables(
            DatabaseRequest request)
        {
            var apiClient = await _apiClientService.CreateInstanceAsync();
            var result = await apiClient.PostAsync<List<string>>($"database/tables", request);

            return result.Data;
        }

        public async Task<DatabaseResponse> GetDataAsync(
            DatabaseRequest request)
        {
            var apiClient = await _apiClientService.CreateInstanceAsync();
            var result = await apiClient.PostAsync<DatabaseResponse>($"database/data", request);

            return result.Data;
        }
    }
}
