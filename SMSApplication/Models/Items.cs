using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSApplication.Models
{
    public class Items
    {
        public int count { get; set; }
        public List<SMS> listofSMS { get; set; }
    }
}