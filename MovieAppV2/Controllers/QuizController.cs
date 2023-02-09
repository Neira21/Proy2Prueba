using Microsoft.AspNetCore.Mvc;
using MovieAppV2.Models;

namespace MovieAppV2.Controllers
{
    public class QuizController : Controller
    {
      public IActionResult Trivia()
      {
        return View();
      }

      public IActionResult ResultadoCreate(Quiz objquiz)
      {
        objquiz.ResulFinal = objquiz.RPg1 + objquiz.RPg2 + objquiz.RPg3;
        
        if(objquiz.ResulFinal>2)
        {
        objquiz.Mensaje = "Tus Aciertos son: " + objquiz.ResulFinal + " Felicidades Acertastes todas";
        }
        else
        {
        objquiz.Mensaje = "Tus Aciertos son: " + objquiz.ResulFinal;
        }

        return View("Trivia",objquiz);
      }   
    }
}