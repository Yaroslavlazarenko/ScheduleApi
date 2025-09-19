namespace ScheduleApi.ExceptionHandlers;

public static class ExceptionHandlerExtensions
{
    public static IServiceCollection AddGlobalExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<UnhandledExceptionHandler>();
        services.AddProblemDetails();
        
        return services;
    }
}