
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
//Database
builder.Services.AddDbContext<CarDb>(opt => opt.UseInMemoryDatabase("CarList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "CarAPI";
    config.Title = "CarAPI v1";
    config.Version = "v1";
});

var app = builder.Build();
// do thing if in Development environment:
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "CarAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

// GroupMapping API
var carItems = app.MapGroup("/caritems");

carItems.MapGet("/", GetAllCars);
carItems.MapGet("/reserved", GetReservedCars);
carItems.MapGet("/{id}", GetCarById);
carItems.MapPost("/", CreateNewCar);
carItems.MapPut("/{id}", UpdateCar);
carItems.MapDelete("/{id}",RemoveCarById);

static async Task<IResult> GetAllCars(CarDb db)
    => TypedResults.Ok(await db.Cars.ToArrayAsync());

static async Task<IResult> GetReservedCars(CarDb db)
    => TypedResults.Ok(await db.Cars.Where(c => c.Reserved == true).ToListAsync());

// app.MapGet("/", async (CarDb db) =>
//     await db.Cars.ToListAsync());

// app.MapGet("/reserved", async (CarDb db) =>
//     await db.Cars.Where(c => c.Reserved == true).ToListAsync());


static async Task<IResult> GetCarById(int id, CarDb db)
{
    return await db.Cars.FindAsync(id) 
        is Car car 
        ? TypedResults.Ok(car) 
        : TypedResults.NotFound();
}


// app.MapGet("/{id}", async (int id, CarDb db) =>
// {
//     Car? car = await db.Cars.FindAsync(id);
//     if (car is not null) return  Results.Ok(car);
//     else return Results.NotFound();
// }
// );

static async Task<IResult> CreateNewCar(Car car, CarDb db){
    await db.Cars.AddAsync(car);
    await db.SaveChangesAsync();
    return  TypedResults.Created($"/caritems/{car.Id}", car);
}

// app.MapPost("/", async (Car car, CarDb db) =>
// {
//     db.Cars.Add(car);
//     await db.SaveChangesAsync();

//     return Results.Created($"/caritems/{car.Id}", car);
// } );

static async Task<IResult> UpdateCar(int id, Car inputCar, CarDb db){
    Car car = await db.Cars.FindAsync(id);

    if (car is null) return TypedResults.NotFound($"The specificied car {car} is not found");
    else {
        car.Brand = inputCar.Brand;
        car.Model = inputCar.Model;
        car.Price = inputCar.Price;
        car.Reserved = inputCar.Reserved;
        car.Year = inputCar.Year;
    }
    return TypedResults.Ok("Updates are succesful");
}



// app.MapPut("/{id}", async (int id, Car car, CarDb db) =>
// {
//     Car? searchCar = await db.Cars.FindAsync(id);
//     if (searchCar is null) return Results.NotFound();

//     searchCar.Brand = car.Brand;
//     searchCar.Model = car.Model;
//     searchCar.Price = car.Price;
//     searchCar.Reserved = car.Reserved;
//     searchCar.Year = car.Year;

//     await db.SaveChangesAsync();

//     return Results.NoContent();
// });

static async Task<IResult> RemoveCarById(int id, CarDb db){
    if (await db.Cars.FindAsync(id) is Car car){
        db.Cars.Remove(car);
        return TypedResults.Ok("Removed");
    } return TypedResults.NotFound("No such car found");
}
// app.MapDelete("/{id}", async (int id, CarDb db) => 
// {
//     if (await db.Cars.FindAsync(id) is Car car)
//     {
//         db.Cars.Remove(car);
//         await db.SaveChangesAsync();
//         return Results.NoContent();
//     }return Results.NotFound();
// });

app.Run();
