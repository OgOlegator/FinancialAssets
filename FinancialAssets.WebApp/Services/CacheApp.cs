using FinancialAssets.WebApp.Services.IServices;

namespace FinancialAssets.WebApp.Services
{
    public class CacheApp : ICache
    {
        private Dictionary<string, object> _cache = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public object Get(string key)
        {
            try
            {
                if (_cache.TryGetValue(key, out object value))
                    return value;
                else
                    return null;
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException();
            }
        }


        public void Set(string key, object value)
        {
            _cache[key] = value;
        }
    }
}
