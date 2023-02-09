using System;

namespace MovieAppV2.Models
{
    public class Noticia
    {
        public int ID { get; set; }

        public string Titulo { get; set; }

        public string Genero { get; set; }

        public DateTime Fecha { get; set; }

        public string contenido { get; set; }

        public string Imagen { get; set; }
        public Usuario Usuario { get; set; }
        public int UsuarioId { get; set; }
    }
}