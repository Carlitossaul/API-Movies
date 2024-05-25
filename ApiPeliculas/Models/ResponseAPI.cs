using System.Net;

namespace ApiPeliculas.Models
{
    public class ResponseAPI
    {
        public ResponseAPI()
        {
                ErrorMessages = new List<string>();

        }
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
