using Microsoft.AspNetCore.Mvc;

namespace GoodFoodProjectMVC.Controllers
{
    public class RecipeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
