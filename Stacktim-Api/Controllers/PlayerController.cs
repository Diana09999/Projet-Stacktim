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

            return Ok(players);
        }

        [HttpGet("{id}")]
        public ActionResult<PlayerDto> GetPlayer(int id)
        {
            var player = _context.Players
                .Where(p => p.IdPlayers == id)
                .Select(p => new PlayerDto
                {
                    Id = p.IdPlayers,
                    Pseudo = p.Name,
                    Email = p.Email,
                    Rank = p.RankPlayer,
                    TotalScore = p.TotalScore
                }).FirstOrDefault();

            if (player == null)
                return NotFound();

            return Ok(player);
        }

        [HttpPost]
        public ActionResult<PlayerDto> CreatePlayer([FromBody] CreatePlayerDto dto)
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
                TotalScore = 0,
                RegistrationDate = DateTime.Now
            };
            _context.Players.Add(player);
            _context.SaveChanges();

            var result = new PlayerDto
            {
                Id = player.IdPlayers,
                Pseudo = player.Name,
                Email = player.Email,
                Rank = player.RankPlayer,
                TotalScore = player.TotalScore
            };
            return CreatedAtAction(nameof(GetPlayer), new { id = player.IdPlayers }, result);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePlayer(int id, [FromBody] UpdatePlayerDto dto)
        {
            var player = _context.Players.Find(id);
            if (player == null)
                return NotFound();

            if (!string.IsNullOrEmpty(dto.Pseudo) && dto.Pseudo != player.Name
                && _context.Players.Any(p => p.Name == dto.Pseudo))
                return BadRequest("Pseudo déja utilisé");
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != player.Email
                && _context.Players.Any(p => p.Email == dto.Email))
                return BadRequest("Email déja utilisé");

            if (!string.IsNullOrEmpty(dto.Pseudo)) player.Name = dto.Pseudo;
            if (!string.IsNullOrEmpty(dto.Email)) player.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Rank)) player.RankPlayer = dto.Rank;
            if (dto.TotalScore.HasValue) player.TotalScore = dto.TotalScore.Value;

            _context.SaveChanges();
            return NoContent();
        }

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

            return Ok(topPlayers);
        }
    }
}