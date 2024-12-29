namespace AMLibrary {
    // Builder class
     public class AmMachineBuilder {
        private readonly AmMachine baseMachine;
        private readonly AmMachineFeatures features;

        public AmMachineBuilder(AmMachine baseMachine) {
            this.baseMachine = baseMachine;
            this.features = new AmMachineFeatures();
        }

        public AmMachineBuilder AddQuadLaser() {
            features.HasQuadLaser = true;
            return this;
        }

        public AmMachineBuilder AddThermalImaging() {
            features.HasThermalImaging = true;
            return this;
        }

        public AmMachineBuilder AddPhotodiodes() {
            features.HasPhotodiodes = true;
            return this;
        }

        public AmMachineBuilder AddReducedBuildVolume() {
            features.HasReducedBuildVolume = true;
            return this;
        }

        public AmMachineBuilder AddPowderRecirculation() {
            features.HasPowderRecirculation = true;
            return this;
        }

        public CustomAmMachine Build() {
            return new CustomAmMachine(baseMachine, features);
        }
    }

    public class AmMachineFeatures
    {
        public bool HasQuadLaser { get; set; }
        public bool HasThermalImaging { get; set; }
        public bool HasPhotodiodes { get; set; }
        public bool HasReducedBuildVolume { get; set; }
        public bool HasPowderRecirculation { get; set; }
        
        public decimal CalculateFeaturesCost() {
            decimal cost = 0;
            if (HasQuadLaser) cost += 225000;
            if (HasThermalImaging) cost += 54000;
            if (HasPhotodiodes) cost += 63000;
            if (HasReducedBuildVolume) cost += 75000;
            if (HasPowderRecirculation) cost += 82000;
            return cost;
        }

        public string GetFeaturesDescription() {
            var features = new List<string>();
            if (HasQuadLaser) features.Add("Quad Laser");
            if (HasThermalImaging) features.Add("Thermal Imaging");
            if (HasPhotodiodes) features.Add("Photodiodes");
            if (HasReducedBuildVolume) features.Add("Reduced Build Volume");
            if (HasPowderRecirculation) features.Add("Powder Recirculation");
            
            if (features.Count > 0) {
                return " with " + string.Join(", ", features);
            }
            else { return "";}
        }
    }
}