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
            int id = 0;
            if (fkID != "")
                id = Convert.ToInt32(fkID);

            switch (fkField.ToLower())
            {
                case "customer":
                    Customer customer = customerService.GetCustomerById(id);
                    return id != 0 ? $"{customer.firstName} {customer.lastName}" : "Ny Kunde";
                case "vehicle":
                    Vehicle vehicle = vehicleService.GetVehicleById(id);
                    return id != 0 ? $"{vehicle.make} {vehicle.model}" : "Nyt Køretøj";
                case "address":
                    Address address = addressService.GetAddresseById(id);
                    return id != 0 ? $"{address.streetAndNo}, {address.zipCode.zipCodeName} {address.zipCode.cityName}" : "Ny Adresse";
                case "fuel":
                    Fuel fuel = fuelService.GetFuelById(id);
                    return id != 0 ? $"{fuel.fuelName}" : "Nyt Brændstof";
                case "zipcode":
                    ZipCode zipCode = zipCodeService.GetZipCodeById(id);
                    return id != 0 ? $"{zipCode.zipCodeName} {zipCode.cityName}" : "Nyt Postnummer";
                case "category":
                    Category category = categoryService.GetCategoryById(id);
                    return id != 0 ? $"{category.categoryName}" : "Ny Kategori";
                default:
                return "foreign key";
            }
        }

        public IDetailModel GetDetailModelFromForeignKey(string fkField, string fkID)
        {
            int id = 0;
            if (fkID != "")
                id = Convert.ToInt32(fkID);

            switch (fkField.ToLower())
            {
                case "customer":
                    return id != 0 ? customerService.GetCustomerById(id) : new Customer();
                case "vehicle":
                    return id != 0 ? vehicleService.GetVehicleById(id) : new Vehicle();
                case "address":
                    return id != 0 ? addressService.GetAddresseById(id) : new Address();
                case "fuel":
                    return id != 0 ? fuelService.GetFuelById(id) : new Fuel();
                case "zipcode":
                    return id != 0 ? zipCodeService.GetZipCodeById(id) : new ZipCode();
                case "category":
                    return id != 0 ? categoryService.GetCategoryById(id) : new Category();
                default:
                    return null;
            }
        }

        public IDetailModel CreateDetailModelFromForeignKey(string fkField)
        {
            return CreateDetailModelFromForeignKey(fkField, "");
        }

        public IDetailModel CreateDetailModelFromForeignKey(string fkField, string fkID)
        {
            int id = 0;
            if (fkID != "")
                id = Convert.ToInt32(fkID);

            switch (fkField.ToLower())
            {
                case "customer":
                    return new Customer(id);
                case "vehicle":
                    return new Vehicle(id);
                case "address":
                    return new Address(id);
                case "fuel":
                    return new Fuel(id);
                case "zipcode":
                    return new ZipCode(id);
                case "category":
                    return new Category(id);
                default:
                    return null;
            }
        }

        public int PostDetailModel(IDetailModel model)
        {
            switch (model.GetModelType())
            {
                case "customer":
                    return customerService.PostCustomer((Customer)model);
                case "vehicle":
                    return vehicleService.PostVehicle((Vehicle)model);
                case "address":
                    return addressService.PostAddress((Address)model);
                case "fuel":
                    return fuelService.PostFuel((Fuel)model);
                case "zipCode":
                    return zipCodeService.PostZipCode((ZipCode)model);
                case "category":
                    return categoryService.PostCategory((Category)model);
                default:
                    return 0;
            }
        }

        public bool UpdateDetailModel(IDetailModel model)
        {
            switch (model.GetModelType())
            {
                case "customer":
                    return customerService.UpdateCustomer((Customer)model);
                case "vehicle":
                    return vehicleService.UpdateVehicle((Vehicle)model);
                case "address":
                    return addressService.UpdateAddress((Address)model);
                case "fuel":
                    return fuelService.UpdateFuel((Fuel)model);
                case "zipcode":
                    return zipCodeService.UpdateZipCode((ZipCode)model);
                case "category":
                    return categoryService.UpdateCategory((Category)model);
                default:
                    return false;
            }
        }

        public bool DropDetailModel(IDetailModel model)
        {
            switch (model.GetModelType())
            {
                case "customer":
                    return customerService.DropCustomer((Customer)model);
                case "vehicle":
                    return vehicleService.DropVehicle((Vehicle)model);
                case "address":
                    return addressService.DropAddress((Address)model);
                case "fuel":
                    return fuelService.DropFuel((Fuel)model);
                case "zipcode":
                    return zipCodeService.DropZipCode((ZipCode)model);
                case "category":
                    return categoryService.DropCategory((Category)model);
                default:
                    return false;
            }
        }
    }
}
