namespace PH.Core3.EntityFramework
{
    public interface ITenantContext
    {
        string TenantId { get; set; }
    }
}