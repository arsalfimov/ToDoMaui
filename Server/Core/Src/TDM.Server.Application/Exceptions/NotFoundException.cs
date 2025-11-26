namespace TDM.Server.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
    
    public NotFoundException(string entityName, long id) 
        : base($"{entityName} с ID {id} не найден.")
    {
    }
}
