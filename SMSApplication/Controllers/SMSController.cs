using SMSApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace SMSApplication.Controllers
{

    public class SMSController : ApiController
    {
        private string path = @"C://log//Logging.txt";
        public enum State
        {
            Success,
            Fail
        };

        // GET api/sms

        [HttpGet]
        public string GET()
        {
            return "abc";
        }

        // GET api/sms
        [HttpGet]
        public HttpResponseMessage send(string from, string to, string text)
        {
            //GET /sms/send.json?from=The+Sender & to=+4917421293388 & text=Hello+World
            // This logic is for sending the SMS message to a dummy implementation
            // which just dumps the SMS to a log file.
            try
            {
                var sw = new StreamWriter(path, true);
                sw.Write("Sender = " + from + ", " + "Reciever = " + to + ", " + "TextMessage = " + text + " \n");
                sw.Flush();
                sw.Close();

                // Also writing the SMS to the SMS table
                using (DataClassesDataContext dbContext = new DataClassesDataContext())
                {
                    SMS anSMS = new SMS();
                    //first identiying the MCC of the receiver 
                    string mcc = to.Substring(1, 2);
                    anSMS.MobileCountryCode = int.Parse(mcc);
                    //lets add fake price for this SMS bill
                    anSMS.Price = 0.009;
                    anSMS.FromSender = from;
                    //lets identify country name from MCC e.g. +49
                    int countryID = dbContext.Country.Where(x => x.MCC.ToString() == mcc).Select(x => x.MCC).SingleOrDefault();
                    anSMS.CountryID = countryID;
                    anSMS.State = State.Success.ToString();
                    anSMS.ToReciever = to;
                    anSMS.dateTime = DateTime.UtcNow;

                    dbContext.SMS.InsertOnSubmit(anSMS);
                    dbContext.SubmitChanges();
                }
                //if sms has been successfullz looged 
                // return State.Success;
                return Request.CreateResponse(HttpStatusCode.OK, State.Success);
            }
            catch (Exception e)
            {

                //else fail
                return Request.CreateResponse(HttpStatusCode.BadRequest, State.Fail);

            }

        }

        // GET api/sms
        [HttpGet]
        public HttpResponseMessage sent(DateTime dateTimeFrom, DateTime dateTimeTo, int skip, int take)
        { //Tuple<int,List<SMS>>
            int count = 0;
            List<SMS> listOfSms = new List<SMS>();

            using (DataClassesDataContext dbContext = new DataClassesDataContext())
            {
                //count = dbContext.SMS.Where(x => x.dateTime > dateTimeFrom && x.dateTime < dateTimeTo).Skip(skip).Take(take).Count();
                var res = dbContext.SMS.Where(x => x.dateTime >= dateTimeFrom && x.dateTime <= dateTimeTo).ToList();
                listOfSms = res.Skip(skip).Take(take).ToList();
                count = listOfSms.Count;
            }
            var check = Tuple.Create(count, listOfSms);

            Items item = new Items();
            item.count = count;
            item.listofSMS = listOfSms;


            return Request.CreateResponse(HttpStatusCode.OK, item);
        }



    }
}
