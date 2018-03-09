using System;

namespace Domain.Base
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
    }
}
