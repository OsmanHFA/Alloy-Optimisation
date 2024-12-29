using Xunit;
using AMLibrary;

namespace AMLibrary.Tests {
    public class MachineTests {
        [Fact]
        public void LowPowerMachine_cost_check() {
            var machine = new LowPowerMachine();
            decimal cost = machine.cost();
            Assert.Equal(450000, cost);
        }

        [Fact]
        public void MediumPowerMachine_cost_check() {
            var machine = new MediumPowerMachine();
            decimal cost = machine.cost();
            Assert.Equal(550000, cost);
        }

        [Fact]
        public void HighPowerMachine_cost_check() {
            var machine = new HighPowerMachine();
            decimal cost = machine.cost();
            Assert.Equal(750000, cost);
        }

        [Fact]
        public static void TestMediumPowerMachineWithFeatures() {
            var builder = new AmMachineBuilder(new MediumPowerMachine());

            var machine = builder
                .AddReducedBuildVolume()
                .AddQuadLaser()
                .AddPowderRecirculation()
                .Build();
        
            decimal cost = machine.cost();
            Assert.Equal(932000, cost);
        }        
    }
}


