namespace CarDealer.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Log
    {
        public int Id { get; set; }

        public Operation Operation { get; set; }

        [Required]
        public string ModifiedTable { get; set; }

        [Required]
        public User User { get; set; }

        public DateTime Time { get; set; }
    }
}