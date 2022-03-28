using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Motorkontor.Data
{
    public class ForeignKeyService
    {
        private FuelService fuelService;
        private VehicleService vehicleService;
        private ZipCodeService zipCodeService;
        private CustomerService customerService;
        private AddressService addressService;
        private CategoryService categoryService;

        public ForeignKeyService(FuelService _fuelService, VehicleService _vehicleService, ZipCodeService _zipCodeService, CustomerService _customerService, AddressService _addressService, CategoryService _categoryService)
        {
            fuelService = _fuelService;
            vehicleService = _vehicleService;
            zipCodeService = _zipCodeService;
            customerService = _customerService;
            addressService = _addressService;
            categoryService = _categoryService;
        }

        public string GetFieldFromForeignKey(string fkField, string fkID)
        {
            int id = Convert.ToInt32(fkID);
            switch (fkField.ToLower())
            {
                case "customer":
                    Customer customer = customerService.GetCustomerById(id);
                    return $"{customer.firstName} {customer.lastName}";
                case "vehicle":
                    Vehicle vehicle = vehicleService.GetVehicleById(id);
                    return $"{vehicle.make} {vehicle.model}";
                case "address":
                    Address address = addressService.GetAddresseById(id);
                    return $"{address.streetAndNo}, {address.zipCode.zipCodeName} {address.zipCode.cityName}";
                case "fuel":
                    Fuel fuel = fuelService.GetFuelById(id);
                    return $"{fuel.fuelName}";
                case "zipcode":
                    ZipCode zipCode = zipCodeService.GetZipCodeById(id);
                    return $"{zipCode.zipCodeName} {zipCode.cityName}";
                case "category":
                    Category category = categoryService.GetCategoryById(id);
                    return $"{category.categoryName}";
                default:
                return "foreign key";
            }
        }
    }
}
