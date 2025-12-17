using BackendExamHub.Models.DTO;
using BackendExamHub.Services;
using Microsoft.AspNetCore.Mvc;
namespace BackendExamHub.Endpoints;

public static class MyofficeEndpoints
{
    public static void MapMyofficeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/Myoffice")
            .WithTags("Myoffice")
            .WithOpenApi();

        group.MapGet("/GetAccount/{id}", async (string id, MyofficeServices service) =>
        {
            try
            {
                var result = await service.GetAccountById(id);
                if (result == null)
                    return Results.NotFound(new { message = $"Account with ID '{id}' not found" });

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });
        group.MapGet("/GetAccounts/", async (MyofficeServices service) =>
        {
            try
            {
                return Results.Ok(await service.GetAccounts());
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });
        group.MapPost("/CreateAccount", async (CreateAccountRequest request, MyofficeServices service) =>
        {
            try
            {
                await service.CreateAccount(request);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });
        group.MapPut("/UpdateAccount", async (UpdateAccountRequest request, MyofficeServices service) =>
        {
            try
            {
                await service.UpdateAccount(request);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });
        group.MapDelete("/DeleteAccount/{id}", async (string id, MyofficeServices service) =>
        {
            try
            {
                await service.DeleteAccount(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });
        group.MapPost("/DeleteAccounts", async (List<string> ids, MyofficeServices service) =>
        {
            try
            {
                await service.DeleteAccounts(ids);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });
    }
}