using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BGoodMusic.Models
{
    public class Rehearsal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        public Nullable<TimeSpan> Duration { get; set; }

        [MaxLength(512)]
        public string Location { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(2048)]
        public string Agenda { get; set; }
    }
}
