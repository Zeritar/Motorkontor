using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Motorkontor.Data
{
    public class CategoryService : DbService
    {
        public Category[] GetCategories()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlDataReader reader = GetProcedure(connection, "usp_readCategories", null);

                while (reader.Read())
                {
                    categories.Add(new Category(Convert.ToInt32(reader["CategoryId"]))
                    {
                        categoryName = reader["CategoryName"].ToString()
                    });
                }
            }
            return categories.ToArray();
        }

        public Category GetCategoryById(int id)
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@categoryId", id)
                };

                SqlDataReader reader = GetProcedure(connection, "usp_readCategoryById", parameters);

                while (reader.Read())
                {
                    categories.Add(new Category(Convert.ToInt32(reader["CategoryId"]))
                    {
                        categoryName = reader["CategoryName"].ToString()
                    });
                }
            }
            if (categories.Count > 0)
                return categories[0];

            return null;
        }

        public bool PostCategory(Category category)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@categoryName", category.categoryName),
                };
                return PostProcedure(connection, "usp_postCategory", parameters);
            }
        }
    }
}
