using System.ComponentModel.DataAnnotations;

namespace WebApiCurso.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
