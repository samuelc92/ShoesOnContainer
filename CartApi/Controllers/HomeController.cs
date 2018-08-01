using Microsoft.AspNetCore.Mvc;

namespace CartApi.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger/ui");
        }
    }
}