namespace WebApiExampleP34.Models.DTO;

public class AuthResultDto: OperationResult
{
    public string? Token { get; set; }

    public static AuthResultDto Ok(string token)
    {
        return new AuthResultDto { Success = true, Token = token };
    }

    public static AuthResultDto Fail(string errorMessage)
    {
        return new AuthResultDto { Success = false, ErrorMessage = errorMessage };
    }
}
