namespace StarStuff.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Log
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [MaxLength(DataConstants.LogTable.ActionMaxLength)]
        public string Action { get; set; }

        [Required]
        [MaxLength(DataConstants.LogTable.TableNameMaxLength)]
        public string TableName { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}