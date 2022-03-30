using System;
using System.Collections.Generic;

namespace Motorkontor.Data
{
    public class Registration : IDetailModel
    {
        public int registrationId { get; private set; }
        public DateTime registrationDate { get; set; }

        private string firstRegistrationDate = "";

        public virtual Customer customer { get; set; }
        public virtual Vehicle vehicle { get; set; }

        public bool hasChanged { get; set; } = false;

        public bool deleted { get; set; } = false;

        public Registration() : this(0)
        {

        }
        
        public Registration(int _id)
        {
            registrationId = _id;
            registrationDate = DateTime.Now;
        }

        public Dictionary<Field, string> GetFields()
        {
            Dictionary<Field, string> fields = new Dictionary<Field, string>();
            fields.Add(new Field("id", "Registrering ID"), registrationId.ToString());
            fields.Add(new Field(nameof(registrationDate), "Registreringsdato"), registrationDate.ToShortDateString());
            fields.Add(new Field(nameof(customer), "FK_Kunde"), (customer != null) ? customer.customerID.ToString() : "");
            fields.Add(new Field(nameof(vehicle), "FK_Vehicle"), (vehicle != null) ? vehicle.vehicleId.ToString() : "");
            fields.Add(new Field(nameof(vehicle.firstRegistrationDate), "Første Registreringsdato"), (vehicle != null) ? vehicle.firstRegistrationDate.ToShortDateString() : DateTime.Now.ToShortDateString());
            return fields;
        }

        public int GetId()
        {
            return registrationId;
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
            return "registration";
        }

        public string GetFKName()
        {
            return "Registrering";
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
                    case nameof(registrationDate):
                        registrationDate = DateTime.Parse(field.Value);
                        break;
                    case nameof(vehicle):
                        vehicle = new Vehicle(field.Value != "" ? Convert.ToInt32(field.Value) : 0);
                        break;
                    case nameof(vehicle.firstRegistrationDate):
                        firstRegistrationDate = field.Value;
                        break;
                    case nameof(customer):
                        customer = new Customer(field.Value != "" ? Convert.ToInt32(field.Value) : 0);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Got unrecognized field: " + field.Key.Name);
                        break;
                }
            }
            vehicle.firstRegistrationDate = DateTime.Parse(firstRegistrationDate);
            hasChanged = true;
        }
    }
}
