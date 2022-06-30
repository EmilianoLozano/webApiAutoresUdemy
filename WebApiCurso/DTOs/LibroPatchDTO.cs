using System.ComponentModel.DataAnnotations;
using WebApiCurso.Validaciones;

namespace WebApiCurso.DTOs
{
    public class LibroPatchDTO
    {

        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")]
        [PrimerLetraMayuscula]
        public string Titulo { get; set; }

        public DateTime? FechaPublicacion { get; set; }

    }
}
