using System.Text;

namespace GoldSync.Service.Contracts.Exceptions;

public static class ExceptionExtension
{
    public static string AggregateExceptionMessages(this Exception ex)
    {
        StringBuilder sb = new();

        while (ex is not null)
        {
            sb.Append(ex.Message);
            sb.Append(" :: ");
            ex = ex.InnerException;
        }

        return sb.ToString();
    }
}
