using SMSApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SMSApplication.Controllers
{
    public class CountriesController : ApiController
    {
        // GET api/countries
        [HttpGet]
        public HttpResponseMessage countries()
        {
            var listOfCountries = new List<Country>();
            using(DataClassesDataContext dbContext=new DataClassesDataContext())
            {
                //return all countries
                listOfCountries = dbContext.Country.ToList();
            }
           // return listOfCountries;
            return Request.CreateResponse(HttpStatusCode.OK, listOfCountries);
        }

    }
}
