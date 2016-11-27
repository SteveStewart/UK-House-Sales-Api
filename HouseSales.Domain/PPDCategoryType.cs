namespace HouseSales.Domain
{
    /// <summary>
    /// The type of price paid transaction
    /// </summary>
    public enum PPDCategoryType
    {
        /// <summary>
        /// Standard sales for full market value
        /// </summary>
        StandardPricePaid,
        /// <summary>
        /// Reposessions, buy-to-lets, transfers to non-private entities
        /// </summary>
        AdditionalPricePaid
    }
}
