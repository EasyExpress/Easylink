namespace Easylink
{
    internal class Criteria
    {
        internal  Criteria(string propertyName, string op, dynamic value,  bool caseSensitive = false)
        {
            PropertyName = propertyName;
            Operator = op;
            Value = value;
            CaseSensitive = caseSensitive;
        }

        internal  bool CaseSensitive { get; set; }

        internal  string PropertyName { get; set; }

        internal  string ColumnName { get; private set; }

        internal object Value { get; set; }

        internal  string Operator { get; set; }

        internal void SetColumnName(string columnName)
        {
            ColumnName = columnName;
        }
    }
}