using System.Runtime.CompilerServices;
using DapperCrudOp.Exception;
using DapperCrudOp.Modal;
using DapperCrudOp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperCrudOp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //Readonly Variable

        private readonly ProductRepository productRepository;
        public ProductController()
        {
            productRepository = new ProductRepository();
        }

        //This function return AllProducts in the database if the return list is empty then it raise exception - EmptyListExecption

        [HttpGet("GetAllProducts")]
        public ActionResult GetAllProducts()
        {
            try {
                List<Product> allProducts=productRepository.GetAll();
                return Ok(allProducts);
            }
            catch (EmptyListExecption ex)
            {
                return StatusCode(500, ex.Message);
            }
          
        }

        //This function return id related object if the Id not valid then it raise exception - InvalidIdExecption

        [HttpGet("GetProductId/{id}")]
        public ActionResult GetProductId(int id)
        {
            try
            {
                Product idProducts=productRepository.GetbyId(id);
                return Ok(idProducts);
            }
            catch (InvalidIdExecption ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //This function Add a new product to database  if the product is null  then it raise exception - ProductIsNullException

        [HttpPost("AddProducts")]
        public ActionResult AddProducts(Product prop)
        {
            try
            {
                productRepository.Add(prop);
               return  Ok(true);
            }
            catch(ProductIsNullException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch(DulicateNameException ex) 
            {
                return StatusCode(400, ex.Message);
            }
        }

        //This Function Delete Product with the help of productId if Id not valid then it raise exception - InvalidIdExecption

        [HttpDelete("DeleteProduct/{id}")]
        public ActionResult DeleteProduct(int id)

        {
            try
            {
                productRepository.Delete(id);
                return Ok(true);
            }
            catch(InvalidIdExecption ex) 
            {
                return StatusCode(500, ex.Message);
            }
            catch(IdNotPresentException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //This Function Update existing productObject if newproducts and id not valid it raise exception - InvalidIdExecption and ProductIsNullException

        [HttpPut("UpdateProducts/{id}")]
        public ActionResult UpdateProducts(int id ,Product prop)
        {
            try
            {
                prop.ProductId = id;
                productRepository.Update(prop);
               return  Ok(true);
            }

            catch(InvalidIdExecption ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch(ProductIsNullException ex) 
            {
                return StatusCode(500, ex.Message);
            }
            catch(IdNotPresentException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
