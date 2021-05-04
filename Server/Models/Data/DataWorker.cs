using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Server.Models.Data
{
    class DataWorker
    {
        const char sg_flag = '5';
        const char R = '1';
        const char Q = 'E';
        const string CCC = "000";
        const string MT = "18";
        const string AAAA = "0001";


        // ген протокола и запись в журнал
        public string get_surgard() 
        {
            string PP = string.Empty;
            string XYZ = string.Empty;
            string GG = string.Empty;

            using (SurGardDataContext sg = new SurGardDataContext())
            {
                GG = sg.Groups.Find(1).code;
                XYZ = sg.Codes.Find(1).code;
                PP = sg.Devices.Find(1).number;
            }
            return sg_flag + PP + R + MT + AAAA + Q + XYZ + GG + CCC;
        }

        public void write_journal(string sg_string)
        {
            
        }

        // создание сообщения на клиент


    }
}
