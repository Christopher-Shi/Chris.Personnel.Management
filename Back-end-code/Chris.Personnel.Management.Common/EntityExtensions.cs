using System;

namespace Chris.Personnel.Management.Common
{
    public static class EntityExtensions
    {
        public static void ForceId(this EntityBase entity, Guid id)
        {
            PrivateAccess.SetPrivate(entity, "Id", id);
        }

        public static T GetPrivate<T>(this EntityBase entity, string propertyName)
        {
            return PrivateAccess.GetDynamicPrivate<T>(entity, propertyName);
        }

        public static void SetPrivate<T>(this EntityBase entity, string propertyName, object value)
        {
            PrivateAccess.SetPrivate(entity, propertyName, value);
        }
    }
}