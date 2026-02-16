using Application.Abstractions.Persistence;
using Application.Modules.Courses;
using Application.Modules.Courses.Input;
using Application.Modules.CourseSessions;
using Application.Modules.CourseSessions.Input;
using Application.Modules.Locations;
using Application.Modules.Locations.Input;
using Application.Modules.Participants;
using Application.Modules.Participants.Inputs;
using Application.Modules.Registrations;
using Application.Modules.Registrations.Input;
using Application.Modules.Roles;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Dtos;
using Presentation.Dtos.Course;
using Presentation.Dtos.CourseSession;
using Presentation.Dtos.Location;
using Presentation.Dtos.Registration;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


builder.Services.AddScoped<IParticipantRepository, ParticipantEntityRepository>();
builder.Services.AddScoped<IUnitOfWork, EfcUnitOfWork>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<ICourseSessionRepository, CourseSessionRepository>();
builder.Services.AddScoped<ICourseSessionService, CourseSessionService>();

builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();


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

roleGroup.MapGet("/", async (IRoleService service, CancellationToken ct) =>
{
    var roles = await service.GetRolesAsync(ct);
    return Results.Ok(roles);
});

#endregion

#region participant endpoints

var participantGroup = app.MapGroup("/participants").WithTags("Participants");

// create
participantGroup.MapPost("/", async (CreateParticipantRequest request, IParticipantService service, CancellationToken ct)  =>
{
    //map (create) a dto from user input 

    var input = new CreateParticipantInput(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.RoleId);

    var id = await service.CreateAsync(input, ct);

    return Results.Created($"/api/participants/{id}", id);

});

// read all
participantGroup.MapGet("/", async (IParticipantService service, CancellationToken ct) =>
{
   var participants =await service.GetAllParticipantsAsync(ct);
   return Results.Ok(participants);

});

// read by id
participantGroup.MapGet("/{id:guid}", async (Guid id, IParticipantService service, CancellationToken ct) =>
{
    var participant = await service.GetByIdAsync(id, ct);
    return participant is not null ? Results.Ok(participant) : Results.NotFound();
});

// update

participantGroup.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateParticipantRequest request, IParticipantService service, CancellationToken ct) =>
{
    var input = new UpdateParticipantInput(request.Id, request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.RoleId, request.RowVersion);

   
        var result = await service.UpdateAsync(input, ct);
        return result is not null ? Results.Ok(result) : Results.NotFound();

  
});

// delete
participantGroup.MapDelete("/{id:guid}", async (Guid id, [FromHeader(Name = "If-Match")] string rowVersionStr, IParticipantService service, CancellationToken ct) =>
{
    // convert string to byte array
    var rowVersion = Convert.FromBase64String(rowVersionStr);

    await service.DeleteAsync(id, rowVersion, ct);
    return Results.NoContent();
});

#endregion

#region course endpoints


var courseGroup = app.MapGroup("/api/courses").WithTags("Courses");


courseGroup.MapPost("/", async (CreateCourseRequest request, ICourseService service, CancellationToken ct) =>
{
    
        var input = new CreateCourseInput(request.CourseCode, request.CourseName, request.Description);
        var id = await service.CreateAsync(input, ct);
        return Results.Created($"/api/courses/{id}", id);
    
});

courseGroup.MapGet("/", async (ICourseService service, CancellationToken ct) =>
{
    var courses = await service.GetAllCoursesAsync(ct);
    return Results.Ok(courses);
});

courseGroup.MapGet("/{id:guid}", async (Guid id, ICourseService service, CancellationToken ct) =>
{
    var course = await service.GetByIdAsync(id, ct);
    return course is not null ? Results.Ok(course) : Results.NotFound();
});

courseGroup.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateCourseRequest request, ICourseService service, CancellationToken ct) =>
{
    
    if (id != request.Id) return Results.BadRequest("ID mismatch.");

    
        var input = new UpdateCourseInput(request.Id, request.CourseCode, request.CourseName, request.Description, request.RowVersion);
        var result = await service.UpdateAsync(input, ct);
        return result is not null ? Results.Ok(result) : Results.NotFound();
    
    
});

courseGroup.MapDelete("/{id:guid}", async (Guid id, [FromHeader(Name = "If-Match")] string rowVersionStr, ICourseService service, CancellationToken ct) =>
{
    
        var rowVersion = Convert.FromBase64String(rowVersionStr);
        await service.DeleteAsync(id, rowVersion, ct);
        return Results.NoContent();
   
});

#endregion

#region location endpoints


var locationGroup = app.MapGroup("/api/locations").WithTags("Location");

locationGroup.MapPost("/", async (CreateLocationRequest request, ILocationService service, CancellationToken ct) =>
{
    
        var input = new CreateLocationInput(request.Name);
        var id = await service.CreateAsync(input, ct);

        
        return Results.Created($"/api/locations/{id}", id);
    
});

locationGroup.MapGet("/", async (ILocationService service, CancellationToken ct) =>
{
    var locations = await service.GetAllAsync(ct);
    return Results.Ok(locations);

});

locationGroup.MapGet("/{id:guid}", async (Guid id, ILocationService service, CancellationToken ct) =>
{

    var location = await service.GetByIdAsync(id, ct);
    return location is not null ? Results.Ok(location) : Results.NotFound();

});


locationGroup.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateLocationRequest request, ILocationService service, CancellationToken ct) =>
{

    if (id != request.Id) return Results.BadRequest("ID mismatch.");

   
        var input = new UpdateLocationInput(request.Id, request.Name, request.RowVersion);
        var result = await service.UpdateAsync(input, ct);
        return result is not null ? Results.Ok(result) : Results.NotFound();
    
   

});

locationGroup.MapDelete("/{id:guid}", async (Guid id, [FromHeader(Name = "If-Match")] string rowVersionStr, ILocationService service, CancellationToken ct) =>
{
   
        var rowVersion = Convert.FromBase64String(rowVersionStr);
        await service.DeleteAsync(id, rowVersion, ct);
        return Results.NoContent();
   
});


#endregion

#region course session endpoints

var courseSessionGroup = app.MapGroup("/api/courseSessions").WithTags("CourseSessions");


courseSessionGroup.MapPost("/", async (CreateCourseSessionRequest request, ICourseSessionService service, CancellationToken ct) =>
{

    var input = new CreateCourseSessionInput(request.CourseId, request.LocationId, request.StartDate, request.EndDate, request.Capacity);
    var id = await service.CreateAsync(input, ct);
    return Results.Created($"/api/courseSessions/{id}", id);
 
});

courseSessionGroup.MapGet("/", async (ICourseSessionService service, CancellationToken ct) =>
{
    var courseSessions = await service.GetAllCourseSessionsAsync(ct);
    return Results.Ok(courseSessions);
});

courseSessionGroup.MapGet("/{id:guid}", async (Guid id, ICourseSessionService service, CancellationToken ct) =>
{
    var courseSession = await service.GetByIdAsync(id, ct);
    return courseSession is not null ? Results.Ok(courseSession) : Results.NotFound();
});

courseSessionGroup.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateCourseSessionRequest request, ICourseSessionService service, CancellationToken ct) =>
{

    if (id != request.Id) return Results.BadRequest("ID mismatch.");

    
    
     var input = new UpdateCourseSessionInput(request.Id, request.CourseId, request.LocationId, request.StartDate, request.EndDate, request.Capacity, request.RowVersion);
     var result = await service.UpdateAsync(input, ct);
     return result is not null ? Results.Ok(result) : Results.NotFound();
    
    
});

courseSessionGroup.MapDelete("/{id:guid}", async (Guid id, [FromHeader(Name = "If-Match")] string rowVersionStr, ICourseSessionService service, CancellationToken ct) =>
{
    
   var rowVersion = Convert.FromBase64String(rowVersionStr);
   await service.DeleteAsync(id, rowVersion, ct);
   return Results.NoContent();
   
});

#endregion

#region registration endpoints

var registrationGroup = app.MapGroup("/api/registrations").WithTags("Registrations");

// create

registrationGroup.MapPost("/", async (CreateRegistrationRequest request, IRegistrationService service, CancellationToken ct) =>
{
    
    var input = new CreateRegistrationInput(request.ParticipantId, request.CourseSessionId);
    var id = await service.CreateAsync(input, ct);

    return Results.Created($"/api/registrations/{id}", id);
});

// read by id 

registrationGroup.MapGet("/{id:guid}", async (Guid id, IRegistrationService service, CancellationToken ct) =>
{
    var result = await service.GetByIdAsync(id, ct);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

// read all 

registrationGroup.MapGet("/", async (IRegistrationService service, CancellationToken ct) =>
{
    var registrations = await service.GetAllAsync(ct);
    return Results.Ok(registrations);

});

//update

registrationGroup.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateRegistrationRequest request, IRegistrationService service, CancellationToken ct) =>
{
    var input = new UpdateRegistrationInput(request.Id, request.Status, request.RowVersion);


    var result = await service.UpdateAsync(input, ct);
    return result is not null ? Results.Ok(result) : Results.NotFound();


});


registrationGroup.MapDelete("/{id:guid}", async (Guid id, [FromBody] byte[] rowVersion, IRegistrationService service, CancellationToken ct) =>
{
    await service.DeleteAsync(id, rowVersion, ct);
    return Results.NoContent();
});

#endregion

app.Run();

