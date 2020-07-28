using System;

namespace Chris.Personnel.Management.Common.EntityModel
{
    public class EntityReference<TEntity> : ValueObject<EntityReference<TEntity>> where TEntity : RootEntity
    {
        public Guid Id { get; protected set; }

        public static implicit operator EntityReference<TEntity>(TEntity entity)
        {
            var obj = new EntityReference<TEntity> { Id = entity.Id };

            return obj;
        }

        public static implicit operator EntityReference<TEntity>(Guid id)
        {
            var obj = new EntityReference<TEntity> { Id = id };

            return obj;
        }

        protected override int ValueObjectGetHashCode()
        {
            return Id.GetHashCode();
        }

        protected override bool ValueObjectIsEqual(EntityReference<TEntity> other)
        {
            return Id == other.Id;
        }
    }
}
