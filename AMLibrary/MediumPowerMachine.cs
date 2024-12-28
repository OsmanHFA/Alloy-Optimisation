namespace AMLibrary {
    public class MediumPowerMachine : AmMachine {
        public override string description => "Medium Power Machine (400W)";
        public override decimal cost() {
            return 550000;
        }
    }
}