namespace AMLibrary {
    // Alloy class calculates creep resistance and cost
    public class Alloy {
        private readonly Dictionary<Element, decimal> _composition;
        private readonly string _baseElement;

        public Alloy(Dictionary<Element, decimal> composition, string baseElement) {
            _composition = composition;
            _baseElement = baseElement;
            // CheckComposition(); // Removed and changed to unit test
        }
        // Exposing composition so unit tests can see it
        public IReadOnlyDictionary<Element, decimal> Composition => _composition;
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