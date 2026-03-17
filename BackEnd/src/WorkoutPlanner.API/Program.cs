using Microsoft.EntityFrameworkCore;
using WorkoutPlanner.Application.Evaluations;
using WorkoutPlanner.Application.Excercises;
using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Application.Services.HasherService;
using WorkoutPlanner.Application.Services.WourkoutProcessorService;
using WorkoutPlanner.Application.Users;
using WorkoutPlanner.Application.WorkoutExcercises;
using WorkoutPlanner.Application.Workouts;
using WorkoutPlanner.Infraestructure.Persistence;
using WorkoutPlanner.Infraestructure.Repositories;
using WorkoutPlanner.Infrastructure.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);

const string FrontendCorsPolicy = "FrontendCorsPolicy";

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEvaluationLogic, EvaluationLogic>();
builder.Services.AddScoped<IExcerciseLogic, ExcerciseLogic>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<IWorkoutLogic, WorkoutLogic>();
builder.Services.AddScoped<IWorkoutExcerciseLogic, WorkoutExcerciseLogic>();
builder.Services.AddScoped<IWorkoutProcessorService, WorkoutProcessorService>();

builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();
builder.Services.AddScoped<IExcerciseRepository, ExcerciseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<IWorkoutExcerciseRepository, WorkoutExcerciseRepository>();

builder.Services.AddScoped<IHasherService, HasherService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await context.Database.MigrateAsync();

    await UserSeeder.SeedAsync(context);
    await ExerciseSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(FrontendCorsPolicy);
app.MapControllers();

app.Run();
