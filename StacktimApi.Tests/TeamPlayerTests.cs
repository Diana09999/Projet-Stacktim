using Xunit;
using Stacktim.Models;
using System;

namespace StacktimApi.Tests
{
    public class TeamPlayerTests
    {
        [Fact]
        public void TeamPlayer_CanBeCreatedAndPropertiesSet()
        {
            var player = new Player { IdPlayers = 1, Name = "TestPlayer" };
            var team = new Team { IdTeams = 1, Name = "TestTeam" };

            var teamPlayer = new TeamPlayer
            {
                PlayerId = player.IdPlayers,
                TeamId = team.IdTeams,
                Role = 1,
                JoinDate = DateTime.Now,
                Player = player,
                Team = team
            };

            Assert.Equal(1, teamPlayer.PlayerId);
            Assert.Equal(1, teamPlayer.TeamId);
            Assert.Equal(1, teamPlayer.Role);
            Assert.Equal(player, teamPlayer.Player);
            Assert.Equal(team, teamPlayer.Team);
        }
    }
}
