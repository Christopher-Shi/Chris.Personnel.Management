using System;

namespace Chris.Personnel.Management.Common.Extensions
{
    public static class EntityExtensions
    {
        public static void ForceId(this RootEntity entity, Guid id)
        {
            PrivateAccess.SetPrivate(entity, "Id", id);
        }

        public static T GetPrivate<T>(this RootEntity entity, string propertyName)
        {
            return PrivateAccess.GetDynamicPrivate<T>(entity, propertyName);
        }

        public static void SetPrivate<T>(this RootEntity entity, string propertyName, object value)
        {
            PrivateAccess.SetPrivate(entity, propertyName, value);
        }
    }
}