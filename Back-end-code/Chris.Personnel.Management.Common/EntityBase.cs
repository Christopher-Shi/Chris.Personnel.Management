using System;

namespace Chris.Personnel.Management.Common
{
    public class EntityBase
    {
        public Guid Id { get; }

        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
