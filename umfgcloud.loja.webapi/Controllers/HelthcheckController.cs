using Microsoft.AspNetCore.Mvc;

namespace umfgcloud.loja.webapi.Controllers
{   
    /// <summary>
    /// endpoint de verificação de saúde da API
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("[controller]")]

    public sealed class HelthcheckController : ControllerBase
    {   
        /// <summary>
        /// verifica o estado de saúde da API 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok($"umfgcloud.loja.service -> online | {DateTime.Now}");
        }

        //[HttpPost]
        //public IActionResult Post()
        //{
        //    return Ok("Teste Post");
        //}
    }
}
