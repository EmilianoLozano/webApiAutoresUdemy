using Microsoft.AspNetCore.Identity;

namespace WebApiCurso.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }

        public int LibroId { get; set; }

        // propiedad de navegacion si quiero cargar los datos del libro relacionado relacion 1 a muchos con libro
        public Libro Libro { get; set; }

        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }

    }
}
