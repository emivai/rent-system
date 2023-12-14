namespace RentSystem.Core.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException()
        {
        }

        public AlreadyExistsException(string entity)
            : base(String.Format($"{entity} already exists"))
        {

        }
    }
}
