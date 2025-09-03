using BookStore.Models;
using BookStore.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BookStore") ?? "Data Source=BookStore.db";
builder.Services.AddSqlite<BookStoreDb>(connectionString);

var app = builder.Build();

// book routes
RouteGroupBuilder books = app.MapGroup("/books");
books.MapGet("/", GetAllBooks);
books.MapPost("/", CreateBook);

app.Run();

// get all books
static async Task<IResult> GetAllBooks(BookStoreDb db)
{
    return TypedResults.Ok(
        await db.Books
        .Include(x => x.Author)
        .ToArrayAsync());
}

// create a book
static async Task<IResult> CreateBook(Book book, BookStoreDb db)
{
    await db.Books.AddAsync(book);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/shows/{book.Id}", book);
}



