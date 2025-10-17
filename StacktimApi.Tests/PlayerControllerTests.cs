using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Stacktim.Controllers;
using Stacktim.DTOs;
using Stacktim.Helpers;
using Stacktim.Models;
using Stacktim.Controllers;
using Xunit;

namespace StacktimApi.Tests
{
    public class PlayersControllerTests
    {
        [Fact]
        public void GetPlayers_ReturnsAllPlayers()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);

            var result = controller.GetPlayers();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var players = okResult.Value as IEnumerable<PlayerDto>;
            players.Should().HaveCount(3);
            context.Dispose();
        }

        [Fact]
        public void GetPlayer_WithValidId_ReturnsPlayer()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);

            var result = controller.GetPlayer(1);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var player = okResult.Value as PlayerDto;
            player.Id.Should().Be(1);
            player.Pseudo.Should().Be("Diana");
            context.Dispose();
        }

        [Fact]
        public void GetPlayer_WithInvalidId_ReturnsNotFound()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);

            var result = controller.GetPlayer(42);

            result.Result.Should().BeOfType<NotFoundResult>();
            context.Dispose();
        }

        [Fact]
        public void CreatePlayer_WithValidData_ReturnsCreated()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);

            var dto = new CreatePlayerDto { Pseudo = "NewGuy", Email = "new@email.com", Rank = "Silver" };

            var result = controller.CreatePlayer(dto);

            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            var player = createdResult.Value as PlayerDto;
            player.Pseudo.Should().Be("NewGuy");
            context.Dispose();
        }

        [Fact]
        public void CreatePlayer_WithDuplicatePseudo_ReturnsBadRequest()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);

            var dto = new CreatePlayerDto { Pseudo = "Diana", Email = "other@email.com", Rank = "Gold" };

            var result = controller.CreatePlayer(dto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
            context.Dispose();
        }

        [Fact]
        public void DeletePlayer_WithValidId_ReturnsNoContent()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);

            var result = controller.DeletePlayer(1);

            result.Should().BeOfType<NoContentResult>();
            context.Players.Find(1).Should().BeNull();
            context.Dispose();
        }

        [Fact]
        public void GetLeaderboard_ReturnsOrderedPlayers()
        {
            var context = TestDbContextFactory.Create();
            var controller = new PlayersController(context);

            var result = controller.GetLeaderboard();

            var okResult = result.Result as OkObjectResult;
            var players = okResult.Value as IEnumerable<PlayerDto>;
            players.Should().BeInDescendingOrder(p => p.TotalScore);
            context.Dispose();
        }
    }
}