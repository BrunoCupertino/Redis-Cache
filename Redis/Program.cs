using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis
{
    class Program
    {
        static void Main(string[] args)
        {
            EntitiesInfo();
        }

        private static void EntitiesInfo()
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            string source;

            var entities = GetEntities(out source);

            stopWatch.Stop();

            Console.WriteLine("{0} rows from {1} - {2} milisecond(s)", entities.Count(), source, stopWatch.Elapsed.Milliseconds);

            Console.Write("Continue y/n?");

            var keyInfo = Console.ReadKey();

            if (keyInfo.KeyChar == 'y')
            {
                Console.WriteLine();

                EntitiesInfo();
            }
        }

        static IEnumerable<Entity> GetEntities(out string source)
        {
            var key = "recentes";

            ICacheManager<IEnumerable<Entity>> cacheManager = new RedisCacheManager<IEnumerable<Entity>>();

            IEnumerable<Entity> entities = cacheManager.Get(key);

            source = "Redis";

            if (entities == null)
            {
                source = "EntityFramework";

                //go to repository
                using (var context = new DataContext())
                {
                    var query = context.Entidades.Where(e => e.ID > 1000).Take(10).OrderBy(e => e.ID);

                    entities = query.ToList();
                }

                cacheManager.Add(key, entities, 5);
            }

            return entities;
        }
    }
}
