using System;

namespace Incident.Comm.Integration.Data.Models
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        public DateTime Created { get; protected set; } = DateTime.UtcNow;

        public DateTime? Updated { get; protected set; }
        public void SetUpdated()
        {
            Updated = DateTime.UtcNow;
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}
