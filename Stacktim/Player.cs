using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Player
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Pseudo { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    [MaxLength(20)]
    public string Rank { get; set; }

    [Range(0, int.MaxValue)]
    public int TotalScore { get; set; } = 0;

    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}