using System.Collections.Generic;

namespace Motorkontor.Data
{
    public class Fuel : IDetailModel
    {
        public int fuelId { get; private set; }
        public string fuelName { get; set; }

        public bool hasChanged { get; set; } = false;

        public bool deleted { get; set; } = false;

        public Fuel() : this(0)
        {

        }
        public Fuel(int _id)
        {
            fuelId = _id;
        }

        public Dictionary<Field, string> GetFields()
        {
            Dictionary<Field, string> fields = new Dictionary<Field, string>();
            fields.Add(new Field("id", "Brændstof ID"), fuelId.ToString());
            fields.Add(new Field(nameof(fuelName), "Brændstof Navn"), Field.NullToEmpty(fuelName));

            return fields;
        }

        public int GetId()
        {
            return fuelId;
        }

        public bool GetChanged()
        {
            return hasChanged;
        }

        public bool GetDeleted()
        {
            return deleted;
        }

        public string GetModelType()
        {
            return "fuel";
        }

        public string GetFKName()
        {
            return "Brændstof";
        }

        public void SetDeleted(bool deleted)
        {
            this.deleted = deleted;
        }

        public void UpdateFields(Dictionary<Field, string> fields)
        {
            foreach (var field in fields)
            {
                switch (field.Key.Name)
                {
                    case "id":
                        break;
                    case nameof(fuelName):
                        fuelName = field.Value;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Got unrecognized field: " + field.Key.Name);
                        break;
                }
            }
            hasChanged = true;
        }

        public override string ToString()
        {
            return $"{fuelName}";
        }
    }
}
