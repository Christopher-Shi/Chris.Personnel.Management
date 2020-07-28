using System;
using System.Reflection;
using Autofac;

namespace Chris.Personnel.Management.Common.EntityModel
{
    public interface IAggregateReference
    {
        Guid Id { get; }
    }

    public class AggregateReference<TAggregate> : EntityReference<TAggregate>, IAggregateReference where TAggregate : Aggregate
    {
        public static implicit operator AggregateReference<TAggregate>(TAggregate entity)
        {
            var obj = new AggregateReference<TAggregate> { Id = entity.Id };

            return obj;
        }

        public static implicit operator AggregateReference<TAggregate>(Guid id)
        {
            var obj = new AggregateReference<TAggregate> { Id = id };

            return obj;
        }

        public AggregateReference()
        {
        }

        public AggregateReference(Guid id)
        {
            Id = id;
        }

        public TAggregate Instance
        {
            get
            {
                // Make sure instance for the current Id hasn't been processed yet
                if (k__BackingFieldInstance != null && k__BackingFieldInstance.Id.Equals(Id))
                    return k__BackingFieldInstance;
                // Get aggregate instance from repository
                var serviceName = Assembly.CreateQualifiedName(
                    "Chris.Personnel.Management.Repository",
                    "Chris.Personnel.Management.Repository.I" + typeof(TAggregate).Name + "Repository");
                var repository =
                    (IBaseRepository<TAggregate>)Dependency.Container.Resolve(Type.GetType(serviceName));

                k__BackingFieldInstance = repository.Get(Id).Result;

                return k__BackingFieldInstance;
            }
        }

        private TAggregate k__BackingFieldInstance = null;
    }
}
