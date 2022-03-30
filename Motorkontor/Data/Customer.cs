using System;
using System.Collections.Generic;

namespace Motorkontor.Data
{
    public class Customer : IDetailModel
    {
        public int customerID { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public DateTime createDate { get; set; }

        public virtual Address address { get; set; }

        public bool hasChanged { get; set; } = false;

        public bool deleted { get; set; } = false;

        public Customer() : this(0)
        {

        }

        public Customer(int _id)
        {
            customerID = _id;
            createDate = DateTime.Now;
        }

        public Dictionary<Field, string> GetFields()
        {
            Dictionary<Field, string> fields = new Dictionary<Field, string>();
            fields.Add(new Field("id", "Kunde ID"), customerID.ToString());
            fields.Add(new Field(nameof(firstName), "Fornavn"), Field.NullToEmpty(firstName));
            fields.Add(new Field(nameof(lastName), "Efternavn"), Field.NullToEmpty(lastName));
            fields.Add(new Field(nameof(createDate), "Dato Oprettet"), createDate.ToShortDateString());
            fields.Add(new Field(nameof(address), "FK_Adresse"), (address != null) ? address.addressId.ToString() : "");
            return fields;
        }

        public int GetId()
        {
            return customerID;
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
            return "customer";
        }

        public string GetFKName()
        {
            return "Kunde";
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
                    case nameof(firstName):
                        firstName = field.Value;
                        break;
                    case nameof(lastName):
                        lastName = field.Value;
                        break;
                    case nameof(createDate):
                        break;
                    case nameof(address):
                        address = new Address(Convert.ToInt32(field.Value));
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
            return $"{firstName} {lastName}";
        }
    }
}
