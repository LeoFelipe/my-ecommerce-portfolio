using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {

        private static readonly string[] Categories =
        [
            "M�veis", "Eletro", "Esportes", "Celulares", "Decoracao", "Agro", "Moda", "Im�veis", "Ve�culos", "Hobbies"
        ];


        [HttpGet(Name = "Categories")]
        public IEnumerable<string> Get()
        {
            return Categories;
        }
    }
}
