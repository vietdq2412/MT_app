using Microsoft.AspNetCore.Http;

namespace MT_app.core.ViewModel;

public class ProductViewModel
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public IFormFile? ImageFile { get; set; }
    public long[]? CategoryIds { get; set; }
    public long? SupplierId { get; set; }
}