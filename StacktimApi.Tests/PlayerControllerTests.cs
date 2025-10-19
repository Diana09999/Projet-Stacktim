using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Stacktim.Controllers;
using Stacktim.DTOs;
using Stacktim.Models;
using Xunit;

namespace StacktimApi.Tests
{
    public class PlayersControllerTests
    {
        [Fact]
        public void GetPlayers_ReturnsAllPlayers()
        {
            var context = TestDbContextFactory.Create();
            var player = new Player { Name = "Diana", Email = "diana@email.com", RankPlayer = "Gold", TotalScore = 10, RegistrationDate = DateTime.Now };
            context.Players.Add(player);
            context.SaveChanges();

            var controller = new PlayersController(context);

            var result = controller.GetPlayers();
            var players = Assert.IsType<ActionResult<IEnumerable<PlayerDto>>>(result);
            Assert.NotNull(players.Value);
            Assert.True(players.Value.Any());
            context.Dispose();
        }

        [Fact]
        public void GetPlayer_WithValidId_ReturnsPlayer()
        {
            var context = TestDbContextFactory.Create();
            var player = new Player { Name = "Diana", Email = "diana@email.com", RankPlayer = "Gold", TotalScore = 10, RegistrationDate = DateTime.Now };
            context.Players.Add(player);
            context.SaveChanges();

            var controller = new PlayersController(context);

            var result = controller.GetPlayer(player.IdPlayers);
            var actionResult = Assert.IsType<ActionResult<PlayerDto>>(result);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(player.IdPlayers, actionResult.Value.Id);
            context.Dispose();
        }

        [Fact]
        public void GetPlayer_WithInvalidId_ReturnsNotFound()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);

            var result = controller.GetPlayer(999);
            var actionResult = Assert.IsType<ActionResult<PlayerDto>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
            context.Dispose();
        }

        [Fact]
        public void CreatePlayer_WithValidData_ReturnsCreated()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);

            var dto = new CreatePlayerDto { Pseudo = "NewPlayer123", Email = "newplayer@email.com", Rank = "Silver" };

            var result = controller.CreatePlayer(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdPlayer = Assert.IsType<PlayerDto>(createdResult.Value);
            Assert.Equal("NewPlayer123", createdPlayer.Pseudo);
            Assert.Equal("newplayer@email.com", createdPlayer.Email);
            context.Dispose();

        }

        [Fact]
        public void CreatePlayer_WithDuplicatePseudo_ReturnsBadRequest()
        {
            var context = TestDbContextFactory.Create();
            var player = new Player { Name = "Diana", Email = "diana@email.com", RankPlayer = "Gold", TotalScore = 10, RegistrationDate = DateTime.Now };
            context.Players.Add(player);
            context.SaveChanges();

            var controller = new PlayersController(context);

            var dto = new CreatePlayerDto { Pseudo = "Diana", Email = "other@email.com", Rank = "Silver" };
            var result = controller.CreatePlayer(dto);
            Assert.IsType<BadRequestObjectResult>(result.Result);
            context.Dispose();
        }

        [Fact]
        public void UpdatePlayer_WithValidData_UpdatesPlayer()
        {
            var context = TestDbContextFactory.Create();
            var player = new Player { Name = "Diana", Email = "diana@email.com", RankPlayer = "Gold", TotalScore = 10 };
            context.Players.Add(player);
            context.SaveChanges();

            var controller = new PlayersController(context);
            var dto = new UpdatePlayerDto { Pseudo = "Felix", Email = "felix@email.com", Rank = "Diamond", TotalScore = 100 };

            Console.WriteLine($"creation d'un joueur {dto.Pseudo} ({dto.Email})");

            var result = controller.UpdatePlayer(player.IdPlayers, dto);
            Assert.IsType<NoContentResult>(result);
            context.Dispose();
        }

        [Fact]
        public void UpdatePlayer_WithInvalidId_ReturnsNotFound()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);
            var dto = new UpdatePlayerDto { Pseudo = "Diana" };

            var result = controller.UpdatePlayer(999, dto);
            Assert.IsType<NotFoundResult>(result);
            context.Dispose();
        }

        [Fact]
        public void UpdatePlayer_WithDuplicatePseudo_ReturnsBadRequest()
        {
            var context = TestDbContextFactory.Create();
            var player1 = new Player { Name = "Diana", Email = "diana@email.com", RankPlayer = "Gold", TotalScore = 10, RegistrationDate = DateTime.Now };
            var player2 = new Player { Name = "Alex", Email = "alex@email.com", RankPlayer = "Silver", TotalScore = 0, RegistrationDate = DateTime.Now };
            context.Players.Add(player1);
            context.Players.Add(player2);
            context.SaveChanges();

            var controller = new PlayersController(context);
            var dto = new UpdatePlayerDto { Pseudo = "Alex" };

            var result = controller.UpdatePlayer(player1.IdPlayers, dto);
            Assert.IsType<BadRequestObjectResult>(result);
            context.Dispose();
        }

        [Fact]
        public void UpdatePlayer_WithDuplicateEmail_ReturnsBadRequest()
        {
            var context = TestDbContextFactory.Create();
            var player1 = new Player { Name = "Diana", Email = "diana@email.com", RankPlayer = "Gold", TotalScore = 10, RegistrationDate = DateTime.Now };
            var player2 = new Player { Name = "Alex", Email = "alex@email.com", RankPlayer = "Silver", TotalScore = 0, RegistrationDate = DateTime.Now };
            context.Players.Add(player1);
            context.Players.Add(player2);
            context.SaveChanges();

            var controller = new PlayersController(context);
            var dto = new UpdatePlayerDto { Email = "alex@email.com" };

            var result = controller.UpdatePlayer(player1.IdPlayers, dto);
            Assert.IsType<BadRequestObjectResult>(result);
            context.Dispose();
        }

        [Fact]
        public void DeletePlayer_WithValidId_DeletesPlayer()
        {
            var context = TestDbContextFactory.Create();
            var player = new Player { Name = "Diana", Email = "diana@email.com", RankPlayer = "Gold", TotalScore = 10, RegistrationDate = DateTime.Now };
            context.Players.Add(player);
            context.SaveChanges();

            var controller = new PlayersController(context);

            var result = controller.DeletePlayer(player.IdPlayers);
            Assert.IsType<NoContentResult>(result);
            context.Dispose();
        }

        [Fact]
        public void DeletePlayer_WithInvalidId_ReturnsNotFound()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);

            var result = controller.DeletePlayer(999);
            Assert.IsType<NotFoundResult>(result);
            context.Dispose();
        }

        [Fact]
        public void GetLeaderboard_ReturnsTopPlayers()
        {
            var context = TestDbContextFactory.Create();
            for (int i = 0; i < 11; i++)
            {
                context.Players.Add(new Player { Name = "Player" + i, Email = $"mail{i}@mail.com", RankPlayer = "Bronze", TotalScore = i * 10, RegistrationDate = DateTime.Now });
            }
            context.SaveChanges();

            var controller = new PlayersController(context);

            var result = controller.GetLeaderboard();
            var players = Assert.IsType<ActionResult<IEnumerable<PlayerDto>>>(result);
            Assert.NotNull(players.Value);
            Assert.Equal(10, players.Value.Count());
            context.Dispose();
        }
    }
}