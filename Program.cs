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
books.MapGet("/{id}", GetBook);
books.MapPost("/", CreateBook);
books.MapPut("/{id}", UpdateBook);
books.MapDelete("/{id}", DeleteBook);

// author routes
RouteGroupBuilder authors = app.MapGroup("/authors");
authors.MapGet("/", GetAllAuthors);
authors.MapGet("/{id}", GetAuthor);
authors.MapPost("/", CreateAuthor);
authors.MapPut("/{id}", UpdateAuthor);
authors.MapDelete("/{id}", DeleteAuthor);

// categories routes
RouteGroupBuilder categories = app.MapGroup("/categories");
categories.MapGet("/", GetAllCategories);
categories.MapGet("/{id}", GetCategory);
categories.MapPost("/", CreateCategory);
categories.MapPut("/{id}", UpdateCategory);
categories.MapDelete("/{id}", DeleteCategory);

app.Run();

// get all books
static async Task<IResult> GetAllBooks(BookStoreDb db)
{
    return TypedResults.Ok(
        await db.Books
        .Include(x => x.Author)
        .Include(x => x.Category)
        .ToArrayAsync());
}

// get book by ID
static async Task<IResult> GetBook(int id, BookStoreDb db)
{
    var book = await db.Books
        .Include(x => x.Author)
        .Include(x => x.Category)
        .FirstOrDefaultAsync(x => x.Id == id);

    return book is not null
        ? TypedResults.Ok(book)
        : TypedResults.NotFound();
}

// create a book
static async Task<IResult> CreateBook(Book book, BookStoreDb db)
{
    await db.Books.AddAsync(book);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/shows/{book.Id}", book);
}

// update a book
static async Task<IResult> UpdateBook(int id, Book book, BookStoreDb db)
{

    var foundBook = await db.Books.FindAsync(id);

    if (foundBook is null) return TypedResults.NotFound();

    foundBook.Title = book.Title;
    foundBook.Description = book.Description;
    foundBook.AuthorId = book.AuthorId;
    foundBook.Author = book.Author;
    foundBook.CategoryId = book.CategoryId;
    foundBook.Category = book.Category;    

    await db.SaveChangesAsync();
    return TypedResults.Ok(foundBook);
}

// delete a book
static async Task<IResult> DeleteBook(int id, BookStoreDb db)
{
    if (await db.Books.FindAsync(id) is Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NoContent();
}

// get all authors
static async Task<IResult> GetAllAuthors(BookStoreDb db)
{
    return TypedResults.Ok(
        await db.Authors
        .ToArrayAsync());
}

// get author by id
static async Task<IResult> GetAuthor(int id, BookStoreDb db)
{
    var author = await db.Authors.FindAsync(id);

    return author is not null
        ? TypedResults.Ok(author)
        : TypedResults.NotFound();
}

// create an author
static async Task<IResult> CreateAuthor(Author author, BookStoreDb db)
{
    await db.Authors.AddAsync(author);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/authors/{author.Id}", author);
}

// update an author
static async Task<IResult> UpdateAuthor(int id, Author author, BookStoreDb db)
{

    var foundAuthor = await db.Authors.FindAsync(id);

    if (foundAuthor is null) return TypedResults.NotFound();

    foundAuthor.Name = author.Name;

    await db.SaveChangesAsync();
    return TypedResults.Ok(foundAuthor);
}

// delete an author
static async Task<IResult> DeleteAuthor(int id, BookStoreDb db)
{
    if (await db.Authors.FindAsync(id) is Author author)
    {
        db.Authors.Remove(author);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NoContent();
}

// get all categories
static async Task<IResult> GetAllCategories(BookStoreDb db)
{
    return TypedResults.Ok(
        await db.Categories
        .ToArrayAsync());
}

// get category by id
static async Task<IResult> GetCategory(int id, BookStoreDb db)
{
    var category = await db.Categories.FindAsync(id);

    return category is not null
        ? TypedResults.Ok(category)
        : TypedResults.NotFound();
}

// create a category
static async Task<IResult> CreateCategory(Category category, BookStoreDb db)
{
    await db.Categories.AddAsync(category);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/categories/{category.Id}", category);
}

// update a category
static async Task<IResult> UpdateCategory(int id, Category category, BookStoreDb db)
{

    var foundCategory = await db.Categories.FindAsync(id);

    if (foundCategory is null) return TypedResults.NotFound();

    foundCategory.Name = category.Name;

    await db.SaveChangesAsync();
    return TypedResults.Ok(foundCategory);
}

// delete a category
static async Task<IResult> DeleteCategory(int id, BookStoreDb db)
{
    if (await db.Categories.FindAsync(id) is Category category)
    {
        db.Categories.Remove(category);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NoContent();
}





