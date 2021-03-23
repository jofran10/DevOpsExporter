namespace DataTransfer.Model.Data
{

    /// <summary>
    /// Provides primary and obrigatory fields in data model entities
    /// </summary>
    public interface IDbEntity<TKEY>
    {

        /// <summary>
        /// Define base primary key
        /// </summary>
        TKEY Id { get; set; }

        /// <summary>
        /// Control of logical delete
        /// </summary>
        bool IsActive { get; set; }

    }
}
