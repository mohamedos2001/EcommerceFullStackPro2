using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        IWebHostEnvironment webHostEnvironment;
        public ProductController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        ContextDB context = new ContextDB();

        [HttpGet]
        public IActionResult GetProduct()
        {
            List<Product> product = context.Products.ToList();

            return Ok(product);

        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var product = context.Products.FirstOrDefault(e => e.Id == id);

            if (product == null)
            {
                
                return NotFound("Product not found");
            }

            
            return Ok(product);
        }


        [HttpPost]
        public IActionResult PostProduct([FromForm] Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Image");
                    var ext = Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    string imageUrl = $"http://localhost:5261/Image/{filename + ext}";
                    product.Image = imageUrl;

                    context.Products.Add(product);
                    context.SaveChanges();
                    return Created("http://localhost:5261/api/Product/" + product.Id, product);
                }
                return BadRequest(ModelState);

            }
            return Ok(product);
        }

        ////////////////////
        [HttpPut("{id}")]
        public IActionResult EditProduct(int id, [FromForm] Product product, IFormFile? file)
        {
            
            var existingProduct = context.Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound("Product not found");
            }

           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            existingProduct.Name = product.Name ?? existingProduct.Name;
            existingProduct.Description = product.Description ?? existingProduct.Description;
            existingProduct.Price = product.Price ?? existingProduct.Price;
            existingProduct.Category = product.Category ?? existingProduct.Category;

            
            if (file != null)
            {
                string rootPath = webHostEnvironment.WebRootPath;
                string filename = Guid.NewGuid().ToString();
                var uploadPath = Path.Combine(rootPath, "Image");
                var ext = Path.GetExtension(file.FileName);

              
                if (!string.IsNullOrEmpty(existingProduct.Image))
                {
                    var oldImagePath = Path.Combine(rootPath, existingProduct.Image.Replace("http://localhost:5261/", ""));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                
                using (var fileStream = new FileStream(Path.Combine(uploadPath, filename + ext), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                
                string imageUrl = $"http://localhost:5261/Image/{filename + ext}";
                existingProduct.Image = imageUrl;
            }

           
            context.SaveChanges();

            return Ok(existingProduct);
        }

        ///////
        [HttpDelete]
        [Route("{id:int}")]

        public IActionResult DeleteProduct(int id)
        {

            Product product = context.Products.FirstOrDefault(e => e.Id == id);
            context.Products.Remove(product);
            context.SaveChanges();

            return Ok();
        }

    }
}
