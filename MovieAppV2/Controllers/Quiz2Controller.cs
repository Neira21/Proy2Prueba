using Microsoft.AspNetCore.Mvc;
 
namespace MovieAppV2.Controllers
{
    public class Quiz2Controller : Controller
    {
        public IActionResult Preguntas(){
            return View();
        }
    }
}