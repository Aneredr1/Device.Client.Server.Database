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

        public Message(DateTime date_action, string code, string code_name, string device_code, string device_name, string group_code, string group_name) 
        {
            this.date_action = date_action;
            this.code = code;
            this.code_name = code_name;
            this.device_code = device_code;
            this.device_name = device_name;
            this.group_code = group_code;
            this.group_name = group_name;
        }
    }
}
