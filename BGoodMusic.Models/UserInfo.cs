using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BGoodMusic.Models
{
    //Table name in db context
    public class UserInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Index("IX_UserIdId", Order = 2)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Index("IX_UserIdId", Order=1)]
        [MaxLength(384)]
        [Required]
        public string UserIdentifier { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
