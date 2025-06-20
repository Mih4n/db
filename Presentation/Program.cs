using App.Lexer;
using App.Parser;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/query", async (HttpRequest request) =>
{
    var lexer = new Lexer();
    var parser = new Parser();

    using var reader = new StreamReader(request.Body);
    var query = await reader.ReadToEndAsync();
    var tokens = lexer.Tokenize(query);
    var expression = parser.Parse(tokens);
    return Results.Ok(expression);
    // using var reader = new StreamReader(request.Body);
    // var query = await reader.ReadToEndAsync();
    // var processor = new Processor();
    // var lexer = new Lexer(query);
    // var parser = new Parser(lexer);

    // try
    // {
    //     var result = await processor.ProcessAsync(parser.Parse());
    //     return Results.Ok(result);
    // }
    // catch (Exception ex)
    // {
    //     return Results.BadRequest(ex.Message);
    // }
});

app.Run();
