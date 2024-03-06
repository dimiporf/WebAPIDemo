using System.ComponentModel.DataAnnotations;
using WebApp.Models.Validations;

namespace WebApp.Models
{
    // Model class representing a shirt
    public class Shirt
    {
        // ID of the shirt
        public int ShirtId { get; set; }

        // Brand of the shirt (required field)
        [Required(ErrorMessage = "Brand is required.")]
        public string? Brand { get; set; }

        // Color of the shirt (required field)
        [Required(ErrorMessage = "Color is required.")]
        public string? Color { get; set; }

        // Size of the shirt (nullable)
        [Shirt_EnsureCorrectSizing]
        public int? Size { get; set; }

        // Gender for which the shirt is intended (required field)
        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }

        // Price of the shirt (nullable)
        public double? Price { get; set; }
    }
}
