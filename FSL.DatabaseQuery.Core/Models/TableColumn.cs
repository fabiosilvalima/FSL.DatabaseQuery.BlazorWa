using Newtonsoft.Json;

namespace FSL.DatabaseQuery.Core.Models
{
    public sealed class TableColumn
    {
        public string Filter { get; set; }

        public string Name { get; set; }

        public string DataType { get; set; }

        [JsonIgnore]
        public bool IsOrderBy
        {
            get
            {
                var name = DataType.ToLower();

                var flag = name == "varbinary"
                    || name == "ntext"
                    || name == "text"
                    || name == "image"
                    || name == "sql_variant"
                    || name == "geometry"
                    || name == "geography"
                    || name == "binary"
                    || name == "xml";

                return !flag;
            }
        }

        [JsonIgnore]
        public bool IsLike
        {
            get
            {
                var name = DataType.ToLower();

                var flag = name == "nchar"
                    || name == "char"
                    || name == "varchar"
                    || name == "nvarchar"
                    || name == "ntext"
                    || name == "text"
                    || name == "xml";

                return flag;
            }
        }

        [JsonIgnore]
        public bool IsEqual
        {
            get
            {
                var name = DataType.ToLower();

                var flag = name == "uniquidentifier"
                    || name == "int"
                    || name == "bigint"
                    || name == "tinyint"
                    || name == "bit";

                return flag;
            }
        }

        [JsonIgnore]
        public bool IsBool
        {
            get
            {
                var name = DataType.ToLower();

                var flag = name == "bit";

                return flag;
            }
        }

        [JsonIgnore]
        public bool IsFilter => IsLike || IsEqual;
    }
}
