using LinFx.Domain.Entities;

namespace LinFx.Test.Domain.Models
{
    internal class User : Entity<long>
    {
        public string Name { get; set; }
    }
}
