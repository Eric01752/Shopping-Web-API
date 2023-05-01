using ShoppingApp.Models;

namespace ShoppingApp.Interfaces
{
    public interface IBrandRepository
    {
        IEnumerable<Brand> GetBrands();
        Brand GetBrand(int id);
        IEnumerable<Product> GetProductsByBrandId(int brandId);
        bool BrandExists(int id);
        bool CreateBrand(Brand brand);
        bool UpdateBrand(Brand brand);
        bool DeleteBrand(Brand brand);
        bool Save();
    }
}
