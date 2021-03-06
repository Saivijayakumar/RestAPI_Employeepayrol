using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace RestSharpTesting
{
    [TestClass]
    public class RestSharpForEmploye
    {
        //seting the restclient as null
        RestClient client = null;
        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient("http://localhost:3000");
        }
        public IRestResponse GetAllEmployees()
        {
            //Define method Type
            RestRequest request = new RestRequest("/employees", Method.GET);
            //Eexcute request
            IRestResponse response = client.Execute(request);
            //Return the response
            return response;
        }
        //UC 1: Getting all the employee details from json server
        [TestMethod]
        public void OnCallingGetMethodWeAreReturningEmployeesData()
        {
            IRestResponse response = GetAllEmployees();
            //Deserialize json object to List
            var jsonObject = JsonConvert.DeserializeObject<List<EmployeeModel>>(response.Content);
            foreach (var element in jsonObject)
            {
                Console.WriteLine($"Id: {element.id} || Name: {element.first_name+" "+element.last_name} || Salary :{element.salary}");
            }
            //Check by count 
            Assert.AreEqual(6, jsonObject.Count);
            //Check by status code 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        //UC 2:Add employee to server
        [TestMethod]
        public void OnCallingPOSTMethodWeAreReturningEmployeesCount()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JsonObject jsonObject = new JsonObject();
            jsonObject.Add("first_name", "Ram");
            jsonObject.Add("last_name", "Kumar");
            jsonObject.Add("salary", 56430);

            //adding parameter to request and in parameters we have content type ,object,parameter type
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<EmployeeModel>(response.Content);
            Console.WriteLine($"Id: {result.id} || Name: {result.first_name + " " + result.last_name} || Salary :{result.salary}");
            Assert.AreEqual("Ram Kumar", result.first_name + " " + result.last_name);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
        //UC 3:Add Multiple Employees to json server
        public IRestResponse AddingInJsonServer(JsonObject jsonObject)
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void UseingPostMethodToAddMultipleEmployees()
        {
            //Create Json object for employee1
            JsonObject employee1 = new JsonObject();
            employee1.Add("first_name", "Ravi");
            employee1.Add("last_name", "Kiran");
            employee1.Add("salary", 6430);
            //Call Function to Add and change that result to status code
            HttpStatusCode response1 = AddingInJsonServer(employee1).StatusCode;

            //Create Json object for employee2
            JsonObject employee2 = new JsonObject();
            employee2.Add("first_name", "Ramya");
            employee2.Add("last_name", "K");
            employee2.Add("salary", 98430);
            //Call Function to Add and change that result to status code
            HttpStatusCode response2 = AddingInJsonServer(employee2).StatusCode;

            Assert.AreEqual(response1, HttpStatusCode.Created);
            Assert.AreEqual(response2, HttpStatusCode.Created);
        }
        //UC 4:Update Values in json server useing id
        [TestMethod]
        public void UseingPUTMethodToUpdateEmployeesData()
        {
            RestRequest request = new RestRequest("/employees/8", Method.PUT);
            JsonObject json = new JsonObject();
            json.Add("first_name", "Pavani");
            json.Add("last_name", "D");
            json.Add("salary", 90000);
            //directly adding json object to request
            request.AddJsonBody(json);
            IRestResponse response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<EmployeeModel>(response.Content);
            Console.WriteLine($"Id: {result.id} || Name: {result.first_name + " " + result.last_name} || Salary :{result.salary}");
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
        // Usecase 5: Delete the employee details using the id
        [TestMethod]
        public void UseingDELETEMethodToDeleteEmployeesData()
        {
            RestRequest request = new RestRequest("/employees/6", Method.DELETE);
            IRestResponse response = client.Execute(request);
            //checking the data after delete
            IRestResponse getresponse = GetAllEmployees();
            var result = JsonConvert.DeserializeObject<List<EmployeeModel>>(getresponse.Content);
            foreach (var element in result)
            {
                Console.WriteLine($"Id: {element.id} || Name: {element.first_name + " " + element.last_name} || Salary :{element.salary}");
            }
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
