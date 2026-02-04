using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors();

builder.Services.AddDbContext<EduSqrlDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("EduSqrlDatabase"),
    sql => sql.MigrationsAssembly(typeof(EduSqrlDbContext).Assembly.FullName)
));

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// api endpoins

// create

app.MapPost("api/participants", (CreateParticipantRequest request)  =>
{
    //input = new CreateParticipantInput (request.FirstName, request.LastName, request.Email, request.PhoneNumber);
    //particpant = service.CreateParticipant(input);
    //return Results.Created($"/api/participants/{{newParticipant.Id}}", participant);

});

// read 

app.MapGet("api/participants", () =>
{
    //get participants from service

    //var participants = service.GetAllParticipants();
    //return Results.Ok(participants);

});



app.Run();

