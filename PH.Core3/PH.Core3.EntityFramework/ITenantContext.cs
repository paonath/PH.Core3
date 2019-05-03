namespace PH.Core3.EntityFramework
{
    /// <summary>
    /// Tenant Context
    /// </summary>
    public interface ITenantContext
    {
        /// <summary>Gets or sets the tenant identifier.</summary>
        /// <value>The tenant identifier.</value>
        string TenantId { get; set; }
    }
}