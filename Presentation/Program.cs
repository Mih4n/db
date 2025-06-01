using App.Lexer;
using App.Processor;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/query", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var query = await reader.ReadToEndAsync();
    var processor = new Processor();
    var lexer = new Lexer(query);
    var parser = new Parser(lexer);

    try
    {
        var result = await processor.ProcessAsync(parser.Parse());
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();
