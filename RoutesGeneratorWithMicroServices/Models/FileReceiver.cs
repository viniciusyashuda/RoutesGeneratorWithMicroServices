using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace RoutesGeneratorWithMicroServices.Models
{
    public class FileReceiver
    {
        public int Id { get; set; }

        [DisplayName("Nome do Arquivo")]
        public string FileName { get; set; }

        [NotMapped]
        [DisplayName("Arquivo")]
        public IFormFile File { get; set; }
    }
}
