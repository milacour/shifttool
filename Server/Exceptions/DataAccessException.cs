using System;
namespace ShiftTool.Server.Exceptions
{
    // Definition af en custom exception til databaseoperationer
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

