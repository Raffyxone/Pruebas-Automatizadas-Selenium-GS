using System.ComponentModel.DataAnnotations;

namespace GymSystem.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El Apellido es obligatorio")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "La Edad es obligatoria")]
        [Range(0, 120, ErrorMessage = "La edad debe estar entre 0 y 120")]
        public int Age { get; set; }

        [Required]
        public string Plan { get; set; } 
    }
}