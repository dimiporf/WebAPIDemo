namespace WebAPIDemo.Models.Repositories
{
    // Repository class for managing shirts
    public static class ShirtRepository
    {
        // Static list of shirts as a mock data source
        private static List<Shirt> shirts = new List<Shirt>()
        {
            // Sample shirts data
            new Shirt { ShirtId = 1, Brand = "My Brand", Color = "Blue", Gender = "Men", Price = 30, Size = 10},
            new Shirt { ShirtId = 2, Brand = "My Brand1", Color = "Black", Gender = "Men", Price = 35, Size = 12},
            new Shirt { ShirtId = 3, Brand = "My Brand2", Color = "Pink", Gender = "Women", Price = 20, Size = 8},
            new Shirt { ShirtId = 4, Brand = "My Brand3", Color = "Yellow", Gender = "Women", Price = 28, Size = 9}
        };

        // Method to get all shirts
        public static List<Shirt> GetShirts()
        {
            return shirts;
        }

        // Method to check if a shirt with the given ID exists
        public static bool ShirtExists(int id)
        {
            return shirts.Any(x => x.ShirtId == id);
        }

        // Method to get a shirt by its ID
        public static Shirt? GetShirtById(int id)
        {
            return shirts.FirstOrDefault(x => x.ShirtId == id);
        }

        // Method to get a shirt by its properties (brand, gender, color, size)
        public static Shirt? GetShirtByProperties(string? brand, string? gender, string? color, int? size)
        {
            return shirts.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(brand) &&
                !string.IsNullOrWhiteSpace(x.Brand) &&
                x.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(gender) &&
                !string.IsNullOrWhiteSpace(x.Gender) &&
                x.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(color) &&
                !string.IsNullOrWhiteSpace(x.Color) &&
                x.Color.Equals(color, StringComparison.OrdinalIgnoreCase) &&
                size.HasValue &&
                x.Size.HasValue &&
                size.Value == x.Size.Value);
        }

        // Method to add a new shirt
        public static void AddShirt(Shirt shirt)
        {
            // Generate a new ID for the shirt
            int maxId = shirts.Max(x => x.ShirtId);
            shirt.ShirtId = maxId + 1;

            // Add the shirt to the list
            shirts.Add(shirt);
        }

        // Method to update an existing shirt
        public static void UpdateShirt(Shirt shirt)
        {
            // Find the shirt to update by its ID
            var shirtToUpdate = shirts.First(x => x.ShirtId == shirt.ShirtId);

            // Update shirt properties
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;
        }

        // Method to delete a shirt by its ID
        public static void DeleteShirt(int shirtId)
        {
            // Find the shirt to delete by its ID
            var shirt = GetShirtById(shirtId);

            // If the shirt exists, remove it from the list
            if (shirt != null)
            {
                shirts.Remove(shirt);
            }
        }
    }
}
