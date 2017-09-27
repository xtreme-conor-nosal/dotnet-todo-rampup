using System;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public bool IsComplete { set; get; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(IsComplete)}: {IsComplete}";
        }

        protected bool Equals(TodoItem other)
        {
            return Id == other.Id && string.Equals(Name, other.Name) && IsComplete == other.IsComplete;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TodoItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsComplete.GetHashCode();
                return hashCode;
            }
        }
    }
}