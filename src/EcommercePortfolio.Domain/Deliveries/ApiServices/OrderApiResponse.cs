using System.Net;

namespace EcommercePortfolio.Domain.Deliveries.ApiServices;

public record OrderApiResponse<T>(
    bool Success,
    HttpStatusCode StatusCode,
    T Response);
