using Play.Catalog.Service.Entities;
using Play.Common.MongoDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String)); 
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String)); 

var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddSingleton<IMongoDatabase>(sp => {
    var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
    return mongoClient.GetDatabase(serviceSettings.ServiceName);
});

//builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();
builder.Services.AddSingleton<IRepository<Item>>(sp => 
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return  new MongoRepository<Item>(database, "items");
});
*/

builder.Services
    .AddMongo()
    .AddMongoRepository<Item>("items");

builder.Services.AddControllers(options => 
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
