using Application.Abstractions.Persistence;
using Application.Modules.Courses;
using Application.Modules.Courses.Input;
using Application.Modules.Participants;
using Application.Modules.Participants.Inputs;
using Application.Modules.Roles;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Dtos;
using Presentation.Dtos.Course;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


builder.Services.AddScoped<IParticipantRepository, ParticipantEntityRepository>();
builder.Services.AddScoped<IUnitOfWork, EfcUnitOfWork>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();


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


#region role endpoints

var roleGroup = app.MapGroup("/roles").WithTags("Roles");

app.MapGet("api/roles", async (IRoleService service, CancellationToken ct) =>
{
    var roles = await service.GetRolesAsync(ct);
    return Results.Ok(roles);
});

#endregion


#region participant endpoints

var participantGroup = app.MapGroup("/participants").WithTags("Participants");

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

    try {
        var result = await service.UpdateAsync(input, ct);
        return result is not null ? Results.Ok(result) : Results.NotFound();

    } catch (ArgumentException ex) 

    { 
        return Results.BadRequest(ex.Message);
    }   


});

// delete
app.MapDelete("/api/participants/{id:guid}", async (Guid id, [FromHeader(Name = "If-Match")] string rowVersionStr, IParticipantService service, CancellationToken ct) =>
{
    // convert string to byte array
    var rowVersion = Convert.FromBase64String(rowVersionStr);

    await service.DeleteAsync(id, rowVersion, ct);
    return Results.NoContent();
});

#endregion


#region course endpoints

var courseGroup = app.MapGroup("/courses").WithTags("Courses");

// create
app.MapPost("api/courses", async (CreateCourseRequest request, ICourseService service, CancellationToken ct) =>
{
    try
    {
        //map (create) a dto from user input 

        var input = new CreateCourseInput(request.CourseCode, request.CourseName, request.Description);

        var id = await service.CreateAsync(input, ct);

        return Results.Created($"/api/courses/{id}", id);
    } catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }

});

// read all
app.MapGet("api/courses", async (ICourseService service, CancellationToken ct) =>
{
    var courses = await service.GetAllCoursesAsync(ct);
    return Results.Ok(courses);

});

// read by id
app.MapGet("api/courses/{id:guid}", async (Guid id, ICourseService service, CancellationToken ct) =>
{
    var course = await service.GetByIdAsync(id, ct);
    return course is not null ? Results.Ok(course) : Results.NotFound();
});


// update
app.MapPut("/api/courses/{id:guid}", async (Guid id, [FromBody] UpdateCourseRequest request, ICourseService service, CancellationToken ct) =>
{
    try
    {
        var input = new UpdateCourseInput(request.Id, request.CourseCode, request.CourseName, request.Description, request.RowVersion);
        var result = await service.UpdateAsync(input, ct);

        return result is not null ? Results.Ok(result) : Results.NotFound();

    } catch (ArgumentException ex)

    {
        return Results.BadRequest(ex.Message);

    }

   

});

// delete
app.MapDelete("/api/courses/{id:guid}", async (Guid id, [FromHeader(Name = "If-Match")] string rowVersionStr, ICourseService service, CancellationToken ct) =>
{
    try
    {
        // convert string to byte array
        var rowVersion = Convert.FromBase64String(rowVersionStr);

        await service.DeleteAsync(id, rowVersion, ct);
        return Results.NoContent();

    } catch(ArgumentException ex)

    {
        return Results.NotFound(ex.Message);
    }

});

#endregion

app.Run();

