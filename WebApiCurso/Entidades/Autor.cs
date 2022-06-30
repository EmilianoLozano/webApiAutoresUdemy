using System.ComponentModel.DataAnnotations;
using WebApiCurso.Validaciones;

namespace WebApiCurso.Entidades

{
    public class Autor 
    {
        public int Id { get; set; }

        // VALIDACIONES POR DEFECTO

        [Required(ErrorMessage = "El campo {0} del autor es requerido")]
        [StringLength(maximumLength:120,ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")]
        [PrimerLetraMayuscula]
        public string Nombre { get; set; }

        public List<AutorLibro> AutoresLibros { get; set; }

    }
}
