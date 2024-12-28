using Xunit;
using AMLibrary;

namespace AMLibrary.Tests {
    public class OptimiserTests {
        [Fact]
        public void FindOptimalAlloy_cost_18() {
            var elements = new List<Element> {
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
                Alpha = 0.0m,     // since Ni doesn't contribute to creep
                Cost = 8.9m,
                MinPercentage = 0.0m,
                MaxPercentage = 100.0m,
                StepSize = 1.0m
            }
        };
        var optimiser = new AlloyOptimiser(elements, "Ni");

        var bestAlloy = optimiser.FindOptimalAlloy(18);

        decimal bestCreep = bestAlloy.CalculateCreepResistance();
        decimal finalCost = bestAlloy.CalculateCost();

        // We expect something near 1.7299e18 as solver will likely not be exact
        // So we need to add some tolerance - I used ± 0.05% error
        decimal expectedCreep = 1.7299e18m;
        decimal tolerance = 1.0e15m;
        Assert.InRange(bestCreep, expectedCreep - tolerance, expectedCreep + tolerance);

        // Also making sure we do not exceed the cost limit
        Assert.True(finalCost <= 18.0m, $"Alloy cost exceeded 18 £/kg. Actual: {finalCost}");
        }
    }
}