using ApiSdk.Models;

namespace PetStore.Tests
{
    public class AvailablePets
    {
        [Fact]
        public void Should_GroupAndSort_In_Correct_Order()
        {
            // Arrange: Create a sample list of pets
            List<Pet> pets = new()
            {
            new Pet { Name = "Bella", Category = new Category { Name = "Dogs" } },
            new Pet { Name = "Charlie", Category = new Category { Name = "Dogs" } },
            new Pet { Name = "Max", Category = new Category { Name = "Cats" } },
            new Pet { Name = "Luna", Category = new Category { Name = "Cats" } },
            new Pet { Name = "Oliver", Category = new Category { Name = "Cats" } },
            new Pet { Name = null, Category = new Category { Name = "Birds" } }, // Should be ignored
            new Pet { Name = "Rocky", Category = null } // Should be categorized as "Uncategorized"
        };

            // Act: Call the method
            Dictionary<string, List<Pet>> result = Helper.GroupAndSortPets(pets);

            // Assert: Check grouping
            Assert.Equal(3, result.Count); // "Cats", "Dogs", "Uncategorized"
            Assert.True(result.ContainsKey("Dogs"));
            Assert.True(result.ContainsKey("Cats"));
            Assert.True(result.ContainsKey("Uncategorized"));

            // Assert: Check sorting within each category
            Assert.Equal(new List<string> { "Oliver", "Max", "Luna" }, result["Cats"].Select(p => p.Name).ToList());
            Assert.Equal(new List<string> { "Charlie", "Bella" }, result["Dogs"].Select(p => p.Name).ToList());
            Assert.Equal(new List<string> { "Rocky" }, result["Uncategorized"].Select(p => p.Name).ToList());
        }
    }
}