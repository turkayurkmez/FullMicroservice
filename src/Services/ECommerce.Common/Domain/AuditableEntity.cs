namespace ECommerce.Common.Domain
{
    public class AuditableEntity<TId> : Entity<TId> where TId : IEquatable<TId>
    {

        public string CreatedBy { get; protected set; } = string.Empty;
        public string? LastModifiedBy { get; protected set; } = string.Empty;

        protected AuditableEntity() : base()
        {

        }
        protected AuditableEntity(TId id) : base(id)
        {

        }

        public void SetCreatedBy(string createdBy)
        {
            CreatedBy = createdBy;
        }

        public void SetLastModifiedBy(string lastModifiedBy)
        {
            LastModifiedBy = lastModifiedBy;
            SetModifiedDate();
        }



    }

}
