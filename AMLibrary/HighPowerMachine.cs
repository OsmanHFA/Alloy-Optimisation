namespace AMLibrary {
    public class HighPowerMachine : AmMachine {
        public override string description => "High Power Machine (500W)";
        public override decimal cost() {
            return 750000;
        }
    }
}