using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using CourseAPI.Domains.Interfaces;
using CourseAPI.Domains.Models;
using CourseAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);



builder.Services.AddDefaultAWSOptions(new Amazon.Extensions.NETCore.Setup.AWSOptions() { Profile = "Nalme", Region = Amazon.RegionEndpoint.APSouth1 });
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddTransient<ICourseService, CourseService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.MapGet("/courses",async (ICourseService service) => await service.GetAllCourses());
app.MapGet("/courses/{id}", async (string id, ICourseService service) => {
    var course = await service.GetCourseById(id);
    if(course == null)
    {
       
        return Results.NotFound();
    }
    return Results.Ok(course);
});
app.MapPost("/courses", async (CourseParam crs, ICourseService service) => {
    var isNewCourse = await service.SaveCourse(crs);
    if (!isNewCourse)
    {
        return Results.BadRequest();
    }
    return Results.Created("/courses", true);
});
app.MapPut("/courses", async (CourseParam crs, ICourseService service) => {
    var updatedCourse = await service.UpdateCourse(crs);
    if(updatedCourse == null)
    {
        return Results.BadRequest();
    }
    return Results.Created("/courses" ,updatedCourse);
});
app.MapDelete("/courses/{id}", async (string id, ICourseService service) =>
{
    var isCourseExist = await service.DeleteCourseById(id);
    if (!isCourseExist)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.NoContent();
    }
});



//app.UseAuthorization();

//app.MapControllers();

app.Run();
