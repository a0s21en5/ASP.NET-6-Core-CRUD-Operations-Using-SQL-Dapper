using Dapper;
using DapperCrudOp.Exception;
using DapperCrudOp.Modal;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.Common;

namespace DapperCrudOp.Repository
{
    public class ProductRepository
    {


        string connectionString = "Data Source=TRAININGDB03;Initial Catalog=ASHISHS_DAPPERDB;Integrated Security=True;TrustServerCertificate=True";

        /// <summary>
        /// Adds products to the DataBase.
        /// </summary>
        /// <param name="product">User Created products.</param>
        public void Add(Product product)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                if (product != null)
                {
                    dbConnection.Open();
                    string siQuery = "select * from Products where Name=@Name";
                    Product searchProducts = dbConnection.QuerySingleOrDefault<Product>(siQuery, new { Name = product.Name });
                    if (searchProducts != null)
                    {
                        throw new DulicateNameException($"{product.Name} is already present");
                    }
                    else
                    {
                        string sQuery = "insert into Products values(@Name,@Quantity,@Price)";
                        dbConnection.Execute(sQuery, product);
                    }

                }
                else
                {
                    throw new ProductIsNullException($"Your provide Product Object IsNull");
                }
            }
        }


        /// <summary>
        /// GetAll Products  from the DataBase.
        /// </summary>
        public List<Product> GetAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sQuery = "select * from Products";
                dbConnection.Open();
                List<Product> allProducts = dbConnection.Query<Product>(sQuery).ToList();
                if (allProducts.Count > 0)
                {
                    dbConnection.Close();

                    return allProducts;
                }
                else
                {
                    dbConnection.Close();

                    throw new EmptyListExecption($" Your List Is Empty");
                }
            }
        }


        /// <summary>
        /// Get product by Id from the DataBase.
        /// </summary>
        /// <param name="Id">User productId.</param>
        ///  <returns>return Products</returns>
        public Product GetbyId(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                if (id > 0)
                {
                    string sQuery = "select * from Products where ProductID=@Id";
                    dbConnection.Open();
                    Product idProducts = dbConnection.QueryFirst<Product>(sQuery, new { Id = id });
                    if (idProducts != null)
                    {
                        dbConnection.Close();

                        return idProducts;
                    }
                    else
                    {
                        dbConnection.Close();

                        throw new IdNotPresentException($"{id} not present in the Data Base");
                    }
                }

                else
                {
                    dbConnection.Close();

                    throw new InvalidIdExecption($"Id is Invalid Plase provide correct Id");
                }

            }
        }


        /// <summary>
        /// Give Id as Para and delete in the DataBase.
        /// </summary>
        /// <param name="Id">User Products Id</param>
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                if (id > 0)
                {
                    string sQuery = "Delete  from Products where ProductID=@Id";
                    dbConnection.Open();
                    int resultOfDelete = dbConnection.Execute(sQuery, new { Id = id });
                    if (resultOfDelete > 0)
                    {
                        dbConnection.Close();
                    }
                    else
                    {
                        dbConnection.Close();

                        throw new IdNotPresentException($"{id} not present in the Data Base");
                    }
                }

                else
                {

                    throw new InvalidIdExecption($"Id is Invalid Plase provide correct Id");
                }

            }
        }


        /// <summary>
        /// Give Id as Para and Update in the DataBase.
        /// </summary>
        /// <param name="product">User Products Id</param>

        public void Update(Product product)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                if (product != null && product.ProductId > 0)
                {
                    string sQuery = "update Products set Name=@Name,Quantity=@Quantity,Price=@Price where ProductID=@ProductID";
                    dbConnection.Open();
                    int updateResult = dbConnection.Execute(sQuery, product);
                    if (updateResult == 0)
                    {
                        throw new IdNotPresentException($"{product.ProductId} Not present in the database");
                    }
                }

                else
                {
                    if (product == null)
                    {
                        throw new ProductIsNullException($"Your provide Product Object IsNull");
                    }
                    else
                    {
                        throw new InvalidIdExecption($"Id is Invalid Plase provide correct Id");
                    }
                }

            }
        }

    }
}
