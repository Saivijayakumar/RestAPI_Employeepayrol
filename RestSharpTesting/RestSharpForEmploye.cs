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
    }
}
