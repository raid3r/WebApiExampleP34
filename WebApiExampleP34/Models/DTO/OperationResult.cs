using Swashbuckle.AspNetCore.Annotations;

namespace WebApiExampleP34.Models.DTO;

public class OperationResult
{
    [SwaggerSchema("Результат операції")]
    public bool Success { get; set; }

    [SwaggerSchema("Опис помилки, якщо помилка")]
    public string? ErrorMessage { get; set; }

    public static OperationResult Ok()
    {
        return new OperationResult { Success = true };
    }

    public static OperationResult Fail(string errorMessage)
    {
        return new OperationResult { Success = false, ErrorMessage = errorMessage };
    }

}
