

using CrossCutting.DTO;

namespace Application.Interfaces
{
    public interface IBookApplicationServices
    {
        /// <summary>
        /// Get Book List
        /// </summary>
        /// <returns></returns>
        Task<List<BookDTO>> GetBooks();
        /// <summary>
        /// Get book by Category
        /// </summary>
        /// <param name="category">Category</param>
        /// <returns></returns>
        Task<ResponseDTO> GetBookByCategory(string category);
        /// <summary>
        /// Get Book by Id
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <returns></returns>
        Task<ResponseDTO> GetBookById(int id);
        /// <summary>
        /// Update Book
        /// </summary>
        /// <param name="entitie">Book entitie</param>
        /// <returns></returns>
        Task<ResponseDTO> UpdateBook(BookDTO entitie);
        /// <summary>
        /// Insert new Book
        /// </summary>
        /// <param name="entitie">Book entitie</param>
        /// <returns></returns>
        Task<ResponseDTO> InsertBook(BookDTO entitie);        
    }
}
