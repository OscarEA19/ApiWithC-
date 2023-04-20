using System.Web.Http;
using Capas.Entidades;
using Capas.Logica;

namespace webApiRest.Controllers
{
    public class UserController : ApiController
    {
        // POST api/<controller>
        public ResIngresarUsuario Post([FromBody] ReqIngresarUsuario req)
        {
            LogUsuario laLogicaDelBackEnd = new LogUsuario();
            return laLogicaDelBackEnd.insertarUsuario(req);

        }

        // PUT api/<controller>/
        public ResActualizarUsuario Put([FromBody] ReqActualizarUsuario req)
        {
            LogUsuario laLogicaDelBackEnd = new LogUsuario();
            return laLogicaDelBackEnd.actualizarUsuario(req);
        }

        // DELETE api/<controller>/5
        public ResEliminarUsuario Delete(int id)
        {
            ReqEliminarUsuario req = new ReqEliminarUsuario();
            req.id = id;
            LogUsuario laLogicaDelBackEnd = new LogUsuario();
            return laLogicaDelBackEnd.eliminarUsuario(req);
        }

        // GET  dominio/api/usuario
        public ResObtenerTodosLosUsuarios Get()
        {
            ReqObtenerTodosLosUsuarios req = new ReqObtenerTodosLosUsuarios();

            LogUsuario laLogicaDelBackEnd = new LogUsuario();
            return laLogicaDelBackEnd.obtenerTodosLosUsuarios(req);
        }

        // GET api/<controller>/
        public ResObtenerUsuario Get(string email, string password)
        {
            ReqObtenerUsuario req = new ReqObtenerUsuario();
            req.email= email; 
            req.password= password; 

            LogUsuario laLogicaDelBackEnd = new LogUsuario();
            return laLogicaDelBackEnd.obtenerUsuario(req);
        }

    }
}