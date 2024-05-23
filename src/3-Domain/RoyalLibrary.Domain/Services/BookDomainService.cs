
using CrossCutting.DTO;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using RoyalLibrary.Domain.Entities;
using RoyalLibrary.Domain.Entities.Validations;

namespace Domain.Services
{
    public class BookDomainService : IBookDomainServices
    {
        private readonly IBookRepositorie _repo;
        

        public BookDomainService(IBookRepositorie bookRepositorie)
        {
            _repo = bookRepositorie;
        }        

        public async Task<List<Book>> GetBooks()
        {
            try
            {
                return await _repo.GetBooks();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<Book> GetBookById(int id)
        {
            try
            {
                return await _repo.GetBookById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Book>> GetBookByCategory(string category)
        {
            try
            {
                return await _repo.GetBookByCategory(category);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ResponseDTO> InsertBookAsync(Book entitie)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {                
                var repoResponse = await _repo.InsertBookAsync(entitie);
                response.Message = string.Format(ValidationMessages.InsertBook, repoResponse);
                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ResponseDTO> UpdateBookAsync(Book entitie)
        {
            ResponseDTO response = new ResponseDTO();

            try
            {                   
                string repoResponse = await _repo.UpdateBookAsync(entitie);
                response.Message = string.Format(ValidationMessages.BookUpdated, repoResponse);                

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
