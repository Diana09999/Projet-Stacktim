public class UpdatePlayer
{
    [StringLength(50)]
    public string? Pseudo { get; set; }

    [StringLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Rank { get; set; }

    public int? TotalScore { get; set; }
}