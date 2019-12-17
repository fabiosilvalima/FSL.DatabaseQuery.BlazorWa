using FSL.DatabaseQuery.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSL.DatabaseQuery.Core.Repository
{
    public interface IDatabaseQueryRepository
    {
        Task<DatabaseResponse> GetDataAsync(
            DatabaseRequest request);

        Task<IEnumerable<string>> GetAllTables(
            DatabaseRequest request);

        Task<IEnumerable<TableColumn>> GetAllColumnsFromTable(
            DatabaseRequest request,
            string tableName);
    }
}
