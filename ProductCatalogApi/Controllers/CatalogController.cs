using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogApi.Controllers
{
    public class CatalogController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
            View();
        }
    }
}