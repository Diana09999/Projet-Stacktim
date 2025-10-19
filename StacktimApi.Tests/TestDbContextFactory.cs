using System;
using Microsoft.EntityFrameworkCore;
using Stacktim.Data;
using Stacktim.Models;

public static class TestDbContextFactory
{
    public static StacktimContext Create()
    {
        var options = new DbContextOptionsBuilder<StacktimContext>()
            .UseInMemoryDatabase(databaseName: "StacktimTestDb_" + Guid.NewGuid())
            .Options;

        var context = new StacktimContext(options);

        context.Players.AddRange(
            new Player { IdPlayers = 1, Name = "Diana", Email = "diana@email.com", RankPlayer = "Bronze", TotalScore = 1500 },
            new Player { IdPlayers = 2, Name = "Logy", Email = "logy@email.com", RankPlayer = "Platinum", TotalScore = 2200 },
            new Player { IdPlayers = 3, Name = "Kiko", Email = "kiko@email.com", RankPlayer = "Silver", TotalScore = 1800 },
            new Player { IdPlayers = 4, Name = "Polm", Email = "polm@email.com", RankPlayer = "Gold", TotalScore = 3100 },
            new Player { IdPlayers = 5, Name = "Sefi", Email = "sefi@email.com", RankPlayer = "Diamond", TotalScore = 900 },
            new Player { IdPlayers = 6, Name = "Azerty", Email = "azerty@email.com", RankPlayer = "Master", TotalScore = 4500 }
        );
        context.SaveChanges();

        return context;
    }
}