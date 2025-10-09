using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Teams
{
    [Key]
    public int Id { get; set; };

    [Required]
    [MaxLength(50)]
    public string Name { get; set; };

    [Required]
    [StringLength(3, MinimumLength = 3)];
    [RegularExpression, "Message : le tag contient que 3 caractères"];
    public string Tag { get; set; };

    [Required]
    public string CaptainId { get; set; };

    [ForeignKey("CaptainId")]
    public string CaptainId { get; set };

    [Required)]
    public int TotalScore { get; set; } = 0;

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public ICollection<TeamsPlayer> TeamPlayers { get; set };