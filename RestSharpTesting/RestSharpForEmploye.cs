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
            //Check by count 
            Assert.AreEqual(6, jsonObject.Count);
            //Check by status code 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
