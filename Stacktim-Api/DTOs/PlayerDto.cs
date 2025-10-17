using System.ComponentModel.DataAnnotations;

namespace Stacktim.DTOs
 {

public class PlayerDto
  {
    public int Id { get; set; }
    public string? Pseudo { get; set; }
    public string Email { get; set; }
    public string Rank { get; set; }
    public int TotalScore { get; set; }
   }
  }