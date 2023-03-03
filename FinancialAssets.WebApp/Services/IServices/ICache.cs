namespace FinancialAssets.WebApp.Services.IServices
{
    public interface ICache
    {

        public object Get(string key);

        public void Set(string key, object value);

    }
}
