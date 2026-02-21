namespace AuthorizationIntegration.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _requestDelegate;
    public GlobalExceptionMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured while requesting endpoint");
            var response = new
            {
                Message = $"Error Message: {ex.Message}",
                StatusCode = 500,
                Detail = "Error occured while processing"
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
