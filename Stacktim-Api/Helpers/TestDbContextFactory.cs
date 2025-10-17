using Microsoft.EntityFrameworkCore;
using Stacktim.Data;
using Stacktim.Models;

namespace Stacktim.Helpers
{
    public static class TestDbContextFactory
    {
        public static StacktimContext Create()
        {
            var options = new DbContextOptionsBuilder<StacktimContext>()
                .UseInMemoryDatabase(databaseName: "TestStacktimDb")
                .Options;

            var context = new StacktimContext(options);
            context.Players.AddRange(
                new Player { IdPlayers = 1, Name = "Diana", Email = "diana@email.com", RankPlayer = "Gold", TotalScore = 1500 },
                new Player { IdPlayers = 2, Name = "Logy", Email = "logy@email.com", RankPlayer = "Platinum", TotalScore = 2200 },
                new Player { IdPlayers = 3, Name = "Polm", Email = "polm@email.com", RankPlayer = "Gold", TotalScore = 1800 }
            );
            context.SaveChanges();

            return context;
        }
    }
}