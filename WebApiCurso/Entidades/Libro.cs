
using System.ComponentModel.DataAnnotations;
using WebApiCurso.Validaciones;

namespace WebApiCurso.Entidades
{
    public class Libro
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")]
        [PrimerLetraMayuscula]
        public string Titulo { get; set; }

        public DateTime? FechaPublicacion { get; set; }

        // propiedad de navegacion si quiero cargar los comentarios del libro relacion 1 a muchos con comentario
        public List<Comentario> Comentarios { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }



    }
}
