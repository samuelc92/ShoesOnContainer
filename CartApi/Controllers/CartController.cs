using Microsoft.AspNetCore.Mvc;

namespace CartApi.Controllers
{
    public class CartController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
            View();
        }
    }
}