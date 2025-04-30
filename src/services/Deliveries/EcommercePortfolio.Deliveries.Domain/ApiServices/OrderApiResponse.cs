using System.Net;

namespace EcommercePortfolio.Deliveries.Domain.ApiServices;

public record OrderApiResponse<T>(
    bool Success,
    HttpStatusCode StatusCode,
    T Response);
