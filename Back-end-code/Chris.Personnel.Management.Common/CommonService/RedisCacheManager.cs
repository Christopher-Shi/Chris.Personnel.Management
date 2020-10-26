using System;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Common.Helper;
using StackExchange.Redis;

namespace Chris.Personnel.Management.Common.CommonService
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly string _redisConnectionString;

        public volatile ConnectionMultiplexer RedisConnection;

        private readonly object _redisConnectionLock = new object();

        public RedisCacheManager()
        {
            var redisConfiguration = AppSettings.Apply("AOP", "RedisCachingAOP", "ConnectionString");
            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("redis config is empty", nameof(redisConfiguration));
            }

            _redisConnectionString = redisConfiguration;
            RedisConnection = GetRedisConnection();
        }


        public string GetValue(string key)
        {
            return RedisConnection.GetDatabase().StringGet(key);
        }

        public TEntity Get<TEntity>(string key)
        {
            var value = RedisConnection.GetDatabase().StringGet(key);
            return value.HasValue ? Json.FromJson<TEntity>(value) : default;
        }

        public void Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                RedisConnection.GetDatabase().StringSet(key, Json.ToJson(value), cacheTime);
            }
        }

        public bool Get(string key)
        {
            return RedisConnection.GetDatabase().KeyExists(key);
        }

        public void Remove(string key)
        {
            RedisConnection.GetDatabase().KeyDelete(key);
        }

        public void Clear()
        {
            foreach (var endPoint in GetRedisConnection().GetEndPoints())
            {
                var server = GetRedisConnection().GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    RedisConnection.GetDatabase().KeyDelete(key);
                }
            }
        }

        /// <summary>
        /// 核心代码，获取连接实例
        /// 通过双if 加lock的方式，实现单例模式
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (RedisConnection != null && RedisConnection.IsConnected)
            {
                return RedisConnection;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (_redisConnectionLock)
            {
                //释放redis连接
                RedisConnection?.Dispose();
                try
                {
                    var config = new ConfigurationOptions
                    {
                        AbortOnConnectFail = false,
                        AllowAdmin = true,
                        ConnectTimeout = 15000,//改成15s
                        SyncTimeout = 5000,
                        //Password = "Pwd",//Redis数据库密码
                        EndPoints = { _redisConnectionString }
                    };
                    RedisConnection = ConnectionMultiplexer.Connect(config);
                }
                catch (Exception)
                {
                    throw new Exception("Redis服务未开启，请开启该服务");
                }
            }
            return RedisConnection;
        }
    }
}