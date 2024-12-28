// Implementing the Decorator pattern to add new features

using AMLibrary;

namespace AMLibrary {
    // Decorator base class
    public abstract class MachineDecorator : AmMachine {
        protected readonly AmMachine _machine;

        protected MachineDecorator(AmMachine machine) {
            _machine = machine;
        }

        public override decimal cost() {
            return _machine.cost();
        }

        public override string description => _machine.description;
    }

    public class QuadLaserDecorator : MachineDecorator {
        public QuadLaserDecorator(AmMachine machine) : base(machine) {}

        public override decimal cost() {
            return base.cost() + 225000;
        }

        public override string description {
            get {
                return base.description + " + Quad Laser";
            }
        }
    }

    public class ThermalImagingCameraDecorator : MachineDecorator {
        public ThermalImagingCameraDecorator(AmMachine machine) : base(machine) {}

        public override decimal cost() {
            return base.cost() + 54000;
        }
        public override string description {
            get {
                return base.description + " + Thermal Imaging Camera";
            }
        }
    }

    public class PhotodiodesDecorator : MachineDecorator {
        public PhotodiodesDecorator(AmMachine machine) : base(machine) {}

        public override decimal cost() {
            return base.cost() + 63000;
        }

        public override string description {
            get {
                return base.description + " + Photodiodes";
            }
        }
    }

    public class ReducedBuildVolumeDecorator : MachineDecorator {
        public ReducedBuildVolumeDecorator(AmMachine machine) : base(machine) {}

        public override decimal cost() {
            return base.cost() + 75000;
        }
        public override string description {
            get {
                return base.description + " + Reduced Build Volume";
            }
        }
    }

    public class PowderRecirculationSystemDecorator : MachineDecorator {
        public PowderRecirculationSystemDecorator(AmMachine machine) : base(machine) {}

        public override decimal cost() {
            return base.cost() + 82000;
        }

        public override string description {
            get {
                return base.description + " + Powder Recirculation System";
            }
        }
    }

}