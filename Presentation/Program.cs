using App;
using App.Lexer;
using App.Processor;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/query", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var query = await reader.ReadToEndAsync();
    var processor = new Processor();
    var parser = new Parser(new Lexer(query));
    var result = processor.Process(parser.Parse());
    return Results.Ok(result);
});

app.Run();
