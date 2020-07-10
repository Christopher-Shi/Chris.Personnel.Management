using System;

namespace Chris.Personnel.Management.Common
{
    public abstract class RootEntity
    {
        //private set; 不能删，否则反射ForceId 会执行失败
        public Guid Id { get; private set; }

        protected RootEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
