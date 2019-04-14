using System.Web.Http.Cors;
using System.Web.Mvc;

namespace AppBase.WebApp.Controllers
{
    public class DefaultController : Controller
    {
        [HttpGet]
        //[EnableCors(origins: "localhost:6070", headers: "*", methods: "*")]
        public ActionResult Index()
        {
            return View();
        }
    }
}