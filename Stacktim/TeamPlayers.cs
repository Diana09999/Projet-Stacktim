using System;
using System.ComponentModel.DataAnnotations;
using System ComponentModel.DataAnnotations.Schema;

public class TeamsPlayer
{

    public int TeamId { get; set; };

    public int PlayerId { get; set; };

    public int Role { get; set; };

    [ForeignKey("TeamId")]
    public Teams Teams { get; set; };

    [ForeignKey("PlayerId")]
    public Player Player { get; set; };

    public DateTime JoinDate { get; set; } = DateTime.Now;

}