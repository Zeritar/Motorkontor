namespace Motorkontor.Data
{
    public class Fuel
    {
        public int fuelId { get; private set; }
        public string fuelName { get; set; }

        public Fuel() : this(0)
        {

        }
        public Fuel(int _id)
        {
            fuelId = _id;
        }
    }
}
