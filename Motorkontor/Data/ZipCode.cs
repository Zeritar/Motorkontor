using System.Collections.Generic;

namespace Motorkontor.Data
{
    public class ZipCode : IDetailModel
    {
        public int zipCodeId { get; private set; }
        public string zipCodeName { get; set; }
        public string cityName { get; set; }

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
            fields.Add(new Field(nameof(zipCodeId),"Postnummer ID"), zipCodeId.ToString());
            fields.Add(new Field(nameof(zipCodeName), "Postnummer"), Field.NullToEmpty(zipCodeName));
            fields.Add(new Field(nameof(cityName), "By"), Field.NullToEmpty(cityName));

            return fields;
        }

        public void UpdateFields(Dictionary<Field, string> fields)
        {

        }
    }
}
