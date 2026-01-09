using Swashbuckle.AspNetCore.Annotations;

namespace WebApiExampleP34.Models.DTO;

public class OperationResult<T> where T: class
{
    [SwaggerSchema("Результат операції")]
    public bool Success { get; set; }

    [SwaggerSchema("Опис помилки, якщо помилка")]
    public string? ErrorMessage { get; set; }

    public T? Data { get; set; }

    public static OperationResult<T> Ok(T? data = null)
    {
        return new OperationResult<T> { Success = true, Data = data };
    }

    public static OperationResult<T> Fail(string errorMessage)
    {
        return new OperationResult<T> { Success = false, ErrorMessage = errorMessage, Data = null };
    }

}
