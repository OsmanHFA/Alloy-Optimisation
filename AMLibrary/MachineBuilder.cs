namespace AMLibrary {
    // Builder class
    public class AmMachineBuilder {
        private AmMachine machine;

        public AmMachineBuilder(AmMachine baseMachine) {
            machine = baseMachine;
        }

        public AmMachineBuilder AddQuadLaser() {
            machine = new QuadLaserDecorator(machine);
            return this;
        }

        public AmMachineBuilder AddThermalImaging() {
            machine = new ThermalImagingCameraDecorator(machine);
            return this;
        }

        public AmMachineBuilder AddPhotodiodes() {
            machine = new PhotodiodesDecorator(machine);
            return this;
        }

        public AmMachineBuilder AddReducedBuildVolume() {
            machine = new ReducedBuildVolumeDecorator(machine);
            return this;
        }

        public AmMachineBuilder AddPowderRecirculation() {
            machine = new PowderRecirculationSystemDecorator(machine);
            return this;
        }

        public AmMachine Build() {
            return machine;
        }
    }
}