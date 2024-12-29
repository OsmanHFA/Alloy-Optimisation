using Xunit;
using AMLibrary;

namespace AMLibrary.Tests {
    public class OptimiserTests {
        private readonly List<Element> _elements;
        private readonly AlloyOptimiser _optimiser;
        private readonly Alloy _bestAlloy;
        public OptimiserTests() {
        // Defining our input
        _elements = new List<Element>
        {
            new Element
            {
                Name = "Cr",
                Alpha = 2.0911350e16m,
                Cost = 14.0m,
                MinPercentage = 14.5m,
                MaxPercentage = 22.0m,
                StepSize = 0.5m
            },
            new Element
            {
                Name = "Co",
                Alpha = 7.2380280e16m,
                Cost = 80.5m,
                MinPercentage = 0.0m,
                MaxPercentage = 25.0m,
                StepSize = 1.0m
            },
            new Element
            {
                Name = "Nb",
                Alpha = 1.0352738e16m,
                Cost = 42.5m,
                MinPercentage = 0.0m,
                MaxPercentage = 1.5m,
                StepSize = 0.1m
            },
            new Element
            {
                Name = "Mo",
                Alpha = 8.9124547e16m,
                Cost = 16.0m,
                MinPercentage = 1.5m,
                MaxPercentage = 6.0m,
                StepSize = 0.5m
            },
            new Element
            {
                Name = "Ni",
                Alpha = 0.0m, // Ni doesn't contribute to creep
                Cost = 8.9m,
                MinPercentage = 0.0m,
                MaxPercentage = 100.0m,
                StepSize = 1.0m
            }
        };

        // 2) Instantiate optimiser and produce the "best" alloy
            _optimiser = new AlloyOptimiser(_elements, "Ni");
            _bestAlloy = _optimiser.FindOptimalAlloy(18);
        }

        // Unit tests 
        // Test 1 - Check input percentages are in range and step size > 0
        [Fact]
        public void Element_ShouldHaveValidRanges() {
            foreach (var element in _elements) {
                Assert.True(element.MinPercentage <= element.MaxPercentage,
                    $"{element.Name} has invalid range: Min={element.MinPercentage}, Max={element.MaxPercentage}");
                
                Assert.True(element.StepSize > 0,
                    $"{element.Name} must have a positive StepSize");
            }
        }
        // Test 2 - Check output
        [Fact]
        public void FindOptimalAlloy_cost_18() {
            // Test cost & creep output
            decimal bestCreep = _bestAlloy.CalculateCreepResistance();
            decimal finalCost = _bestAlloy.CalculateCost();

            // We expect something near 1.7299e18
            decimal expectedCreep = 1.7299e18m;
            // Solver will likely not be exact so we place some tolerance
            decimal tolerance = 1.0e15m; // Used ± 0.05% error
            Assert.InRange(bestCreep, expectedCreep - tolerance, expectedCreep + tolerance);

            // Also making sure we do not exceed the cost limit
            Assert.True(finalCost <= 18.0m, $"Alloy cost exceeded 18 £/kg. Actual: {finalCost}");
        }
        // Test 3 - Validating our output 
        [Fact]
        public void ValidateBestAlloyComposition()
        {
            // Retrieve final composition from the bestAlloy
            var composition = _bestAlloy.Composition;

            // Check if the sum of percentages is approximately 100%
            decimal total = composition.Sum(x => x.Value);
            Assert.InRange(total, 99.999m, 100.001m);

            // Check each element's percentage is within [MinPercentage, MaxPercentage]
            foreach (var kvp in composition)
            {
                Element element = kvp.Key;
                decimal percentage = kvp.Value;

                Assert.True(percentage >= element.MinPercentage,
                    $"Percentage for {element.Name} is below the min range (got {percentage}, min {element.MinPercentage}).");
                Assert.True(percentage <= element.MaxPercentage,
                    $"Percentage for {element.Name} is above the max range (got {percentage}, max {element.MaxPercentage}).");
            }
        }
    }
}