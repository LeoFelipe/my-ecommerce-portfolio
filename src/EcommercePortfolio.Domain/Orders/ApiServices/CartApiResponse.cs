using System.Net;

namespace EcommercePortfolio.Domain.Orders.ApiServices;

public record CartApiResponse<T>(
    bool Success,
    HttpStatusCode StatusCode,
    T Response);
