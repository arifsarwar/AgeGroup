using WebAPI.BusinessRule;
using DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL;

namespace AgeAPI.Controllers
{
    public class PersonController : ApiController
    {
        static readonly IPersonRepository repository = new PersonRepository();

        //[AllowAnonymous]
        //[HttpGet]
        //  [ActionName("GetPersonByID")]
        public IHttpActionResult Get(int id)
        {
            Person item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Ok(item);
        }

        //[AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage PostPerson(Person p)
        {
            int status = repository.AddPerson(p);

            var response = Request.CreateResponse<Person>(HttpStatusCode.Created, p);
            string uri = "";

            if (status > -1)
                uri = Url.Link("DefaultApi", new { id = status });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        //[AllowAnonymous]
        //[HttpGet]
        public IEnumerable<Person> GetAllPersons(string Opt, string val)
        {
            DataTable DT = repository.GetAll(Opt, val);

            foreach (DataRow row in DT.Rows)
            {
                yield return new Person
                {
                    ID = Convert.ToInt32(row["ID"]),
                    FName = row["FirstName"].ToString(),
                    LName = row["LastName"].ToString(),
                    Age = Convert.ToInt32(row["Age"]),
                    AgeGroup = row["AgeGroup"].ToString()
                };
            }
            //  return Json(DataTableToJSONWithJSONNet(DT)) ;
            //DatatableToDictionary(dt, "ID"),); //DA.AddPerson(p);
        }

        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(table);
            return JSONString;
        }
    }
}
