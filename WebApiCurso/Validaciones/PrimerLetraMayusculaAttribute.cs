

using System.ComponentModel.DataAnnotations;

namespace WebApiCurso.Validaciones
{
    public class PrimerLetraMayusculaAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Para q no valide lo mismo 2 veces. ya que en la otra clase esta el [Required]
            // Esta validacion seria igual q required.
           if(value==null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primeraLetra = value.ToString()[0].ToString();

            if(primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayuscula");
            }

            return ValidationResult.Success;

        }
    }
}
