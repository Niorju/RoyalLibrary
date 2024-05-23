using Newtonsoft.Json;

namespace CrossCutting.DTO
{
    public class ResponseDTO
    {
        public ResponseDTO()
        {
            Error = [];
        }

        /// <summary>
        /// Mensagem de retorno
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Book List
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<BookDTO> BookList { get; set; }
        
        /// <summary>
        /// Book By Id
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BookDTO Book { get; set; }               

        /// <summary>  
        ///  Error List
        /// </summary>                
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ErrorResponse> Error { get; set; }
    }

    public class ErrorResponse
    {
        /// <summary>
        /// Origem do Erro
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }
        /// <summary>  
        ///  Código do erro
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Error_code { get; set; }
        /// <summary>  
        ///  Descrição do erro
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Error_description { get; set; }
        /// <summary>  
        ///  Inner Exception
        /// </summary>  
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Inner_exception { get; set; }
    }

}
