using Google.OrTools.LinearSolver;

namespace AMLibrary {
    // Alloy Optimiser class is responsible for finding the optimal alloy composition
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