using System.ComponentModel.DataAnnotations;

namespace Stacktim.DTOs
{

    public class CreatePlayerDto
    {
        [Required]
        [StringLength(50)]
        public string Pseudo { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Rank { get; set; }
    }
}