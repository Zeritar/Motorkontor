using System;
using System.Collections.Generic;

namespace Motorkontor.Data
{
    public class Vehicle : IDetailModel
    {
        public int vehicleId { get; private set; }
        public string make { get; set; }
        public string model { get; set; }
        public DateTime createDate { get; set; }

        public virtual Category category { get; set; }
        public virtual Fuel fuel { get; set; }

        public bool hasChanged { get; set; } = false;

        public bool deleted { get; set; } = false;

        public Vehicle() : this(0)
        {

        }

        public Vehicle(int _id)
        {
            vehicleId = _id;
        }

        public Dictionary<Field, string> GetFields()
        {
            Dictionary<Field, string> fields = new Dictionary<Field, string>();
            fields.Add(new Field("id", "Køretøj ID"), vehicleId.ToString());
            fields.Add(new Field(nameof(make), "Fabrikant"), Field.NullToEmpty(make));
            fields.Add(new Field(nameof(model), "Model"), Field.NullToEmpty(model));
            fields.Add(new Field(nameof(createDate), "Dato Oprettet"), createDate.ToShortDateString());
            fields.Add(new Field(nameof(category), "FK_Kategori ID"), (category != null) ? category.categoryId.ToString() : "");
            fields.Add(new Field(nameof(fuel), "FK_Brændstof ID"),  (fuel != null) ? fuel.fuelId.ToString() : "");
            return fields;
        }

        public int GetId()
        {
            return vehicleId;
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
                    case nameof(make):
                        make = field.Value;
                        break;
                    case nameof(model):
                        model = field.Value;
                        break;
                    case nameof(createDate):
                        break;
                    case nameof(category):
                        category = new Category(Convert.ToInt32(field.Value));
                        break;
                    case nameof(fuel):
                        fuel = new Fuel(Convert.ToInt32(field.Value));
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Got unrecognized field: " + field.Key.Name);
                        break;
                }
            }
            hasChanged = true;
        }
    }
}
