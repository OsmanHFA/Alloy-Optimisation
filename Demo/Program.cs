// Program to test some outputs

using AMLibrary;

public class Program {
    public static void Main() {
        // Task 1
        AmMachine machine = new LowPowerMachine();

        machine = new QuadLaserDecorator(machine);
        
        Console.WriteLine("Description: " + machine.description);
        Console.WriteLine("Cost: " + machine.cost());

        machine = new PhotodiodesDecorator(machine);

        if (machine is MachineDecorator topDecorator) {
            var features = topDecorator.GetFeatures();
            Console.WriteLine("Features: " + string.Join(", ", features));
        }
        
        // Task 2
        var elements = new List<Element>
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

        Console.WriteLine($"Max Creep Resistance: {bestCreep:e} m^2/s");
        Console.WriteLine($"Cost: {finalCost:F2} £/kg");    
        
    }
}
