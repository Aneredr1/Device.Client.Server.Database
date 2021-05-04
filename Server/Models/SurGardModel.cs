using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Server.Models
{
    public class di_devices
    {
        [Key][Required]
        public int id_device { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string number { get; set; }

        public IEnumerable<jr_surgard> jr_surgard { get; set; }

        public di_devices()
        {
            jr_surgard = new List<jr_surgard>();
        }
    }

    public class di_codes
    {
        [Key][Required]
        public int id_code { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string code { get; set; }

        public IEnumerable<jr_surgard> jr_surgard { get; set; }

        public di_codes()
        {
            jr_surgard = new List<jr_surgard>();
        }
    }

    public class di_groups
    {
        [Key][Required]
        public int id_group{ get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string code { get; set; }

        public IEnumerable<jr_surgard> jr_surgard { get; set; }

        public di_groups()
        {
            jr_surgard = new List<jr_surgard>();
        }
    }

    public class jr_surgard
    {
        [Key][Required]
        public int id_surgard { get; set; }

        [Required]
        public DateTime date_action { get; set; }

        [Required]
        public int? id_code { get; set; }
        public di_codes di_codes { get; set; }

        [Required]
        public int? id_device { get; set; }
        public di_devices di_devices { get; set; }

        [Required]
        public int? id_group { get; set; }
        public di_groups di_groups { get; set; }





    }
}
