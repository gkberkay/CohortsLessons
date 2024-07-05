using Restful.API.Models;

namespace Restful.API.Services
{
    public class ProductService
    {
        private List<Product> _products = new List<Product>();

        public ProductService()
        {
            _products.Add(new Product { Id = 1, Name = "Default Product 1", Description = "This is the first default product", Price = 20.0m });
            _products.Add(new Product { Id = 2, Name = "Default Product 2", Description = "This is the second default product", Price = 10.0m });
        }

        public IEnumerable<Product> GetAll() => _products;
        public Product GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
        public IEnumerable<Product> GetByCondition(Func<Product, bool> predicate) => _products.Where(predicate);

        public void Add(Product product)
        {
            product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var existingProduct = GetById(product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
            }
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
