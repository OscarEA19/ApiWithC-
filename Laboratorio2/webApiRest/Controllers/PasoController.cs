using System.Web.Http;
using Capas.Entidades;
using Capas.Logica;

namespace webApiRest.Controllers
{
    public class PasoController : ApiController
    {

        // POST api/<controller>
        public ResIngresarPaso Post([FromBody] ReqIngresarPaso req)
        {
            LogPaso laLogicaDelBackEnd = new LogPaso();
            return laLogicaDelBackEnd.insertarPaso(req);

        }

        // PUT api/<controller>/5
        public ResActualizarPaso Put([FromBody] ReqActualizarPaso req)
        {
            LogPaso laLogicaDelBackEnd = new LogPaso();
            return laLogicaDelBackEnd.actualizarPaso(req);
        }

        // DELETE api/<controller>/5
        public ResEliminarPaso Delete(int id)
        {
            ReqEliminarPaso req = new ReqEliminarPaso();
            req.id = id;
            LogPaso laLogicaDelBackEnd = new LogPaso();
            return laLogicaDelBackEnd.eliminarPaso(req);
        }

    }
}