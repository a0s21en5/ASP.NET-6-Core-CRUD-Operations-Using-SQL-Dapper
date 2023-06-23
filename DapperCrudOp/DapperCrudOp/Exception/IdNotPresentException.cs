namespace DapperCrudOp.Exception
{
    public class IdNotPresentException:ApplicationException
    {
        public IdNotPresentException()
        {
                
        }
        public IdNotPresentException(string msg):base(msg)
        {

        }
    }
}
