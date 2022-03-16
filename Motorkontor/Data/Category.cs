namespace Motorkontor.Data
{
    public class Category
    {
        public int categoryId { get; private set; }
        public string categoryName { get; set; }

        public Category() : this(0)
        {

        }

        public Category(int _id)
        {
            categoryId = _id;
        }
    }
}
