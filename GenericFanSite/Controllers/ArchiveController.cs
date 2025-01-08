using Microsoft.AspNetCore.Mvc;

namespace GenericFanSite.Controllers
{
    public class ArchiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
