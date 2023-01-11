using System.ComponentModel.DataAnnotations;

namespace Tourfirm.DAL.ViewModels;

public class MultipleFilesModel
{
    [Required(ErrorMessage = "Please select files")]
    public List<IFormFile> Files { get; set; }
}