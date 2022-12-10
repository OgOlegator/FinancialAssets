using Microsoft.AspNetCore.Mvc;

namespace FinancialAssets.WebApp.Controllers
{
    public class WaletAssetsController : Controller
    {
        
        
        public WaletAssetsController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
