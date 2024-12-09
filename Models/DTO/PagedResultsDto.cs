namespace Aries.Models.DTOs;

public class PagedResultDto<T>
{
    public IEnumerable<T> Data { get; set; } = default!;
    public int TotalRecords { get; set; }
    public int FilteredRecords { get; set; }
    public void Deconstruct(out IEnumerable<T> data, out int totalRecords, out int filteredRecords)
    {
        data = Data;
        totalRecords = TotalRecords;
        filteredRecords = FilteredRecords;
    }
    
}