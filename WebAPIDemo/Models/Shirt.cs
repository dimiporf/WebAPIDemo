using System.ComponentModel.DataAnnotations;

namespace WebAPIDemo.Models
{
    // Model class representing a shirt
    public class Shirt
    {
        // ID of the shirt
        public int ShirtId { get; set; }

        // Brand of the shirt (required field)
        [Required(ErrorMessage = "Brand is required.")]
        public string? Brand { get; set; }

        // Added for versioning training
        public string? Description { get; set; }

        // Color of the shirt (required field)
        [Required(ErrorMessage = "Color is required.")]
        public string? Color { get; set; }

        // Size of the shirt (nullable)
        public int? Size { get; set; }

        // Gender for which the shirt is intended (required field)
        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }

        // Price of the shirt (nullable)
        public double? Price { get; set; }

        // Validates the description by checking if it is not null or empty (Added for versioning training)
        public bool ValidateDescription()
        {
            return !string.IsNullOrEmpty(Description);
        }
    }
}
