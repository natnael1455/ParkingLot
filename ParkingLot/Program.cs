using System;

namespace ParkingLot
{
    internal class Program
    {
        static IParkingLot _parkingLot;
        static IParkingLotGate _entryGate;
        static IParkingLotGate _exitGate;

        // We can use inputs as our simulation state
        static GateInputs _entryInputs;
        static GateInputs _exitInputs;

        static Program()
        {
            _parkingLot = new Library.ParkingLot();
            _entryGate = _parkingLot.InitEntryGate();
            _exitGate = _parkingLot.InitExitGate();

            _entryInputs = new GateInputs();
            _exitInputs = new GateInputs();
        }
        
        static void Main(string[] args)
        {
            // Define a driver and a car
            string driver1Id = "123";
            string car1LicensePlate = "1245";

            // Add driver
            Console.WriteLine($"Adding driver {driver1Id}");
            _parkingLot.AddAuthorizedDriver(driver1Id);

            // Add car
            _entryInputs.LicensePlate = car1LicensePlate;
            _entryInputs.DriverId = driver1Id;
            Console.WriteLine($"Driving car {_entryInputs.LicensePlate} in");

            // Drive in
            SimulateCarDrivingThroughGate(_entryGate, _entryInputs);

            // Drive out
            _exitInputs.LicensePlate = car1LicensePlate;
            Console.WriteLine($"Driving car {_exitInputs.LicensePlate} out");

            // Drive out
            SimulateCarDrivingThroughGate(_exitGate, _exitInputs);
        }

        static void SimulateCarDrivingThroughGate(IParkingLotGate gate, GateInputs inputs)
        {
            // Car first triggers the inductive sensor
            inputs.InductiveSensor = true;

            // Gate should open, wait for it
            GateOutputs outputs;
            while (true)
            {
                outputs = SimulateCycle(gate, inputs);

                if (outputs.GreenLight)
                {
                    break;
                }

                Console.WriteLine($"{gate}: Waiting for green light");
            }

            Console.WriteLine($"{gate}: Green light, driving through gate");

            // Start driving in, safety sensor is triggered first
            inputs.SafetySensor = true;

            SimulateCycle(gate, inputs);

            // As car continues it exits the inductive sensor area and the readers are cleared
            inputs.InductiveSensor = false;
            inputs.LicensePlate = null;
            inputs.DriverId = null;

            SimulateCycle(gate, inputs);

            // Once fully through the safety sensor triggers off as well
            inputs.SafetySensor = false;

            SimulateCycle(gate, inputs);

            Console.WriteLine($"{gate}: Drove through gate");

            // Gate should be closed automatically after fully through
            while (!inputs.GateFullyClosed)
            {
                Console.WriteLine($"{gate}: Waiting for gate to be closed");

                outputs = SimulateCycle(gate, inputs);
            }
        }

        static GateOutputs SimulateCycle(IParkingLotGate gate, GateInputs inputs)
        {
            GateOutputs outputs = new GateOutputs();

            gate.RunCycle(inputs, outputs);
            
            // Handle opening the gate
            if (outputs.OpenGate)
            {
                if (inputs.GateFullyClosed)
                {
                    Console.WriteLine($"{gate}: Opening");
                    inputs.GateFullyClosed = false;
                    inputs.GateFullyOpen = false;
                }
                else
                {
                    Console.WriteLine($"{gate}: Fully open");
                    inputs.GateFullyOpen = true;
                }
            }

            // Handle closing the gate
            if (outputs.CloseGate)
            {
                if (inputs.GateFullyOpen)
                {
                    Console.WriteLine($"{gate}: Closing");
                    inputs.GateFullyClosed = false;
                    inputs.GateFullyOpen = false;
                }
                else
                {
                    Console.WriteLine($"{gate}: Fully closed");
                    inputs.GateFullyClosed = true;
                }
            }

            return outputs;
        }
    }
}