namespace Hao.GroupBlog.Manager.Basic
{
    public class MyCustomException : Exception
    {
        public MyCustomException(string message) : base(message)
        {

        }
    }

    public class MyUnauthorizedException : Exception
    {
        public MyUnauthorizedException(string message) : base(message)
        {

        }
    }
}
