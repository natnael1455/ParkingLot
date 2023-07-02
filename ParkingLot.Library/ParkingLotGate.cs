using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLot.Library;

public class ParkingLotGate : IParkingLotGate
{
    private bool entry;
    public ParkingLotGate(bool entry = false)
    {
        this.entry = entry;
    }

    public void RunCycle(IParkingLotGateInputs inputs, IParkingLotGateOutputs outputs)

    {
        if (inputs.EStop || inputs.SafetySensor)
        {
            outputs.OpenGate = false;
            outputs.CloseGate = false;
            outputs.YellowLight = true;
        }

        else if (IsvalidDriver(inputs.DriverId) && inputs.InductiveSensor && !IsvalidLicensePlate(inputs.LicensePlate) && !inputs.GateFullyOpen)
        {
            outputs.OpenGate = true;
            outputs.GreenLight = true;
        }
        
        else if (!inputs.GateFullyClosed)
        {
            outputs.CloseGate = true;
            outputs.RedLight = true;
        }

    }

    private bool IsvalidDriver(string driverID)
    {
        // this function checks validity of by checking in the array assuming it is geting from database
        List<string> Driverids = new List<string> { "123", "456", "789" };
        if (Driverids.Contains(driverID) || !entry)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsvalidLicensePlate(string licensePlate)
    {
        // avalible cars in the lot
        List<string> licenses = new List<string> { "123", "456", "789" };
        if (licenses.Contains(licensePlate) && entry)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}
