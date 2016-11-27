namespace HouseSales.Domain
{
    /// <summary>
    /// This marks the transaction type, i.e. newly added sale, a revision to a previous transaction record, or a deletion of an transaction recorded in error
    /// </summary>
    public enum RecordStatusType
    {
        Addition,
        Update,
        Deletion
    }
}
