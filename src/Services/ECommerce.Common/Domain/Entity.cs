namespace ECommerce.Common.Domain
{
    public abstract class Entity<TId> where TId : IEquatable<TId>
    {
        public TId Id { get; protected set; }

        public DateTime CreatedDate { get; protected set; }
        public DateTime LastModifiedDate { get; protected set; }


        protected Entity()
        {

            //Eğer TId Guid ise yeni bir Guid oluşturuyoruz.
            if (typeof(TId) == typeof(Guid))
            {
                Id = (TId)(object)Guid.NewGuid();
            }
            else
            {
                //Diğer durumlarda default değeri atıyoruz.
                Id = default(TId)!;
            }

            CreatedDate = DateTime.UtcNow;
        }

        public Entity(TId id) : this()
        {
            Id = id;
        }

        public void SetModifiedDate()
        {
            LastModifiedDate = DateTime.UtcNow;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (GetType() != other.GetType())
            {
                return false;
            }
            if (Id.Equals(default) || other.Id.Equals(default))
            {
                return false;
            }
            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<TId> a, Entity<TId> b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            if (a is null || b is null)
            {
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator !=(Entity<TId> a, Entity<TId> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() * 41;
        }
    }
}
