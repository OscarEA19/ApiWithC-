using System.Web.Http;
using Capas.Entidades;
using Capas.Logica;

namespace webApiRest.Controllers
{
    public class IngredienteController : ApiController
    {
        // POST api/<controller>
        public ResIngresarIngrediente Post([FromBody] ReqIngresarIngrediente req)
        {
            LogIngrediente laLogicaDelBackEnd = new LogIngrediente();
            return laLogicaDelBackEnd.insertarIngrediente(req);

        }

        // PUT api/<controller>/5
        public ResActualizarIngrediente Put([FromBody] ReqActualizarIngrediente req)
        {
            LogIngrediente laLogicaDelBackEnd = new LogIngrediente();
            return laLogicaDelBackEnd.actualizarIngrediete(req);
        }
        
        // DELETE api/<controller>/5
        public ResEliminarIngrediente Delete(int id)
        {
            ReqEliminarIngrediente req = new ReqEliminarIngrediente();
            req.id = id;
            LogIngrediente laLogicaDelBackEnd = new LogIngrediente();
            return laLogicaDelBackEnd.eliminarIngrediente(req);
        }
    }
}