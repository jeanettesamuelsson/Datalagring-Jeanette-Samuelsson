using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;
using Application.Modules.Participants;
using Application.Modules.Participants.Inputs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IParticipantService, ParticipantService>();
builder.Services.AddCors();

builder.Services.AddDbContext<EduSqrlDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("EduSqrlDatabase"),
    sql => sql.MigrationsAssembly(typeof(EduSqrlDbContext).Assembly.FullName)
));

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

var list = new List<ParticipantDto>(){};

// api endpoins

// create
app.MapPost("api/participants", async (CreateParticipantRequest request, IParticipantService service, CancellationToken ct)  =>
{
    //map (create) a dto from user input 

    var input = new CreateParticipantInput(request.FirstName, request.LastName, request.Email);

    var participant = await service.CreateAsync(input, ct);

    return Results.Created($"/api/participants/{participant.Id}", participant);

});

// read all
app.MapGet("api/participants", async (IParticipantService service, CancellationToken ct) =>
{
   var participants =await service.GetAllParticipantsAsync(ct);
   return Results.Ok(participants);

});

// read by id
app.MapGet("api/participants/{id:guid}", async (Guid id, IParticipantService service, CancellationToken ct) =>
{
    var participant = await service.GetByIdAsync(id, ct);
    return participant is not null ? Results.Ok(participant) : Results.NotFound();
});

// update
app.MapPut("/api/participants/{id:guid}", async (Guid id, [FromBody] UpdateParticipantRequest request, IParticipantService service, CancellationToken ct) =>
{
    var input = new UpdateParticipantInput(request.Id, request.FirstName, request.LastName, request.Email);
    var participant = await service.UpdateAsync(input, ct);

    return Results.Ok(participant);

});

// delete
app.MapDelete("/api/participants/{id:guid}", async (Guid id, IParticipantService service, CancellationToken ct) =>
{
    var result = await service.DeleteAsync(id, ct);

    //if true 204 NoContent (deleted successfully) else 404 NotFound
    return result ? Results.NoContent() : Results.NotFound();
});

app.Run();

