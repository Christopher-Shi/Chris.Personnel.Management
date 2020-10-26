using System;

namespace Chris.Personnel.Management.Common.CommonService
{
    public interface IRedisCacheManager
    {
        /// <summary>
        /// 获取Redis缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetValue(string key);

        /// <summary>
        /// 获取Redis缓存值，并序列化
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity Get<TEntity>(string key);

        /// <summary>
        /// 设置Redis缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">有效时间</param>
        void Set(string key,object value,TimeSpan cacheTime);

        /// <summary>
        /// 判断Redis缓存值是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Get(string key);

        /// <summary>
        /// 移除Redis缓存值
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// 全部清除
        /// </summary>
        void Clear();
    }
}