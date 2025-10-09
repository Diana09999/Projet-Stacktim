using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TeamPlayer
{
    
    public int TeamId { get; set; }
    public int PlayerId { get; set; }

    [Range(0, 2)]
    public int Role { get; set; }

    [ForeignKey(nameof("TeamId"))]
    public Team Team { get; set; }

    [ForeignKey(nameof("PlayerId"))]
    public Player Player { get; set; }

    public DateTime JoinDate { get; set; } = DateTime.Now;

}