using System.Net;

namespace EcommercePortfolio.Orders.Domain.ApiServices;

public record CartApiResponse<T>(
    bool Success,
    HttpStatusCode StatusCode,
    T Response);
