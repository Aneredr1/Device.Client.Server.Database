using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Models
{
    public class Message
    {
        public DateTime date_action { get; set; }

        public string code { get; set; }

        public string code_name { get; set; }

        public string device_code { get; set; }

        public string device_name { get; set; }

        public string group_code { get; set; }

        public string group_name { get; set; }
    }
}
