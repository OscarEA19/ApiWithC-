using System.Web.Http;
using Capas.Entidades;
using Capas.Logica;

namespace webApiRest.Controllers
{
    public class LoginController : ApiController
    {

        // POST api/<controller>
        public ResLoginUser Post([FromBody] ReqLoginUser req)
        {
            LogUsuario laLogicaDelBackEnd = new LogUsuario();
            return laLogicaDelBackEnd.loginUser(req);

        }
    }
}