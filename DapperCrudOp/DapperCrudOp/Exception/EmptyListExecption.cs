namespace DapperCrudOp.Exception
{
    public class EmptyListExecption:ApplicationException
    {
        public EmptyListExecption()
        {
                
        }

        public EmptyListExecption(string msg):base(msg)
        {

        }
    }
}
