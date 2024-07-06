namespace Room8.API.ExceptionHandler.CustomExceptions
{
    [Serializable]
    public class IllegalArgumentException : Exception
    {
        public IllegalArgumentException()
        {

        }
        public IllegalArgumentException(string? message)
            : base(message)
        {
        }
    }
}
