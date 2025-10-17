using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Stacktim.Data;
using Stacktim.Models;
using Stacktim.DTOs;

[Route("api/[controller]")]
[ApiController]
public class TeamsController : ControllerBase
{
    private readonly StacktimContext _context;

    public TeamsController(StacktimContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Team>> GetTeams()
    {
        return Ok(_context.Teams.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Team> GetTeam(int id)
    {
        var team = _context.Teams.Find(id);
        if (team == null)
            return NotFound();
        return Ok(team);
    }

    [HttpPost]
    public ActionResult<Team> CreateTeam([FromBody] Team team)
    {
        if (_context.Teams.Any(t => t.Name == team.Name))
            return BadRequest("Nom déjà utilisé");
        if (_context.Teams.Any(t => t.Tag == team.Tag))
            return BadRequest("Tag déjà utilisé");

        _context.Teams.Add(team);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetTeam), new { id = team.IdTeams }, team);
    }

    [HttpGet("{id}/roster")]
    public ActionResult GetRoster(int id)
    {
        var team = _context.Teams.Find(id);
        if (team == null)
            return NotFound();

        var roster = _context.TeamPlayers
            .Where(tp => tp.TeamId == id)
            .Join(_context.Players, tp => tp.PlayerId, p => p.IdPlayers,
                (tp, p) => new
                {
                    Pseudo = p.Name,
                    Role = tp.Role
                }).ToList();

        return Ok(roster);
    }
}