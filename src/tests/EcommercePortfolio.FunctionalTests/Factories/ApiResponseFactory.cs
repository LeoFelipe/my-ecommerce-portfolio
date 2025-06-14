using System.Net;

namespace EcommercePortfolio.FunctionalTests.Factories;

public record ApiResponse<T>(
    bool Success,
    HttpStatusCode StatusCode,
    T Response);