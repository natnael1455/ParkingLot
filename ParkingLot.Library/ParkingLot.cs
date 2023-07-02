using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLot.Library
{
    public class ParkingLot : IParkingLot
    {
        // this list is the db that will be used in driver validation
        private List<string> driverIds = new List<string>();
        public IParkingLotGate InitEntryGate() => new ParkingLotGate(true);
        public IParkingLotGate InitExitGate() => new ParkingLotGate();

        public void AddAuthorizedDriver(string driverId)
        {
            if (!driverIds.Contains(driverId))
            {
                driverIds.Add(driverId);
                Console.WriteLine($"driver id {driverId} is added");
            }
            else
            {
                Console.WriteLine($"driver id {driverId} already exists");
            }
        }

        public void RemoveAuthorizedDriver(string driverId)
        {
            if (driverIds.Contains(driverId))
            {
                driverIds.Remove(driverId);
                Console.WriteLine($"driver id {driverId} is removed");
            }
            else
            {
                Console.WriteLine($"driver id {driverId} already exists");
            }
        }
    }
}
