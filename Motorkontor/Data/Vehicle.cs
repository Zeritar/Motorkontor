using System;

namespace Motorkontor.Data
{
    public class Vehicle
    {
        public int vehicleId { get; private set; }
        public string make { get; set; }
        public string model { get; set; }
        public DateTime createDate { get; set; }

        public virtual Category category { get; set; }
        public virtual Fuel fuel { get; set; }

        public Vehicle()
        {

        }

        public Vehicle(int _id)
        {
            vehicleId = _id;
        }
    }
}
