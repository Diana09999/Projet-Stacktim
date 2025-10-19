using Xunit;
using Stacktim.Models;
using System;
using System.Collections.Generic;

namespace StacktimApi.Tests
{
    public class TeamControllerTests
    {
        [Fact]
        public void Team_CanBeCreatedAndPropertiesSet()
        {
            var captain = new Player { IdPlayers = 1, Name = "CaptainTest" };
            var teamPlayers = new List<TeamPlayer>();

            var team = new Team
            {
                IdTeams = 1,
                Name = "TestTeam",
                Tag = "TT",
                CaptainId = captain.IdPlayers,
                CreationDate = DateTime.Now,
                Captain = captain,
                TeamPlayers = teamPlayers
            };

            Assert.Equal(1, team.IdTeams);
            Assert.Equal("TestTeam", team.Name);
            Assert.Equal("TT", team.Tag);
            Assert.Equal(captain.IdPlayers, team.CaptainId);
            Assert.Equal(captain, team.Captain);
            Assert.Equal(teamPlayers, team.TeamPlayers);
            Assert.True(team.CreationDate <= DateTime.Now); 
        }
    }
}
