using ProductApi.Models;

namespace ProductApi.Repository
{
    public interface IProductRepository
    {
        public IEnumerable<Product> GetAllProducts();
        public bool AddProduct(Product product);
        public bool UpdateProduct(int id, Product product);
        public bool DeleteProduct(int id);
    }
}
