using System.ComponentModel.DataAnnotations;

namespace Stacktim.DTOs
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Pseudo { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rank { get; set; } = null!;
        public int TotalScore { get; set; }
    }
}