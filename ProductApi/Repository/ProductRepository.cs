using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.Repository
{

        public class ProductRepository : IProductRepository
        {
            ProductDbContext _context;
            public ProductRepository(ProductDbContext context)
            {
                _context = context;
            }
            public bool AddProduct(Product product)
            {
                if (product == null) { return false; }
                _context.products.Add(product);
                _context.SaveChanges();
                return true;
            }

            public bool DeleteProduct(int id)
            {
                var isProductExist = _context.products.FirstOrDefault(p => p.Id == id);
                if (isProductExist == null) { return false; }
                _context.products.Remove(isProductExist);
                _context.SaveChanges();
                return true;
            }

            public IEnumerable<Product> GetAllProducts()
            {
                return _context.products.ToList();
            }

            public bool UpdateProduct(int id, Product product)
            {
                var isProductExist = _context.products.FirstOrDefault(product => product.Id == id);
                if (isProductExist != null)
                {
                    isProductExist.Price = product.Price;
                    isProductExist.Name = product.Name;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    
}
