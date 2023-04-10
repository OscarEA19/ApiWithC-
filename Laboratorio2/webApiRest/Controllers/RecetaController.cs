using System.Web.Http;
using Capas.Entidades;
using Capas.Logica;

namespace webApiRest.Controllers
{
    public class RecetaController : ApiController
    {

        // POST api/<controller>
        public ResIngresarReceta Post([FromBody] ReqIngresarReceta req)
        {
            LogReceta laLogicaDelBackEnd = new LogReceta();
            return laLogicaDelBackEnd.insertarReceta(req);
        }
        // PUT api/<controller>/5
        public ResActualizarReceta Put([FromBody] ReqActualizarReceta req)
        {
            LogReceta laLogicaDelBackEnd = new LogReceta();
            return laLogicaDelBackEnd.actualizarRecet(req);
        }

        // DELETE api/<controller>/5
        public ResEliminarReceta Delete(int id)
        {
            ReqEliminarReceta req = new ReqEliminarReceta();
            req.id = id;
            LogReceta laLogicaDelBackEnd = new LogReceta();
            return laLogicaDelBackEnd.eliminarReceta(req);
        }


        // GET  dominio/api/usuario
        public ResObtenerTodasLasRecetas Get()
        {
            ReqObtenerTodasLasRecetas req = new ReqObtenerTodasLasRecetas();
            LogReceta laLogicaDelBackEnd = new LogReceta();
            return laLogicaDelBackEnd.obtenerTodasLasRecetas(req);
        }

        // GET api/<controller>/5
        public ResObtenerRecetaByUsuario Get(int id)
        {
            ReqObtenerRecetaByUsuario req = new ReqObtenerRecetaByUsuario();
            req.idUsuario = id;
            LogReceta laLogicaDelBackEnd = new LogReceta();
            return laLogicaDelBackEnd.obtenerRecetaByUsuario(req);
        }

       

        


    }
}