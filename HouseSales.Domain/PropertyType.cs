namespace HouseSales.Domain
{
    /// <summary>
    /// The type of property
    /// </summary>
    public enum PropertyType
    {
        /// <summary>
        /// No property type specified
        /// </summary>
        None = -1,
        /// <summary>
        /// A detached property
        /// </summary>
        Detached = 0,
        /// <summary>
        /// A semi-detached property
        /// </summary>
        SemiDetached = 1,
        /// <summary>
        /// A terraced property
        /// </summary>
        Terraced = 2,
        /// <summary>
        /// A flat or maisonette
        /// </summary>
        FlatOrMaisonette = 3,
        /// <summary>
        /// Other, i.e. Public House
        /// </summary>
        Other = 4
    }

}
