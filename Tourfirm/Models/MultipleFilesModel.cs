using System.ComponentModel.DataAnnotations;
using Tourfirm.Models;

namespace Tourfirm.DAL.ViewModels;

public class MultipleFilesModel : ResponseModel
{
    [Required(ErrorMessage = "Please select files")]
    public List<IFormFile> Files { get; set; }
}