using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Motorkontor.Data
{
    public class VehicleService : DbService
    {
        private CategoryService categoryService;
        private FuelService fuelService;

        public VehicleService(CategoryService _categoryService, FuelService _fuelService)
        {
            categoryService = _categoryService;
            fuelService = _fuelService;
        }

        public Vehicle[] GetVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlDataReader reader = GetProcedure(connection, "usp_readVehicles", null);

                while (reader.Read())
                {
                    vehicles.Add(new Vehicle(Convert.ToInt32(reader["VehicleId"]))
                    {
                        make = reader["Make"].ToString(),
                        model = reader["Model"].ToString(),
                        createDate = (DateTime)reader["CreateDate"],
                        category = categoryService.GetCategoryById(Convert.ToInt32(reader["FK_CategoryId"].ToString())),
                        fuel = fuelService.GetFuelById(Convert.ToInt32(reader["FK_FuelId"].ToString()))
                    });
                }
            }
            return vehicles.ToArray();
        }

        public Vehicle GetVehicleById(int id)
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@vehicleId", id)
                };

                SqlDataReader reader = GetProcedure(connection, "usp_readVehicleById", parameters);

                while (reader.Read())
                {
                    vehicles.Add(new Vehicle(Convert.ToInt32(reader["VehicleId"]))
                    {
                        make = reader["Make"].ToString(),
                        model = reader["Model"].ToString(),
                        createDate = (DateTime)reader["CreateDate"],
                        category = categoryService.GetCategoryById(Convert.ToInt32(reader["FK_CategoryId"].ToString())),
                        fuel = fuelService.GetFuelById(Convert.ToInt32(reader["FK_FuelId"].ToString()))
                    });
                }
            }
            if (vehicles.Count > 0)
                return vehicles[0];

            return null;
        }

        public bool PostVehicle(Vehicle vehicle)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@make", vehicle.make),
                    new SqlParameter("@model", vehicle.model),
                    new SqlParameter("@categoryId", vehicle.category.categoryId),
                    new SqlParameter("@fuelId", vehicle.fuel.fuelId)
                };
                return PostProcedure(connection, "usp_postVehicle", parameters);
            }
        }

        public bool UpdateVehicle(Vehicle vehicle)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@vehicleId", vehicle.vehicleId),
                    new SqlParameter("@make", vehicle.make),
                    new SqlParameter("@model", vehicle.model),
                    new SqlParameter("@categoryId", vehicle.category.categoryId),
                    new SqlParameter("@fuelId", vehicle.fuel.fuelId)
                };
                return PostProcedure(connection, "usp_updateVehicle", parameters);
            }
        }

        public bool DropVehicle(Vehicle vehicle)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@vehicleId", vehicle.vehicleId)
                };
                return PostProcedure(connection, "usp_dropVehicle", parameters);
            }
        }
    }
}
