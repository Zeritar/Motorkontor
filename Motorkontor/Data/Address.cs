using System;
using System.Collections.Generic;

namespace Motorkontor.Data
{
    public class Address : IDetailModel
    {
        public int addressId { get; private set; }
        public string streetAndNo { get; set; }
        public DateTime createDate { get; set; }
        public virtual ZipCode zipCode { get; set; }

        public bool hasChanged { get; set; } = false;

        public bool deleted { get; set; } = false;

        public Address() : this(0)
        {

        }

        public Address(int _id)
        {
            addressId = _id;
        }

        public Dictionary<Field, string> GetFields()
        {
            Dictionary<Field, string> fields = new Dictionary<Field, string>();
            fields.Add(new Field("id", "Adresse ID"), addressId.ToString());
            fields.Add(new Field(nameof(streetAndNo), "Vejnavn og Husnummer"), Field.NullToEmpty(streetAndNo));
            fields.Add(new Field(nameof(createDate), "Dato Oprettet"), createDate.ToShortDateString());
            fields.Add(new Field(nameof(zipCode), "FK_Postnummer"), (zipCode != null) ? zipCode.zipCodeId.ToString() : "");

            return fields;
        }

        public int GetId()
        {
            return addressId;
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
            return "address";
        }

        public string GetFKName()
        {
            return "Adresse";
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
                    case nameof(streetAndNo):
                        streetAndNo = field.Value;
                        break;
                    case nameof(zipCode):
                        zipCode = new ZipCode(Convert.ToInt32(field.Value));
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
            return $"{streetAndNo} - {zipCode.zipCodeName} {zipCode.cityName}";
        }
    }
}
