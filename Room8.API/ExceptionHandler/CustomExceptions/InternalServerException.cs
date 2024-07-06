namespace Room8.API.ExceptionHandler.CustomExceptions
{
    [Serializable]
    public class InternalServerException : Exception
    {
        public InternalServerException()
        {

        }
        public InternalServerException(string? message)
            : base(message)
        {
        }
    }
}
