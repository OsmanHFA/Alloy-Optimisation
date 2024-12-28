using Google.OrTools.LinearSolver;

namespace AMLibrary {
    // Element class stores element properties
    public class Element {
        public string Name {get; set;}
        public  decimal Alpha {get; set;}
        public decimal Cost {get; set;}
        public decimal MinPercentage {get; set;}
        public decimal MaxPercentage {get; set;}
        public decimal StepSize {get; set;}
    }
    
    // Alloy class represents alloy composition and their properties
    public class Alloy {
        private readonly Dictionary<Element, decimal> _composition;
        private readonly string _baseElement;

        public Alloy(Dictionary<Element, decimal> composition, string baseElement) {
            _composition = composition;
            _baseElement = baseElement;
            CheckComposition(); // Function to check if a compoisition is valid
        }
        private void CheckComposition() {
            // Check if percentages add up to 100. Edit: added 0.001 as margin of error
            if (Math.Abs(_composition.Sum(x => x.Value) - 100) > 0.001m) {
                throw new ArgumentException("Element percentages must sum to 100%.");
            }
            // Check if percentages are within the Min/Max range
            foreach(var (element, percentage) in _composition) {
                if (percentage < element.MinPercentage || percentage > element.MaxPercentage) {
                    throw new ArgumentException($"Percentage for {element.Name} is outside the allowed range.");
                }
            }
        }
        public decimal CalculateCreepResistance() {
            return _composition
                               .Where(x => x.Key.Name != _baseElement) // Filtering the base element. Edit: CHECK THIS
                               .Sum(x => x.Key.Alpha * x.Value); // Sum of (alpha_i * x_i) for each element
        }
        public decimal CalculateCost() {
            return  _composition.Sum(x => x.Key.Cost * x.Value / 100);
        }
    }

    // Alloy Optimiser is responsible for finding the optimal alloy composition
    public class AlloyOptimiser {
        private readonly List<Element> _elements;
        private readonly Element _baseElement;
        public AlloyOptimiser(List<Element> elements, string baseElement) {
            _elements = elements;
            _baseElement = elements.First(e => e.Name == baseElement);
        }
    
        public Alloy FindOptimalAlloy(decimal maxCost) {
            // Creating our LP Solver
            Solver solver = Solver.CreateSolver("SCIP");

            var elementVariables = new Dictionary<Element, Variable>();
            
            double sumOfMinPerc = 0.0;
            double sumOfMinCost = 0.0;

            foreach (var element in _elements) {
                // The possible number of steps for each element - ( (Max-Min) / StepSize )
                int stepCounts = (int)((element.MaxPercentage - element.MinPercentage) / element.StepSize);
                
                Variable varSteps = solver.MakeIntVar(0.0, stepCounts, element.Name);

                elementVariables[element] = varSteps;

                sumOfMinPerc += (double)element.MinPercentage; 
                sumOfMinCost += (double)(element.Cost * element.MinPercentage); 

            }

            // Objective: Maximise creep resistance
            Objective objective = solver.Objective();
            foreach (var element in _elements.Where(e => e != _baseElement)) {
                objective.SetCoefficient(
                    elementVariables[element],
                    (double)(element.Alpha * element.StepSize)
                );
            }
            objective.SetMaximization();

            // Constraint 1: Sum of percentages = 100
            double lowerBound = 99.99 - sumOfMinPerc;
            double upperBound = 100.01 - sumOfMinPerc;

            Constraint totalPercentage = solver.MakeConstraint(lowerBound, upperBound, "total_percentage");

            foreach (var (element, varSteps) in elementVariables) {
                totalPercentage.SetCoefficient(varSteps, (double)element.StepSize);
            }
            
            // Constraint 2: Cost constraint: cost <= maxCost
            double costUpperBound = (double)(maxCost * 100) - sumOfMinCost;

            Constraint costConstraint = solver.MakeConstraint(double.NegativeInfinity, costUpperBound, "cost_constraint");

            foreach (var (element, varSteps) in elementVariables) {
                costConstraint.SetCoefficient(varSteps, (double)(element.Cost * element.StepSize));
            }
            // Solve
            Solver.ResultStatus resultStatus = solver.Solve();
            
            if (resultStatus != Solver.ResultStatus.OPTIMAL) {
                throw new InvalidOperationException(
                    $"Could not find optimal solution. Status: {resultStatus}");
            }
            
            // Convert solution to alloy composition
            var composition = new Dictionary<Element, decimal>();
            
            foreach (var (element, variable) in elementVariables) {
                decimal steps = (decimal)Math.Round(variable.SolutionValue());
                decimal percentage = element.MinPercentage + (steps * element.StepSize);
                composition[element] = percentage;
            }
            
            return new Alloy(composition, _baseElement.Name);

        }
    }

    
}