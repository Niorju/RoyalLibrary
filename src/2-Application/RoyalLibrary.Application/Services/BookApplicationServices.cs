

using Application.Interfaces;
using AutoMapper;
using CrossCutting.Commons;
using CrossCutting.DTO;
using Domain.Interfaces.Services;
using Domain.Services.Common;
using Domain.Validation;
using RoyalLibrary.Domain.Entities;

namespace Application.Services
{
    public class BookApplicationServices : IBookApplicationServices
    {
        private readonly IMapper _mapper;
        private readonly IBookDomainServices _bookDomainServices;
        private ValidationResult _validationResult { get; set; }

        public BookApplicationServices(IMapper mapper, IBookDomainServices bookDomainServices)
        {
            _mapper = mapper;
            _bookDomainServices = bookDomainServices;
        }

        public async Task<List<BookDTO>> GetBooks()
        {
            var response = new List<BookDTO>();
            
            try
            {
                var responseDomain = await _bookDomainServices.GetBooks();
                var booksDTO = _mapper.Map<IList<BookDTO>>(responseDomain);
                response = booksDTO.ToList();

                if (response.Count <= 0)
                    response.Add(new BookDTO() { Message = "No books avaliable" }); 
            }
            catch (Exception ex)
            {
                ErrorResponse erro =  new ErrorResponse()
                {
                    Error_code = ex.HResult,
                    Error_description = "DESCRIPTION: " + ex.Message?.ToString() + "\nSTACK-TRACE: " + ex.StackTrace?.ToString(),
                    Inner_exception = ex.InnerException?.ToString()
                };
                response.Add(new BookDTO() { 
                    Message = "An error occurred in your request.",
                    Error = erro
                });
            }
            return response;
        }

        public async Task<ResponseDTO> GetBookById(int id)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                var responseDomain = await _bookDomainServices.GetBookById(id);
                var bookDTO = _mapper.Map<BookDTO>(responseDomain);
                response.Book = bookDTO;
                if (response.Book == null)
                    response.Message = "No books avaliable";

            }
            catch (Exception ex)
            {

                response.Message = "An error occurred in your request.";
                response.Error.Add(new ErrorResponse()
                {
                    Error_code = ex.HResult,
                    Error_description = "DESCRIPTION: " + ex.Message?.ToString() + "\nSTACK-TRACE: " + ex.StackTrace?.ToString(),
                    Inner_exception = ex.InnerException?.ToString()
                });
            }
            return response;
        }

        public async Task<ResponseDTO> GetBookByCategory(string category)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                var responseDomain = await _bookDomainServices.GetBookByCategory(category);
                var bookDTO = _mapper.Map<IList<BookDTO>>(responseDomain);
                response.BookList = bookDTO.ToList();
                if (response.BookList.Count <= 0)
                    response.Message = "No books avaliable";
            }
            catch (Exception ex)
            {

                response.Message = "An error occurred in your request.";
                response.Error.Add(new ErrorResponse()
                {
                    Error_code = ex.HResult,
                    Error_description = "DESCRIPTION: " + ex.Message?.ToString() + "\nSTACK-TRACE: " + ex.StackTrace?.ToString(),
                    Inner_exception = ex.InnerException?.ToString()
                });
            }
            return response;
        }
        public async Task<ResponseDTO> UpdateBook(BookDTO entitie)
        {
            var response = new ResponseDTO();
            try
            {
                var bookEntitie = _mapper.Map<Book>(entitie);
                var checkEntitie = new Service<Book>();
                _validationResult = checkEntitie.InsertUpdate(bookEntitie);

                if (!_validationResult.IsValid)
                {
                    foreach (ValidationError item in _validationResult.Errors)
                    {
                        response.Error.Add(new ErrorResponse()
                        {
                            Error_code = Convert.ToInt32(ErrorCodeEnum.ERROR_INVALID_FIELD_IN_PARAMETER_LIST),
                            Error_description = item.Message
                        });
                    }

                    return response;
                }

                response = await _bookDomainServices.UpdateBookAsync(bookEntitie);
            }
            catch (Exception ex)
            {

                response.Message = "An error occurred in your request.";
                response.Error.Add(new ErrorResponse()
                {
                    Error_code = ex.HResult,
                    Error_description = "DESCRIPTION: " + ex.Message?.ToString() + "\nSTACK-TRACE: " + ex.StackTrace?.ToString(),
                    Inner_exception = ex.InnerException?.ToString()
                });
            }
            return response;
        }

        public async Task<ResponseDTO> InsertBook(BookDTO entitie)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                var bookEntitie = _mapper.Map<Book>(entitie);
                var checkEntitie = new Service<Book>();
                _validationResult = checkEntitie.InsertUpdate(bookEntitie);

                if (!_validationResult.IsValid)
                {
                    foreach (ValidationError item in _validationResult.Errors)
                    {
                        response.Error.Add(new ErrorResponse()
                        {
                            Error_code = Convert.ToInt32(ErrorCodeEnum.ERROR_INVALID_FIELD_IN_PARAMETER_LIST),
                            Error_description = item.Message
                        });
                    }

                    return response;
                }

                response = await _bookDomainServices.InsertBookAsync(bookEntitie);

            }
            catch (Exception ex)
            {

                response.Message = "An error occurred in your request.";
                response.Error.Add(new ErrorResponse()
                {
                    Error_code = ex.HResult,
                    Error_description = "DESCRIPTION: " + ex.Message?.ToString() + "\nSTACK-TRACE: " + ex.StackTrace?.ToString(),
                    Inner_exception = ex.InnerException?.ToString()
                });
            }

            return response;
        }        
    }
}
