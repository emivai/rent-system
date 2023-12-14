namespace RentSystem.Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
        }
        public BadRequestException(string text)
           : base(String.Format(text))
        {

        }
    }
}
