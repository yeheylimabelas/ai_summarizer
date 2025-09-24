var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient("ollama", client =>
{
    // Ambil dari ENV (default ke localhost kalau tidak ada)
    var ollamaUrl = builder.Configuration["OLLAMA_API"] ?? "http://localhost:11434";
    client.BaseAddress = new Uri(ollamaUrl);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();