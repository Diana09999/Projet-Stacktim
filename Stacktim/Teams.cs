using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Teams
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string Tag { get; set; }

    [Required]
    [MaxLength(20)]
    public string CaptainId { get; set; }

    [Range(0, int.MaxValue)]
    public int TotalScore { get; set; } = 0;

    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}