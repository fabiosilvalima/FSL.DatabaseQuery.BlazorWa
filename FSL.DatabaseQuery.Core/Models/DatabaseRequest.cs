using System;
using System.Collections.Generic;
using FSL.Framework.Core.Extensions;

namespace FSL.DatabaseQuery.Core.Models
{
    public enum OrderTypes
    {
        Asc,
        Desc
    }

    public sealed class DatabaseRequest
    {
        public List<string> Columns { get; set; }
        public string DataSource { get; set; }
        public string DatabaseName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string TableName { get; set; }
        public string OrderBy { get; set; }
        public string OrderTypeStr { get; set; }
        public OrderTypes OrderType
        {
            get => (OrderTypes)Enum.Parse(typeof(OrderTypes), OrderTypeStr);
            set => OrderTypeStr = value.ToString();
        }
        public int Page { get; set; }
        public int Rows => Convert.ToInt32(PerPage);
        public string PerPage { get; set; }
        public List<TableColumn> Filters { get; set; }

        public bool IsConnectionOk
        {
            get
            {
                if (DatabaseName.IsNullOrEmpty()
                    || DataSource.IsNullOrEmpty()
                    || Password.IsNullOrEmpty()
                    || User.IsNullOrEmpty())
                {
                    return false;
                }

                return true;
            }
        }

        public string ConnectionString => $@"Data Source={DataSource};Initial Catalog={DatabaseName};User ID={User};Password={Password};Persist Security Info=False;Connect Timeout=200000";
    }
}
