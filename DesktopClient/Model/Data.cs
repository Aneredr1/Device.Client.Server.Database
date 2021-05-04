using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace DesktopClient.Model
{
    public class Data
    {
        public User CurrentUser { get; set; }

        async void GetUser()
        {
            try
            {
                using (FileStream fs = new FileStream("user.json", FileMode.Open))
                {
                    CurrentUser = await JsonSerializer.DeserializeAsync<User>(fs);
                }
            }
            catch (Exception ex)
            {
                using (FileStream fs = new FileStream("user.json", FileMode.Create))
                {
                    User newUser = new User();
                    await JsonSerializer.SerializeAsync<User>(fs, newUser);
                    CurrentUser = newUser;
                }
            }
        }

        public async void SerializeUser()
        {
            try
            {
                using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
                {
                    await JsonSerializer.SerializeAsync<User>(fs, CurrentUser);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Data()
        {
            GetUser();
        }
    }
}
