using Application.Abstractions.Persistence;
using Application.Modules.Participants;
using Application.Modules.Participants.Inputs;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Dtos;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


builder.Services.AddScoped<IParticipantRepository, ParticipantEntityRepository>();
builder.Services.AddScoped<IUnitOfWork, EfcUnitOfWork>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();

builder.Services.AddCors();

builder.Services.AddDbContext<EduSqrlDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("EduSqrlDatabase"),
    sql => sql.MigrationsAssembly(typeof(EduSqrlDbContext).Assembly.FullName)
));

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

var list = new List<ParticipantDto>(){};

// api endpoins

// create
app.MapPost("api/participants", async (CreateParticipantRequest request, IParticipantService service, CancellationToken ct)  =>
{
    //map (create) a dto from user input 

    var input = new CreateParticipantInput(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Roles);

    var id = await service.CreateAsync(input, ct);

    return Results.Created($"/api/participants/{id}", id);

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
    var input = new UpdateParticipantInput(request.Id, request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Roles, request.RowVersion);
    var result = await service.UpdateAsync(input, ct);

    return result is not null ? Results.Ok(result) : Results.NotFound();

});

// delete
app.MapDelete("/api/participants/{id:guid}", async (Guid id, [FromHeader(Name = "If-Match")] string rowVersionStr, IParticipantService service, CancellationToken ct) =>
{
    // convert string to byte array
    var rowVersion = Convert.FromBase64String(rowVersionStr);

    await service.DeleteAsync(id, rowVersion, ct);
    return Results.NoContent();
});

app.Run();

