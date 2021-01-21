using Guid = System.Guid;
// using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        // [NotMapped]
        // public Guid Uuid { get => Id; }

        public uint xmin { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html

        protected Entity(
            )
        {
        }
    }
}
