using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis
{
    public interface ICacheManager<T>
    {
        void Add(string cacheItemKey, T cacheItem);
        void Add(string cacheItemKey, T cacheItem, int secondsToExpire);        
        bool ContainsKey(string cacheItemKey);
        T Get(string cacheItemKey);
        void RemoveByKey(string cacheItemKey);
    }

    public class RedisCacheManager<T> : ICacheManager<T>
    {
        private readonly string hostName;

        public RedisCacheManager()
        {
            this.hostName = "127.0.0.1:6379";
        }

        public void Add(string cacheItemKey, T cacheItem)
        {
            using (var redisClient = new BasicRedisClientManager(this.hostName).GetClient())
            {
                redisClient.Add(cacheItemKey, cacheItem);
            }
        }

        public void Add(string cacheItemKey, T cacheItem, int secondsToExpire)
        {
            using (var redisClient = new BasicRedisClientManager(this.hostName).GetClient())
            {
                redisClient.Add(cacheItemKey, cacheItem, DateTime.Now.AddSeconds(secondsToExpire));
            }
        }

        public bool ContainsKey(string cacheItemKey)
        {
            using (var redisClient = new BasicRedisClientManager(this.hostName).GetClient())
            {
                return redisClient.ContainsKey(cacheItemKey);
            }
        }

        public T Get(string cacheItemKey)
        {
            using (var redisClient = new BasicRedisClientManager(this.hostName).GetClient())
            {
                return redisClient.Get<T>(cacheItemKey);
            }
        }

        public void RemoveByKey(string cacheItemKey)
        {
            using (var redisClient = new BasicRedisClientManager(this.hostName).GetClient())
            {
                redisClient.Remove(cacheItemKey);
            }
        }
    }
}
