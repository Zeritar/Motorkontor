using System;
using System.Collections.Generic;

namespace Motorkontor.Data
{
    public class Registration : IDetailModel
    {
        public int registrationId { get; private set; }
        public DateTime firstRegistrationDate { get; set; }

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
        }

        public Dictionary<Field, string> GetFields()
        {
            Dictionary<Field, string> fields = new Dictionary<Field, string>();
            fields.Add(new Field("id", "Registrering ID"), registrationId.ToString());
            fields.Add(new Field(nameof(firstRegistrationDate), "Første Registreringsdato"), firstRegistrationDate.ToShortDateString());
            fields.Add(new Field(nameof(customer), "FK_Kunde ID"), (customer != null) ? customer.customerID.ToString() : "");
            fields.Add(new Field(nameof(vehicle), "FK_Vehicle ID"), (vehicle != null) ? vehicle.vehicleId.ToString() : "");
            return fields;
        }

        public int GetId()
        {
            return registrationId;
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
                    case nameof(firstRegistrationDate):
                        firstRegistrationDate = DateTime.Parse(field.Value);
                        break;
                    case nameof(customer):
                        customer = new Customer(Convert.ToInt32(field.Value));
                        break;
                    case nameof(vehicle):
                        vehicle = new Vehicle(Convert.ToInt32(field.Value));
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
