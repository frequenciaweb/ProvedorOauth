using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilitarios;

namespace Site.MVC.Controllers
{
    [Authorize]
    public class VendasController : Controller
    {
        public async Task<IActionResult> Index()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            ViewBag.RetornoApiClientes = await Integracao.ChamarApi("https://localhost:44309/", accessToken);
            ViewBag.RetornoApiProdutos = await Integracao.ChamarApi("https://localhost:44356/", accessToken);
            ViewBag.RetornoApiVendas = await Integracao.ChamarApi("https://localhost:44334/", accessToken);
            return View();
        }
    }
}
