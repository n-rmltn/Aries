using Microsoft.AspNetCore.Mvc;

namespace Aries.Models;

public class DataTablesRequest
{
    [FromQuery(Name = "draw")]
    public int Draw { get; set; }
    
    [FromQuery(Name = "start")]
    public int Start { get; set; }
    
    [FromQuery(Name = "length")]
    public int Length { get; set; }
    
    public Search Search { get; set; } = new();
}

public class Search
{
    [FromQuery(Name = "search[value]")]
    public string Value { get; set; } = string.Empty;
    
    [FromQuery(Name = "search[regex]")]
    public bool Regex { get; set; }
}

public class DataTablesResponse<T>
{
    public int Draw { get; set; }
    public int RecordsTotal { get; set; }
    public int RecordsFiltered { get; set; }
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
}