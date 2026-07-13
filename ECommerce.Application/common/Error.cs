using System.Text.Json.Serialization;

namespace ECommerce.Application.common;

public sealed record Error(string Code, string Description, ErrorType ErrorType = ErrorType.Failure)
{
    public static Error ValidationError(string code="General.validation", string description= "General Validation Error has occurred")
        => new Error(code, description, ErrorType.ValidationError);
    public static Error NotFound(string code = "General.notfound", string description = "Requested resource not found")
        => new Error(code, description, ErrorType.NotFound);
    public static Error Conflict(string code = "General.conflict", string description = "Resource conflict has occurred")
        => new Error(code, description, ErrorType.Conflict);
    public static Error Unauthorized(string code = "General.unauthorized", string description = "Unauthorized access")
        => new Error(code, description, ErrorType.Unauthorized);
    public static Error Forbidden(string code = "General.forbidden", string description = "Forbidden access")
        => new Error(code, description, ErrorType.Forbidden);
    public static Error InternalServerError(string code = "General.internalservererror", string description = "Internal server error has occurred")
        => new Error(code, description, ErrorType.InternalServerError);
    public static Error Failure(string code = "General.failure", string description = "General failure has occurred")
        => new Error(code, description, ErrorType.Failure);
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorType
{
    Failure = 0,
    ValidationError = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    InternalServerError = 6
}