using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Stacktim.Data;
using Stacktim.DTOs;
using Stacktim.Models;

namespace Stacktim.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly StacktimContext _context;

        public PlayersController(StacktimContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlayerDto>> GetPlayers()
        {
            var players = _context.Players
                .Select(p => new PlayerDto
                {
                    Id = p.IdPlayers,
                    Pseudo = p.Name,
                    Email = p.Email,
                    Rank = p.RankPlayer,
                    TotalScore = p.TotalScore
                }).ToList();

            return players;
        }

        [HttpGet("{id}")]
        public ActionResult<PlayerDto> GetPlayer(int id)
        {
            var player = _context.Players.Find(id);
            if (player == null)
            
                return NotFound();

              var dto = new PlayerDto
              {
                  Id = player.IdPlayers,
                  Pseudo = player.Name,
                  Email = player.Email,
                  Rank = player.RankPlayer,
                  TotalScore = player.TotalScore
              };
                return dto;
        }

        [HttpPost]
        public ActionResult<PlayerDto> CreatePlayer(CreatePlayerDto dto)
        {
            if (_context.Players.Any(p => p.Name == dto.Pseudo))
                return BadRequest("Pseudo déja utilisé");
            if (_context.Players.Any(p => p.Email == dto.Email))
                return BadRequest("Email déja utilisé");

            var player = new Player
            {
                Name = dto.Pseudo,
                Email = dto.Email,
                RankPlayer = dto.Rank,
                RegistrationDate = DateTime.Now,
                TotalScore = 0,
            };

            _context.Players.Add(player);
            _context.SaveChanges();

            var createdPlayer = new PlayerDto
            {
                Id = player.IdPlayers,
                Pseudo = player.Name,
                Email = player.Email,
                Rank = player.RankPlayer,
                TotalScore = player.TotalScore
            };
            return CreatedAtAction(nameof(GetPlayer), new { id = player.IdPlayers }, createdPlayer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePlayer(int id, UpdatePlayerDto dto)
        {
            var player = _context.Players.FirstOrDefault(p => p.IdPlayers == id);
            if (player == null) return NotFound();

            if (!string.IsNullOrEmpty(dto.Pseudo) && PseudoExists(dto.Pseudo, id))
                return BadRequest("Ce pseudo est déjà utilisé.");
            if (!string.IsNullOrEmpty(dto.Email) && EmailExists(dto.Email, id))
                return BadRequest("Cet email est déjà utilisé.");

            player.Name = dto.Pseudo ?? player.Name;
            player.Email = dto.Email ?? player.Email;
            player.RankPlayer = dto.Rank ?? player.RankPlayer;
            player.TotalScore = dto.TotalScore ?? player.TotalScore;

            _context.SaveChanges();
            return NoContent();
        }

        private bool PseudoExists(string pseudo, int excludeId) =>
        
            _context.Players.Any(p => p.Name == pseudo && p.IdPlayers != excludeId);

        private bool EmailExists(string email, int excludeId) =>
            _context.Players.Any(p => p.Email == email && p.IdPlayers != excludeId);


        [HttpDelete("{id}")]
        public ActionResult DeletePlayer(int id)
        {
            var player = _context.Players.Find(id);
            if (player == null)
                return NotFound();

            _context.Players.Remove(player);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet("leaderboard")]
        public ActionResult<IEnumerable<PlayerDto>> GetLeaderboard()
        {
            var topPlayers = _context.Players
                .OrderByDescending(p => p.TotalScore)
                .Take(10)
                .Select(p => new PlayerDto
                {
                    Id = p.IdPlayers,
                    Pseudo = p.Name,
                    Email = p.Email,
                    Rank = p.RankPlayer,
                    TotalScore = p.TotalScore
                }).ToList();

            return topPlayers;
        }
    }
}