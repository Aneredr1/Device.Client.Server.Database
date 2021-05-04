using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Server.Models.Data
{
    public static class DataWorker
    {
        const char sg_flag = '5';
        const char R = '1';
        const char Q = 'E';
        const string CCC = "000";
        const string MT = "18";
        const string AAAA = "0001";


        // ген протокола и запись в журнал
        public static string get_surgard() 
        {
            string PP = string.Empty;
            string XYZ = string.Empty;
            string GG = string.Empty;

            using (SurGardDataContext sg = new SurGardDataContext())
            {
                GG = sg.Groups.Find(1).code; // берем код группы
                XYZ = sg.Codes.Find(1).code; // берем код события
                PP = sg.Devices.Find(1).number; // берем номер приемника
            }
            return sg_flag + PP + R + MT + AAAA + Q + XYZ + GG + CCC;
        }

        public static bool write_journal(string sg_string)
        {
            bool res = false;
            StringBuilder builder = new StringBuilder(sg_string);
            if (builder[0]=='5') // проверака того, что сообщение по протоколу SurGard
            {
                if (builder.Length == 19) //сообщение имеет 19 символов (по протоколу)
                {
                    string GG = builder[14].ToString() + builder[15].ToString(); //достаем из сообщения номер группы
                    string XYZ = builder[11].ToString() + builder[12].ToString() + builder[13].ToString(); // достаем код события
                    string PP = builder[1].ToString() + builder[2].ToString(); // достаем номер приемника
                    using (SurGardDataContext sg = new SurGardDataContext())
                    {
                        if (sg.Groups.Where(x=>x.code == GG).Any() && sg.Codes.Where(x => x.code == XYZ).Any() && sg.Devices.Where(x => x.number == PP).Any()) 
                            //проверяем есть ли коды в базе
                        {
                            int gr_id = sg.Groups.Where(x => x.code == GG).FirstOrDefault().id_group;
                            int cod_id = sg.Codes.Where(x => x.code == XYZ).FirstOrDefault().id_code;
                            int dev_id = sg.Devices.Where(x => x.number == PP).FirstOrDefault().id_device;

                            sg.Journal.Add(new jr_surgard { date_action = DateTime.Now, id_device = dev_id, id_code = cod_id, id_group = gr_id });
                            sg.SaveChanges();
                            res = true;
                        }
                    }
                }
            }
            return res;
        }


        public static Message make_message()
        {
            Message mes;
            using (SurGardDataContext sg = new SurGardDataContext())
            {
                jr_surgard note = sg.Journal.Last(); //последний элемент журнала

                string code = sg.Codes.Find(note.id_code).code; // код события

                string cod_name = sg.Codes.Find(note.id_code).name; // описание события

                string dev_code = sg.Devices.Find(note.id_device).number; // номер устройства

                string dev_name = sg.Devices.Find(note.id_device).name; // название устройства

                string gr_code = sg.Groups.Find(note.id_group).code; // код группы

                string gr_name = sg.Groups.Find(note.id_group).name; // название группы

                mes = new Message(note.date_action, code, cod_name, dev_code, dev_name, gr_code, gr_name);
            }

            return mes;
        }

    }
}
