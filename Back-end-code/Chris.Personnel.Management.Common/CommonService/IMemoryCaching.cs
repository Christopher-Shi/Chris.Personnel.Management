namespace Chris.Personnel.Management.Common.CommonService
{
    public interface IMemoryCaching
    {
        object Get(string cacheKey);

        void Set(string cacheKey, object cacheValue);

        void Set(string cacheKey, object cacheValue, double expirationMinutes);
    }
}