using System.Data;

namespace URF.Core.Helper.Extensions
{
    public static class DataTableExtensions
    {

        public static void SetColumnsOrder(this DataTable table, params string[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }

        public static void ChangeColumnName(this DataTable table, string oldColumn, string newColumn)
        {
            if (table.Columns[oldColumn] != null)
            {
                table.Columns[oldColumn].ColumnName = newColumn;
            }
        }
    }
}
