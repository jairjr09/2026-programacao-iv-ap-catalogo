using Microsoft.AspNetCore.Mvc;
using umfgcloud.loja.dominio.service.DTO;
using umfgcloud.loja.dominio.service.Interfaces.Servicos;

namespace umfgcloud.loja.webapi.Controllers
{

    /// <summary>
    /// endpoint(s) para cadastrar um usuário
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("[controller]")]

    public sealed class SignupController : ControllerBase
    {   
        // a palavra reservada readonly permite que a variável
        // tenha seu valor manipulado apenas em sua definição
        // ou dentro do método construtor

        private readonly IUsuarioServico _servico;

        public SignupController(IUsuarioServico servico)
        {
            _servico = servico ?? throw new ArgumentNullException(nameof(servico));
        }

        /// <summary>
        /// Cadastra um usuário
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SignupAsync(UsuarioDTO.SingUpRequest dto)
        {
            try
            {
                await _servico.CadastrarAsync(dto);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
