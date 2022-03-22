using System.Collections.Generic;

namespace Motorkontor.Data
{
    public class Category : IDetailModel
    {
        public int categoryId { get; private set; }
        public string categoryName { get; set; }

        public bool hasChanged { get; set; } = false;

        public bool deleted { get; set; } = false;

        public Category() : this(0)
        {

        }

        public Category(int _id)
        {
            categoryId = _id;
        }

        public Dictionary<Field, string> GetFields()
        {
            Dictionary<Field, string> fields = new Dictionary<Field, string>();
            fields.Add(new Field("id", "Kategori ID"), categoryId.ToString());
            fields.Add(new Field(nameof(categoryName), "Kategori Navn"), Field.NullToEmpty(categoryName));

            return fields;
        }

        public int GetId()
        {
            return categoryId;
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
                    case nameof(categoryName):
                        categoryName = field.Value;
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
