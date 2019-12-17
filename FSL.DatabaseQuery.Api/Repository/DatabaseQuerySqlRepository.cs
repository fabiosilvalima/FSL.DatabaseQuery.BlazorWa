using FSL.Framework.Core.Repository;
using FSL.Framework.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FSL.DatabaseQuery.Core.Repository;
using FSL.DatabaseQuery.Core.Models;
using FSL.Framework.Web.Configuration.Models;

namespace FSL.DatabaseQuery.Api.Repository
{
    public sealed class DatabaseQuerySqlRepository :
        SqlRepository,
        IDatabaseQueryRepository
    {
        public DatabaseQuerySqlRepository(
            DefaultConfiguration configuration) : base(configuration)
        {

        }

        public async Task<DatabaseResponse> GetDataAsync(
            DatabaseRequest request)
        {
            if (!request.IsConnectionOk)
            {
                return new DatabaseResponse();
            }

            try
            {
                return await WithConnectionAsync(
                    async connection =>
                    {
                        var sql = BuildQuery(request);
                        var dts = new DataSet();
                        var da = new SqlDataAdapter(sql, connection);

                        await Task.Run(() => da.Fill(dts));

                        var dtt = dts.Tables[0];
                        var totalRecordsColumn = "total_records";

                        var response = new DatabaseResponse()
                        {
                            Success = true,
                            TotalRecords = dtt.Rows.Count > 0 ?
                                Convert.ToInt32(dtt.Rows[0][totalRecordsColumn]) :
                                0
                        };

                        dtt.Columns.Remove(totalRecordsColumn);

                        response.Data = GetRows(dtt, response.TotalRecords);

                        return response;
                    },
                    request.ConnectionString);
            }
            catch (Exception ex)
            {
                return new DatabaseResponse
                {
                    Success = false,
                    Message = ex.ToString()
                };
            }
        }

        public async Task<IEnumerable<string>> GetAllTables(
            DatabaseRequest request)
        {
            if (!request.IsConnectionOk)
            {
                return new List<string>();
            }

            try
            {
                return await WithConnectionAsync(
                    async connection =>
                    {
                        var sql = @"SELECT		name AS Name
                                    FROM		sysobjects WITH (NOLOCK) 
                                    WHERE		xtype = 'U'
                                    ORDER BY	name";

                        return await connection.QueryAsync<string>(sql);

                    },
                    request.ConnectionString);
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<IEnumerable<TableColumn>> GetAllColumnsFromTable(
            DatabaseRequest request,
            string tableName)
        {
            if (!request.IsConnectionOk)
            {
                return new List<TableColumn>();
            }

            try
            {
                return await WithConnectionAsync(
                    async connection =>
                    {
                        var parameters = new
                        {
                            tableName
                        };

                        var sql = @"SELECT		c.name AS Name, 
                                                t.name AS DataType
                                    FROM		syscolumns AS c WITH (NOLOCK)
                                    INNER JOIN	systypes AS t ON c.xtype = t.xusertype
                                    WHERE		OBJECT_NAME(c.id) = @tableName
                                    ORDER BY	c.name";

                        return await connection.QueryAsync<TableColumn>(
                            sql,
                            parameters);

                    },
                    request.ConnectionString);
            }
            catch
            {
                return new List<TableColumn>();
            }
        }

        private List<List<string>> GetRows(
            DataTable dtt,
            int totalRecords)
        {
            var rows = new List<List<string>>();

            if (totalRecords == 0)
            {
                return rows;
            }

            foreach (DataRow dtr in dtt.Rows)
            {
                var columns = new List<string>();

                foreach (DataColumn dtc in dtt.Columns)
                {
                    var value = dtr[dtc]?.ToString();
                    columns.Add(value ?? "");
                }

                rows.Add(columns);
            }

            return rows;
        }

        private string BuildQuery(
            DatabaseRequest request)
        {
            var page = (request.Page <= 0) ? 1 : request.Page;
            var rows = (request.Rows <= 0) ? 10 : request.Rows;
            var select = BuildSelect(request);
            var where = BuildWhere(request);

            var sql = $@"
                    DECLARE @p_num_page AS INT
                    DECLARE @p_page_size AS INT
                    SET @p_num_page = {page}
                    SET @p_page_size = {rows}
                    SET @p_num_page = ((@p_num_page * @p_page_size) - @p_page_size) + 1
                    SET @p_page_size = @p_num_page + @p_page_size - 1
                    BEGIN WITH [query] AS (SELECT ROW_NUMBER() OVER (ORDER BY {request.OrderBy} {request.OrderType.ToString().ToUpper()}) AS [row_num], {select} FROM [{request.DatabaseName}].[dbo].[{request.TableName}] WITH (NOLOCK) {where}) SELECT * FROM [query] CROSS JOIN (SELECT COUNT(*) AS [total_records] FROM [query]) AS [counters] WHERE ([row_num] BETWEEN @p_num_page AND @p_page_size) END ";

            return sql;
        }

        private string BuildSelect(
            DatabaseRequest request)
        {
            if (request.Columns.IsNotNull()
                && request.Columns.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var columnName in request.Columns)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(columnName);
                }

                return sb.ToString();
            }

            return "*";
        }

        private string BuildWhere(
            DatabaseRequest request)
        {
            if (request.Filters.IsNotNull()
                && request.Filters.Count > 0)
            {
                var sb = new StringBuilder();

                foreach (var filter in request.Filters)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" AND ");
                    }

                    if (filter.IsLike)
                    {
                        sb.AppendFormat(
                            " {0} LIKE '%{1}%'",
                            filter.Name,
                            filter.Filter);
                    }
                    else if (filter.IsEqual)
                    {
                        if (filter.DataType.ToString() == "uniquidentifier")
                        {
                            sb.AppendFormat(
                                " {0} = '{1}'",
                                filter.Name,
                                filter.Filter);
                        }
                        else
                        {
                            sb.AppendFormat(
                                " {0} = {1}",
                                filter.Name,
                                filter.Filter);
                        }
                    }
                }

                return $" WHERE ({sb.ToString()}) ";
            }

            return "";
        }
    }
}
