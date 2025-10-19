using System;
using System.Linq;
using Stacktim.Data;
using Stacktim.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace StacktimApi.Tests
{
    public class TeamPlayerTests
    {
        private StacktimContext CreateContext()
        {
            return TestDbContextFactory.Create();
        }

        private Team AddTeam(StacktimContext context, string name, string tag, int captainId)
        {
            var team = new Team
            {
                Name = name,
                Tag = tag,
                CaptainId = captainId,
                CreationDate = DateTime.Now
            };
            context.Teams.Add(team);
            context.SaveChanges();
            return team;
        }

        private TeamPlayer AddPlayerToTeam(StacktimContext context, int teamId, int playerId, int role)
        {
            if (role < 0 || role > 2)
                throw new ArgumentException("role seulement 0, 1 et 2");

            var teamPlayer = new TeamPlayer
            {
                TeamId = teamId,
                PlayerId = playerId,
                Role = role,
                JoinDate = DateTime.Now
            };
            context.TeamPlayers.Add(teamPlayer);
            context.SaveChanges();
            return teamPlayer;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AddPlayerToTeam_WithDifferentRoles_AddsSuccessfully(int role)
        {
            var context = CreateContext();
            var captain = context.Players.First();
            var team = AddTeam(context, "STL", "TMA", captain.IdPlayers);

            var teamPlayer = AddPlayerToTeam(context, team.IdTeams, captain.IdPlayers, role);

            var dbEntry = context.TeamPlayers.FirstOrDefault(tp => tp.TeamId == team.IdTeams && tp.PlayerId == captain.IdPlayers);
            dbEntry.Should().NotBeNull();
            dbEntry.Role.Should().Be(role);

            context.Dispose();
        }

        [Fact]
        public void AddPlayerToTeam_WithInvalidRole_ThrowsException()
        {
            var context = CreateContext();
            var captain = context.Players.First();
            var team = AddTeam(context, "STW", "STW", captain.IdPlayers);

            Action act = () => AddPlayerToTeam(context, team.IdTeams, captain.IdPlayers, 15);

            act.Should().Throw<ArgumentException>().WithMessage("role seulement 0, 1 et 2");
            context.Dispose();
        }

        [Fact]
        public void DeleteTeam_RemovesTeamPlayersCascade()
        {
            var context = CreateContext();
            var captain = context.Players.First();
            var team = AddTeam(context, "STX", "STX", captain.IdPlayers);
            AddPlayerToTeam(context, team.IdTeams, captain.IdPlayers, 0);

            context.TeamPlayers.Count().Should().Be(1);

            context.Teams.Remove(team);
            context.SaveChanges();

            context.TeamPlayers.Count().Should().Be(0);
            context.Dispose();
        }

        [Fact]
        public void AddSamePlayerTwiceToTeam_ThrowsException()
        {
            var context = CreateContext();
            var captain = context.Players.First();
            var team = AddTeam(context, "STX", "STX", captain.IdPlayers);

            AddPlayerToTeam(context, team.IdTeams, captain.IdPlayers, 1);

            Action act = () =>
            {
                AddPlayerToTeam(context, team.IdTeams, captain.IdPlayers, 1);
            };

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("déja track");

            context.Dispose();
        }
    }
}
