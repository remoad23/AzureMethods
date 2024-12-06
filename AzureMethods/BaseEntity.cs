using Azure.Data.Tables;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMethods
{
    public class BaseEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        // Required for Table Storage, but you can leave it unimplemented if you don't use it
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
