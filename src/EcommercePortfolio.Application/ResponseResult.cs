using System.Net;

namespace EcommercePortfolio.Application;

public record ResponseResult(
    bool Success,
    HttpStatusCode StatusCode,
    object Response);