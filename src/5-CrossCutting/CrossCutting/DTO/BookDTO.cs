
using Newtonsoft.Json;

namespace CrossCutting.DTO
{
    public class BookDTO
    {   
        public BookDTO()
        {
            Error = new ErrorResponse();
        }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalCopies { get; set; } = 0;
        public int CopiesInUse { get; set; } = 0;
        public string Type { get; set; }
        public string Isbn { get; set; }
        public string Category { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorResponse Error { get; set; }      

    }
}
