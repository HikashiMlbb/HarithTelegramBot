namespace Main.Core.Domain.Exceptions.Database;

[Serializable]
public class ConnectionStringIsNotDefinedException : Exception
{
    public ConnectionStringIsNotDefinedException(string connectionString) : base(
        $"Application can't find connection string ({connectionString}) in your configuration")
    {
    }
}