using Xunit;
using Stacktim.Controllers;
using Stacktim.Data;
using Stacktim.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TeamsControllerTests
{
	[Fact]
	public async Task GetTeams_ReturnsAllTeams()
	{
		var context = TestDbContextFactory.Create();
		var controller = new TeamsController(context);

		var result = await controller.GetTeams();
		var teams = Assert.IsType<ActionResult<IEnumerable<Team>>>(result);
		Assert.NotNull(teams.Value);
		Assert.True(teams.Value.Count() >= 1);
		context.Dispose();
	}

	[Fact]
	public async Task GetTeam_WithValidId_ReturnsTeam()
	{
		var context = TestDbContextFactory.Create();
		var controller = new TeamsController(context);

		var id = 1;
		var result = await controller.GetTeam(id);
		var team = Assert.IsType<ActionResult<Team>>(result);
		Assert.NotNull(team.Value);
		Assert.Equal(id, team.Value.IdTeams);
		context.Dispose();
	}

	[Fact]
	public async Task GetTeam_WithInvalidId_ReturnsNotFound()
	{
		var context = TestDbContextFactory.Create();
		var controller = new TeamsController(context);

		var result = await controller.GetTeam(999);
		var actionResult = Assert.IsType<ActionResult<Team>>(result);
		Assert.IsType<NotFoundResult>(actionResult.Result);
		context.Dispose();
	}

	[Fact]
	public async Task PostTeam_WithValidData_CreatesTeam()
	{
		var context = TestDbContextFactory.Create();
		var controller = new TeamsController(context);

		var team = new Team { Name = "TestTeam", Tag = "TTT", CaptainId = 1 };
		var result = await controller.PostTeam(team);
		var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
		var createdTeam = Assert.IsType<Team>(createdResult.Value);
		Assert.Equal("TestTeam", createdTeam.Name);
		Assert.Equal("TTT", createdTeam.Tag);
		context.Dispose();
	}

	[Fact]
	public async Task PutTeam_WithValidId_UpdatesTeam()
	{
		var context = TestDbContextFactory.Create();
		var controller = new TeamsController(context);

		var team = context.Teams.First();
		team.Name = "UpdatedName";
		var result = await controller.PutTeam(team.IdTeams, team);
		Assert.IsType<NoContentResult>(result);
		context.Dispose();
	}

	[Fact]
	public async Task PutTeam_WithInvalidId_ReturnsBadRequest()
	{
		var context = TestDbContextFactory.Create();
		var controller = new TeamsController(context);

		var team = context.Teams.First();
		var result = await controller.PutTeam(team.IdTeams + 999, team);
		Assert.IsType<BadRequestResult>(result);
		context.Dispose();
	}

	[Fact]
	public async Task DeleteTeam_WithValidId_DeletesTeam()
	{
		var context = TestDbContextFactory.Create();
		var controller = new TeamsController(context);

		var team = new Team { Name = "DelTeam", Tag = "DEL", CaptainId = 1 };
		context.Teams.Add(team);
		context.SaveChanges();

		var result = await controller.DeleteTeam(team.IdTeams);
		Assert.IsType<NoContentResult>(result);
		context.Dispose();
	}

	[Fact]
	public async Task DeleteTeam_WithInvalidId_ReturnsNotFound()
	{
		var context = TestDbContextFactory.Create();
		var controller = new TeamsController(context);

		var result = await controller.DeleteTeam(999);
		Assert.IsType<NotFoundResult>(result);
		context.Dispose();
	}
}