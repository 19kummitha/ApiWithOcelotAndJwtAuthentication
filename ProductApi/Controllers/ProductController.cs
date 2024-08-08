using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Repository;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository _productcontext;
        public ProductController(IProductRepository productContext)
        {
            _productcontext = productContext;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _productcontext.GetAllProducts();
            if (result.Count() == 0)
            {
                return Ok("Database is empty");
            }
            return Ok(result);
        }
        [HttpPost]
        public IActionResult AddDetails([FromBody] Product product)
        {

            var result = _productcontext.AddProduct(product);
            return Ok($"Product is added and success code is {result}");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            var result = _productcontext.DeleteProduct(id);
            if (result)
            {
                return Ok($"Product with id{id} is successfully deleted");
            }
            return NotFound($"Product is with id{id} not found");

        }
       
        [HttpPut("{id}")]
        public IActionResult UpdateProduct([FromRoute] int id, [FromBody] Product product)
        {
            var isUpdate = _productcontext.UpdateProduct(id, product);
            if (isUpdate)
            {
                return Ok($"Product with id {id} is successfully updated");
            }
            return Ok($"Product with Id {id} is not found");
        }
    }
}
