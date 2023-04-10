using Capas.accesoDatos;
using Capas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Capas.Logica
{
    public class LogIngrediente
    {
        public ResIngresarIngrediente insertarIngrediente(ReqIngresarIngrediente req)
        {
            ResIngresarIngrediente res = new ResIngresarIngrediente();
            try
            {
                //Validar datos

                if (req.ingrediente.idReceta == 0)
                {
                    res.listaDeErrores.Add("Falta id de la receta para asociar");
                    res.result = false;
                }
                else if (String.IsNullOrEmpty(req.ingrediente.ingrediente))
                {
                    res.listaDeErrores.Add("No se ingreso el nombre del ingrediente");
                    res.result = false;
                }
                else
                {
                    conexionLinqDataContext laConexion = new conexionLinqDataContext();
                    laConexion.SP_CREAR_INGREDIENTE(req.ingrediente.idReceta, req.ingrediente.ingrediente); ;
                    res.result = true;
                }
            }
            catch (Exception ex)
            {
                res.listaDeErrores.Add(ex.Message);
                res.result = false;
            }
            return res;
        }

        public ResActualizarIngrediente actualizarIngrediete(ReqActualizarIngrediente req)
        {
            ResActualizarIngrediente res = new ResActualizarIngrediente();
            try
            {
                //Validar datos

                if (req.ingrediente.id == 0)
                {
                    res.listaDeErrores.Add("No se ingreso el id");
                    res.result = false;
                }
                else if (String.IsNullOrEmpty(req.ingrediente.ingrediente))
                {

                    res.listaDeErrores.Add("Falta el nombre del ingrediente");
                    res.result = false;
                }
                else
                {
                    conexionLinqDataContext laConexion = new conexionLinqDataContext();
                    laConexion.SP_EDITAR_INGREDIENTE(req.ingrediente.id, req.ingrediente.ingrediente);
                    res.result = true;
                }
            }
            catch (Exception ex)
            {

                res.listaDeErrores.Add(ex.Message);
                res.result = false;
            }
            return res;

        }

        public ResEliminarIngrediente eliminarIngrediente(ReqEliminarIngrediente req)
        {
            ResEliminarIngrediente res = new ResEliminarIngrediente();
            try
            {
                if (req.id == 0)
                {
                    res.listaDeErrores.Add("No se envio el id");
                    res.result = false;
                }
                else
                {
                    conexionLinqDataContext laConexion = new conexionLinqDataContext();
                    laConexion.SP_ELIMINAR_INGREDIENTE(req.id);
                    res.result = true;
                }
            }
            catch (Exception ex)
            {
                res.listaDeErrores.Add(ex.Message);
                res.result = false;
            }
            return res;

        }


    }
}