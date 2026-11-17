
using Microsoft.AspNetCore.Mvc;

namespace Packet_Sniffers.Controllers
{
    public class EarbudsController : Controller
    {
        // GET: /Earbuds
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Earbuds/Details/1
        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
