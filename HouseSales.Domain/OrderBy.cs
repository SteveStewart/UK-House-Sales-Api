namespace HouseSales.Domain
{
    public enum OrderField
    {
        None = 0,
        Address = 1,
        LastSaleDate = 2,
        LastSalePrice = 3
    }

    public enum OrderDirection
    {
        Ascending = 0,
        Descending = 1
    }

    public class OrderBy
    {
        public OrderBy(OrderField field, OrderDirection direction)
        {
            Field = field;
            Direction = direction;
        }

        /// <summary>
        /// The field on which to order the results
        /// </summary>
        public OrderField Field { get; private set; }
        /// <summary>
        /// The direction to order the results, i.e. Ascending / Descending
        /// </summary>
        public OrderDirection Direction { get; private set; }
    }
}