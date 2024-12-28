namespace AMLibrary {
    public class LowPowerMachine : AmMachine {
        public override string  description => "Low Power Machine (200W)";
        public override decimal cost() {
            return 450000;
        }
    }
}