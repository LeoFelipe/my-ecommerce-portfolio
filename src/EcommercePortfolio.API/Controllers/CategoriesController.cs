using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {

        private static readonly string[] Categories =
        [
            "Móveis", "Eletro", "Esportes", "Celulares", "Decoracao", "Agro", "Moda", "Imóveis", "Veículos", "Hobbies"
        ];


        [HttpGet(Name = "Categories")]
        public IEnumerable<string> Get()
        {
            return Categories;
        }
    }
}
