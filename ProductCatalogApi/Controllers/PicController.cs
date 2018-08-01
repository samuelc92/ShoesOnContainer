using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogApi.Controllers
{
    public class PicController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
            View();
        }
    }
}