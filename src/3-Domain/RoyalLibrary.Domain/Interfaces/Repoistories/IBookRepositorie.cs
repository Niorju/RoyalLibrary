
using RoyalLibrary.Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IBookRepositorie
    {
        /// <summary>
        /// Get book List
        /// </summary>
        /// <returns></returns>
        Task<List<Book>> GetBooks();
        /// <summary>
        /// Get Book by Category
        /// </summary>
        /// <param name="category">NomeOf Category</param>
        /// <returns></returns>
        Task<List<Book>> GetBookByCategory(string category);
        /// <summary>
        /// Get book by Id
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <returns></returns>
        Task<Book> GetBookById(int id);
        /// <summary>
        /// Update Book
        /// </summary>
        /// <param name="entitie">Book entitie</param>
        /// <returns></returns>
        Task<string> UpdateBookAsync(Book entitie);
        /// <summary>
        /// Insert new book
        /// </summary>
        /// <param name="entitie">Book entitie</param>
        /// <returns></returns>
        Task<string> InsertBookAsync(Book entitie);        
    }
}
