using System.Net;

namespace EcommercePortfolio.Services.ObjectResponses;

public record ResponseResult(
    bool Success,
    HttpStatusCode StatusCode,
    object Response);