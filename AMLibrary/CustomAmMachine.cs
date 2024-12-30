namespace AMLibrary {
    public class CustomAmMachine : AmMachine {
        private readonly AmMachine baseMachine;
        private readonly AmMachineFeatures features;

        public CustomAmMachine (AmMachine baseMachine, AmMachineFeatures features) {
            this.baseMachine = baseMachine;
            this.features = features;
        }

        public override string description => 
            baseMachine.description + features.GetFeaturesDescription();

        public override decimal cost() =>
            baseMachine.cost() + features.CalculateFeaturesCost();
    }
}