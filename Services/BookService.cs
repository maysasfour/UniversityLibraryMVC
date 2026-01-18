using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext _context;

        public BookService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            try
            {
                return await _context.Books.ToListAsync();
            }
            catch (Exception ex)
            {
          
                Console.WriteLine($"Error in GetAllBooksAsync: {ex.Message}");
                return new List<Book>();
            }
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            try
            {
                return await _context.Books.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBookByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AddBookAsync(Book book)
        {
            try
            {
                _context.Books.Add(book);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBookAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            try
            {
                _context.Books.Update(book);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateBookAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    var result = await _context.SaveChangesAsync();
                    return result > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteBookAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Book>> SearchBooksAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrEmpty(searchTerm))
                    return await _context.Books.ToListAsync();

                return await _context.Books
                    .Where(b => b.Title.Contains(searchTerm) ||
                               b.Author.Contains(searchTerm) ||
                               b.ISBN.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchBooksAsync: {ex.Message}");
                return new List<Book>();
            }
        }
    }
}