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
    
}