using System.ComponentModel.DataAnnotations;

namespace Tourfirm.Domain.Entity;

public class Country
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set;}
    public string? Language { get; set; }
    public string? Climate { get; set; }
    public double MidTemp { get; set; }
    
    public List<Tour> Tours { get; set; } = new();
    
    public enum SortState
    {
        IdAsc,
        IdDesc,
        ClimateAsc,
        ClimateDesc, 
        LanguageAsc,
        LanguageDesc, 
        MidTempAsc,
        MidTempDesc,
        NameAsc,
        NameDesc
    }
}