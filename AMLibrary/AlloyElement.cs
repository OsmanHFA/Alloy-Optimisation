using Google.OrTools.LinearSolver;

namespace AMLibrary {
    // Element class stores element properties
    public class Element {
        public required string Name {get; set;}
        public  decimal Alpha {get; set;}
        public decimal Cost {get; set;}
        public decimal MinPercentage {get; set;}
        public decimal MaxPercentage {get; set;}
        public decimal StepSize {get; set;}
    }
    
}