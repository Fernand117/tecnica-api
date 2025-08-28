using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnicaAPI.Data;
using TecnicaAPI.Models;
using TecnicaAPI.Utils;

namespace TecnicaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookContext context;

        public BookController(BookContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> ListarLibros()
        {
            var libros = await context.Books.ToListAsync();

            if (libros == null || libros.Count == 0)
            {
                return NotFound(new ApiResponse(404, "No hay libros", null));
            }

            return Ok(new ApiResponse(200, "Lista de libros", libros));
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult> GetLibroById(Guid id)
        {
            var libro =  await context.Books.FindAsync(id);

            if (libro == null)
            {
                return NotFound(new ApiResponse(404, "No existe el libro", null));
            }

            return Ok(new ApiResponse(200, "Libro encontrado", libro));
        }

        [HttpPost]
        public async Task<ActionResult> GuardarLibro([FromBody] BookDTO bookDto)
        {
            var libro = new BookEntity
            {
                Id = Guid.NewGuid(),
                Title = bookDto.Title,
                Author = bookDto.Author,
                ISBN = bookDto.ISBN,
                PublishedDate = bookDto.PublishedDate,
                Sumary = bookDto.Sumary
            };

            await context.Books.AddAsync(libro);
            await context.SaveChangesAsync();

            return Ok(new ApiResponse(200, "Libro guardado", libro));
        }

        [HttpPut("/{id}")]
        public async Task<ActionResult> ActualizarLibro(Guid id, [FromBody] BookDTO bookDto)
        {
            var libro = await context.Books.FindAsync(id);
            if (libro == null)
            {
                return NotFound(new ApiResponse(404, "No existe el libro", null));
            }
            libro.Title = bookDto.Title;
            libro.Author = bookDto.Author;
            libro.ISBN = bookDto.ISBN;
            libro.PublishedDate = bookDto.PublishedDate;
            libro.Sumary = bookDto.Sumary;
            context.Books.Update(libro);
            await context.SaveChangesAsync();
            return Ok(new ApiResponse(200, "Libro actualizado", libro));
        }

        [HttpDelete("/{id}")]
        public async Task<ActionResult> EliminarLibro(Guid id)
        {
            var libro = await context.Books.FindAsync(id);
            if (libro == null)
            {
                return NotFound(new ApiResponse(404, "No existe el libro", null));
            }
            context.Books.Remove(libro);
            await context.SaveChangesAsync();
            return Ok(new ApiResponse(200, "Libro eliminado", libro));
        }
    }
}
