using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;
using MovieAppV2.Models;
using System.Collections.Generic;
using MovieAppV2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MovieAppV2.Controllers
{
    public class PeliculaController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly MovieContext _context;
        private UserManager<IdentityUser> _um;
        private SignInManager<IdentityUser> _sim;
        public PeliculaController(ILogger<HomeController> logger,
            MovieContext context, UserManager<IdentityUser> um, SignInManager<IdentityUser> sim)
        {
            _logger = logger;
            _context = context;
            _um = um;
            _sim = sim;
        }
        public IActionResult AgregarPelicula()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AgregarPelicula(Pelicula objPelicula)
        {
            objPelicula.UsuarioId = (int)_context.Usuarios.Where(p => p.Username.Equals(User.Identity.Name)).ToList().First().Id;
            if(ModelState.IsValid)
            {
                if(objPelicula.Trailer.Length>42){
                objPelicula.Trailer=ConvertirPelicula(objPelicula.Trailer);
                }
                _context.Add(objPelicula);
                _context.SaveChanges();
                return RedirectToAction("Catalogo");
            }else
            return View(objPelicula);
                
            
            /*
            TempData["Nombre de pelicula"]=objPelicula.Titulo;  
            TempData["Nombre del Director"]=objPelicula.Director;
            TempData["Año"]=objPelicula.Año;
            */
        }
        public string ConvertirPelicula(string urlpelicula){
            urlpelicula=urlpelicula.Substring(32,11);
            var peliculaurl="https://www.youtube.com/embed/" + urlpelicula;
            return peliculaurl;
        }

        public IActionResult EditarPelicula(int? id)
        {
            if(id == null){
                return NotFound();
            }
            var pelicula = _context.Peliculas.Find(id);
            if(pelicula == null){
                return NotFound();
            }
            return View(pelicula);
        }

        [HttpPost]
        public IActionResult EditarPelicula(int id, Pelicula objPelicula)
        {
            objPelicula.UsuarioId = (int)_context.Usuarios.Where(p => p.Username.Equals(User.Identity.Name)).ToList().First().Id;
            if (ModelState.IsValid)
            {
                if(objPelicula.Trailer.Length>42){
                    objPelicula.Trailer=ConvertirPelicula(objPelicula.Trailer);
                }
                _context.Update(objPelicula);
                _context.SaveChanges();
                return RedirectToAction("Catalogo");
            }
            return View();
        }
        
        public IActionResult BorrarPelicula(int? id)
        {
            var pelicula = _context.Peliculas.Find(id);
            _context.Peliculas.Remove(pelicula);
            _context.SaveChanges();
            return RedirectToAction(nameof(Catalogo));
        }
        public IActionResult Catalogo(){
            var listPelicula=_context.Peliculas.OrderBy(s=>s.ID) .ToList();
            return View(listPelicula);
        }
        public IActionResult VistaPelicula(int? id)
        {
            var pelicula = _context.Peliculas.Find(id);
            ViewBag.resenas = false;
            if (_context.Reseñas.Where(x => x.PeliculaId.Equals(id)).Count() > 0)
            {
                ViewBag.resenas = true;
                Random rnd = new Random();
                int nrouser = rnd.Next(_context.Reseñas.Where(x=>x.PeliculaId.Equals(id)).OrderBy(x=>x.Id).ToList().First().Id,_context.Reseñas.Where(x=>x.PeliculaId.Equals(id)).OrderBy(x => x.Id).ToList().Last().Id);
                ViewBag.fecha = _context.Reseñas.Find(nrouser).Fecha.ToString();
                ViewBag.estrellitas = _context.Reseñas.Where(x=>x.PeliculaId.Equals(id)).FirstOrDefault(x=>x.Id.Equals(nrouser)).Estrellas;
                ViewBag.nombreusuario = _context.Usuarios.Where(x => x.Id.Equals(_context.Reseñas.Find(nrouser).UsuarioId)).First().Username;
                ViewBag.resenausuario = _context.Reseñas.Where(x=>x.PeliculaId.Equals(id)).FirstOrDefault(x=>x.Id.Equals(nrouser)).Comentario;
                ViewBag.fecha = _context.Reseñas.Where(x=>x.PeliculaId.Equals(id)).FirstOrDefault(x=>x.Id.Equals(nrouser)).Fecha;
            }
            if(pelicula == null){
                return NotFound();
            }
            return View(pelicula);
        }
        [HttpPost]
        public IActionResult Catalogo(string idfiltro, string filtro)
        {
            var listClientes = _context.Peliculas.OrderBy(s => s.ID).ToList();
            if(idfiltro == "titulo"){
                listClientes=_context.Peliculas.Where(c => c.Titulo.ToUpper().Contains(filtro.ToUpper())).OrderBy(s=>s.ID) .ToList();
            }else if(idfiltro == "director"){
                listClientes=_context.Peliculas.Where(c => c.Director.ToUpper().Contains(filtro.ToUpper())).OrderBy(s=>s.ID) .ToList();
            }else{
                listClientes=_context.Peliculas.Where(c => c.Genero.ToUpper().Contains(filtro.ToUpper())).OrderBy(s=>s.ID) .ToList();
            }
            // var listClientes=_context.Clientes.Where(c => c.Nombre.ToUpper().Contains(filtro.ToUpper())).OrderBy(s=>s.Id) .ToList();
            return View(listClientes);
        }
        public IActionResult MisPeliculas()
        {
            var listapeliculas = _context.Peliculas.OrderBy(x => x.ID).Where(p => p.Usuario.Username.Equals(User.Identity.Name)).ToList();
            return View(listapeliculas);
        }
        [HttpPost]
        public IActionResult MisPeliculas(string idfiltro, string filtro)
        {
            var listapeliculas = _context.Peliculas.OrderBy(x => x.ID).Where(p => p.Usuario.Username.Equals(User.Identity.Name)).ToList();
            if(idfiltro == "titulo"){
                listapeliculas=_context.Peliculas.Where(c => c.Titulo.ToUpper().Contains(filtro.ToUpper())).OrderBy(s=>s.ID) .ToList();
            }else if(idfiltro == "director"){
                listapeliculas=_context.Peliculas.Where(c => c.Director.ToUpper().Contains(filtro.ToUpper())).OrderBy(s=>s.ID) .ToList();
            }else{
                listapeliculas=_context.Peliculas.Where(c => c.Genero.ToUpper().Contains(filtro.ToUpper())).OrderBy(s=>s.ID) .ToList();
            }
            return View(listapeliculas);
        }

        
    }
}
    /*
        public IActionResult Catalogo()
        {

            
            ViewData["Nombre de pelicula"]=TempData["Nombre de pelicula"];
            ViewData["Nombre del Director"]=TempData["Nombre del Director"];
            ViewData["Año"]=TempData["Año"];
            return View();
            
        }
*/


        /*
        public IActionResult GetPelicula()
        {
            var listCatalogo = new List<Pelicula>();
            listCatalogo.Add(
                new Pelicula(){ Titulo="Virus",Director="Aashiq Abu", Pais="Corea del Sur", Año=2013, Genero="Suspenso",Duracion=121, Clasificacion="R", Idioma="Coreano", Sinopsis="El distrito surcoreano de Bundang-gu debe enfrentar una repentina proliferación del virus H5N1, enfermedad que mata a sus víctimas tras 36 horas de alojarse en el organismo. El virus, transmitido por vía respiratoria, llega al distrito luego de que un grupo de inmigrantes ilegales llevarán la enfermedad hasta allí al ser transportados en un contenedor. Kang Ji-goo conocerá en medio del caos a la hermosa Kim In-hae, con la que deberá enfrentar esta terrible y mortal amenaza." });
            listCatalogo.Add(
                new Pelicula(){ Titulo="It Capítulo Dos",Director="	Andrés Muschietti", Pais="	Estados Unidos", Año=2019, Genero="Terror",Duracion=169, Clasificacion="+14", Idioma="Ingles", Sinopsis="En el misterioso pueblo de Derry, un malvado payaso llamado Pennywise vuelve 27 años después para atormentar a los ya adultos miembros del Club de los Perdedores, que ahora están más alejados unos de otros." });
            listCatalogo.Add(
                new Pelicula(){ Titulo="Train to Busan",Director="Yeon Sang-ho", Pais="Corea del Sur", Año=2016, Genero="Drama",Duracion=118, Clasificacion="R", Idioma="Coreano", Sinopsis="Un brote viral misterioso pone a Corea en estado de emergencia. Sok-woo y su hija Soo-ahn suben al KTX, un tren rápido que une los 442 km que separan Seúl de Busan, una ciudad que se defiende con éxito de la epidemia. Sin embargo, justo en el momento de su partida, la estación es invadida por zombis que matan al conductor del tren y a otros pasajeros." });
            return View("Catalogo",listCatalogo);
        }
        */
