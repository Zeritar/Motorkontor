using System.Collections.Generic;

namespace Motorkontor.Data
{
    public class ZipCode : IDetailModel
    {
        public int zipCodeId { get; private set; }
        public string zipCodeName { get; set; }
        public string cityName { get; set; }
        public bool hasChanged { get; set; } = false;

        public bool deleted { get; set; } = false;

        public ZipCode() : this(0)
        {

        }

        public ZipCode(int _id)
        {
            zipCodeId = _id;
        }

        public Dictionary<Field, string> GetFields()
        {
            Dictionary<Field, string> fields = new Dictionary<Field, string>();
            fields.Add(new Field("id","Postnummer ID"), zipCodeId.ToString());
            fields.Add(new Field(nameof(zipCodeName), "Postnummer"), Field.NullToEmpty(zipCodeName));
            fields.Add(new Field(nameof(cityName), "Bynavn"), Field.NullToEmpty(cityName));

            return fields;
        }

        public int GetId()
        {
            return zipCodeId;
        }

        public void SetDeleted(bool deleted)
        {
            this.deleted = deleted;
        }

        public void UpdateFields(Dictionary<Field, string> fields)
        {
            foreach (var field in fields)
            {
                switch(field.Key.Name)
                {
                    case "id":
                        break;
                    case nameof(zipCodeName):
                        zipCodeName = field.Value;
                        break;
                    case nameof(cityName):
                        cityName = field.Value;
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
