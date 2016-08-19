using SMSApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SMSApplication.Controllers
{
    public class StatisticsController : ApiController
    {
        // GET api/statistics
        [HttpGet]
        public HttpResponseMessage statistics(DateTime dateFrom, DateTime dateTo, string mccList)
        {
            //List<string> mccList = new List<string> {"262","232" };
            List<Statistics> listOfStatistics = new List<Statistics>();
            using (DataClassesDataContext dbContext = new DataClassesDataContext())
            {

                if (mccList == null ||mccList == "null") 
                {
                    //return all records is mccList is empty or null
                    listOfStatistics = dbContext.Statistics.ToList();
                }
                else if (mccList != null)
                {
                    List<int> mccIdList = new List<int>();
                    string[] arrAccountId = mccList.Split(new char[] { ',' });
                    for (var i = 0; i < arrAccountId.Length; i++)
                    {
                        try
                        {
                            mccIdList.Add(Int32.Parse(arrAccountId[i]));
                        }
                        catch (Exception)
                        {
                            //Deal with Exception
                        }
                    }
                    //return all countries matching mcc list
                    listOfStatistics = dbContext.Statistics.Where(x => arrAccountId.Contains(x.MCC.ToString())).ToList();
                }
                if(listOfStatistics.Count == 0)
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    response.Content = new StringContent("Out of Range. Request Failed");
                    return response;
                }
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, listOfStatistics);
        }
    }
}
