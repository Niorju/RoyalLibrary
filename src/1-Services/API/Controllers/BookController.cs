using Application.Interfaces;
using CrossCutting.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.NETCore.Controllers
{
    [Produces("application/json", "application/xml")]    
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookApplicationServices _bookApplicationServices;

        public BookController(IBookApplicationServices bookApplicationServices)
        {
            _bookApplicationServices = bookApplicationServices;
        }

        /// <summary>
        /// Get Books
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDTO>> GetBook()
        {
            var response = await _bookApplicationServices.GetBooks();
            if (response.Count == 0) return NotFound();
            return Ok(response);
        }
        /// <summary>
        /// Get Book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBookById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<ResponseDTO>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDTO>> GetBookById(int id)
        {
            ResponseDTO response = await _bookApplicationServices.GetBookById(id);
            if (response.Book == null) return NotFound(response.Message);
            if (response.Error.Count > 0) return BadRequest(response.Error);
            return Ok(response);
        }

        /// <summary>
        /// Get book by Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBookByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<ResponseDTO>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDTO>> GetBookByCategory(string category)
        {
            ResponseDTO response = await _bookApplicationServices.GetBookByCategory(category);
            if (response.BookList.Count <= 0) return NotFound(response.Message);
            if (response.Error.Count > 0) return BadRequest(response.Error);
            return Ok(response);
        }

        /// <summary>
        /// Update Book
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("PutBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseDTO>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDTO>> PutBook([FromBody] BookDTO dto)
        {
            ResponseDTO response = await _bookApplicationServices.UpdateBook(dto);
            if (response.Error.Count > 0) return BadRequest(response.Error);
            return Ok(response);
        }

        /// <summary>
        /// Insert Book
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseDTO>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDTO>> PostBook([FromBody] BookDTO dto)
        {
            ResponseDTO response = await _bookApplicationServices.InsertBook(dto);
            if (response.Error.Count > 0) return BadRequest(response.Error);
            return Ok(response);
        }
    }
}