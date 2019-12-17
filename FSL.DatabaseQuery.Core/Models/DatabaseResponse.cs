using FSL.Framework.Core.Models;
using System.Collections.Generic;

namespace FSL.DatabaseQuery.Core.Models
{
    public sealed class DatabaseResponse : BaseResult<List<List<string>>>
    {
        public int TotalRecords { get; set; }
    }
}
