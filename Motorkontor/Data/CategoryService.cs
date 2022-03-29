using System;
using System.Data;
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

        public int PostCategory(Category category)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@categoryName", category.categoryName),
                    new SqlParameter("@id", SqlDbType.Int)
                };
                parameters[parameters.Count - 1].Direction = ParameterDirection.Output;
                PostProcedure(connection, "usp_postCategory", parameters);
                return (int)parameters[parameters.Count - 1].Value;
            }
        }

        public bool UpdateCategory(Category category)
        {
            // No need to bother the database if the object didn't originate there. Newly created objects have an ID of 0
            if (category.categoryId < 1)
                return false;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@categoryId", category.categoryId),
                    new SqlParameter("@categoryName", category.categoryName)
                };
                return PostProcedure(connection, "usp_updateCategory", parameters);
            }
        }

        public bool DropCategory(Category category)
        {
            // No need to bother the database if the object didn't originate there. Newly created objects have an ID of 0
            if (category.categoryId < 1)
                return false;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@categoryId", category.categoryId)
                };
                return PostProcedure(connection, "usp_dropCategory", parameters);
            }
        }
    }
}
