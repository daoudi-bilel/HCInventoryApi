namespace ITInventoryManagementAPI.Models.Responses
{
    public class PagedResponse<T>
{
    public IEnumerable<T> Content { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
}