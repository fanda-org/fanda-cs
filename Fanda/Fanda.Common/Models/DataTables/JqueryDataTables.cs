using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FandaTabler.Models.DataTables
{
    public class DataTablesParams
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public DataTablesColumnSearch search { get; set; }
        public List<DataTablesColumnOrder> order { get; set; }
        public List<DataTablesColumn> columns { get; set; }
    }

    public enum DataTablesColumnOrderDirection
    {
        asc, desc
    }

    public class DataTablesColumnOrder
    {
        public int column { get; set; }
        public DataTablesColumnOrderDirection dir { get; set; }
    }
    public class DataTablesColumnSearch
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class DataTablesColumn
    {
        public string data { get; set; }
        public string name { get; set; }
        public Boolean searchable { get; set; }
        public Boolean orderable { get; set; }
        public DataTablesColumnSearch search { get; set; }
    }
}
