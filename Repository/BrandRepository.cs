using ShoppingApp.Interfaces;
using ShoppingApp.Models;

namespace ShoppingApp.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDBContext _context;

        public BrandRepository(AppDBContext context)
        {
            _context = context;
        }

        public bool BrandExists(int id)
        {
            return _context.Brands.Any(b => b.Id == id);
        }

        public bool CreateBrand(Brand brand)
        {
            _context.Add(brand);
            return Save();
        }

        public bool DeleteBrand(Brand brand)
        {
            _context.Remove(brand);
            return Save();
        }

        public Brand GetBrand(int id)
        {
            return _context.Brands.FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _context.Brands.OrderBy(b => b.Id).ToList();
        }

        public IEnumerable<Product> GetProductsByBrandId(int brandId)
        {
            return _context.Products.Where(p => p.Brand.Id == brandId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateBrand(Brand brand)
        {
            _context.Update(brand);
            return Save();
        }
    }
}
