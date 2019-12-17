using Microsoft.AspNetCore.Components;
using FSL.Framework.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSL.DatabaseQuery.Core.Models;
using FSL.DatabaseQuery.Core.Repository;

namespace FSL.DatabaseQuery.BlazorWa.Components
{
    public class DatabaseComponent : ComponentBase
    {
        public DatabaseComponent()
        {
            Response = new DatabaseResponse();

            Request = new DatabaseRequest
            {
                DataSource = @".\SQLEXPRESS2008R2",
                DatabaseName = "consulta_cep",
                User = "sa",
                Password = "1234567890",
                TableName = "",
                OrderBy = "",
                OrderType = OrderTypes.Asc,
                PerPage = "10"
            };
        }

        protected List<IDropDownListItem> Tables { get; set; }

        protected IEnumerable<TableColumn> Columns { get; set; }

        protected IEnumerable<TableColumn> Filters => Columns?.Where(x => !x.Filter.IsNullOrEmpty());

        protected bool HasFilters => Filters != null && Filters.Count() > 0;

        protected DatabaseRequest Request { get; set; }

        protected DatabaseResponse Response { get; set; }

        [Inject]
        public IDatabaseQueryRepository DatabaseQueryRepository { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetTablesAsync();
        }

        protected async Task OnTableChangedAsync(
            string value)
        {
            Request.OrderBy = "";
            Request.TableName = value;

            Columns = await DatabaseQueryRepository.GetAllColumnsFromTable(Request, Request.TableName);

            Request.OrderBy = Columns.Where(x => x.IsOrderBy)?.FirstOrDefault()?.Name ?? "";
            Request.Page = 1;

            await GetDataAsync();
        }

        protected async Task OnOrderChangedAsync(
            string column,
            OrderTypes orderType)
        {
            Request.OrderType = orderType;
            Request.OrderBy = column;

            await GetDataAsync();
        }

        protected async Task OnPagingAsync(
            int page)
        {
            Request.Page = page;

            await GetDataAsync();
        }

        protected async Task ConectionChangedAsync(
            string value)
        {
            await GetTablesAsync();
        }

        protected async Task ValueChangedAsync(
            string value)
        {
            Request.Page = 1;

            await GetDataAsync();
        }

        protected async Task ResetFilterAsync(
            string name = null)
        {
            if (HasFilters)
            {
                if (name.IsNullOrEmpty())
                {
                    Filters.ToList().ForEach(x => x.Filter = "");
                }
                else
                {
                    Filters.Where(x => x.Name == name).ToList().ForEach(x => x.Filter = "");
                }
            }

            await ValueChangedAsync("");
        }

        private async Task GetDataAsync()
        {
            if (Request.IsConnectionOk
                && !Request.TableName.IsNullOrEmpty()
                && !Request.OrderBy.IsNullOrEmpty())
            {
                Request.Filters = Filters.ToList();
                Request.Columns = Columns?.Where(x => x.Name != "row_num")?.Select(x => x.Name)?.ToList();

                Response = await DatabaseQueryRepository.GetDataAsync(Request);
            }
        }

        private async Task GetTablesAsync()
        {
            Tables = new List<IDropDownListItem>()
            {
                new Item("", "")
            };

            if (Request.IsConnectionOk)
            {
                var items = await DatabaseQueryRepository.GetAllTables(Request);
                items.ToList().ForEach(x => Tables.Add(new Item(x, x)));
            }
        }
    }
}
