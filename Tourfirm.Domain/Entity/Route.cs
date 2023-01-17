using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class Route
{
    [Key]
    public int Id { get; set; }
    public string? StartPos { get; set; }
    public string? EndPost { get; set; }
    public double Hours { get; set; }
    public string? Type { get; set; }

    public List<Tour> Tours { get; set; } = new();
    
    public enum SortState
    {
        IdAsc,
        IdDesc,
        StartAsc,
        StartDesc
    }
}