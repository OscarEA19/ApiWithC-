using Capas.accesoDatos;
using Capas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Capas.Logica
{
    public class LogPaso
    {
        public ResIngresarPaso insertarPaso(ReqIngresarPaso req)
        {
            ResIngresarPaso res = new ResIngresarPaso();
            try
            {
                //Validar datos

                if (req.paso.idReceta == 0)
                {
                    res.listaDeErrores.Add("Falta id de la receta para asociar");
                    res.result = false;
                }
                else if (String.IsNullOrEmpty(req.paso.paso))
                {
                    res.listaDeErrores.Add("No se ingreso el nombre del paso");
                    res.result = false;
                }
                else
                {
                    conexionLinqDataContext laConexion = new conexionLinqDataContext();
                    laConexion.SP_CREAR_PASO(req.paso.idReceta, req.paso.paso);
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

        public ResActualizarPaso actualizarPaso(ReqActualizarPaso req)
        {
            ResActualizarPaso res = new ResActualizarPaso();
            try
            {
                //Validar datos

                if (req.paso.id == 0)
                {
                    res.listaDeErrores.Add("No se ingreso el id");
                    res.result = false;
                }
                else if (String.IsNullOrEmpty(req.paso.paso))
                {

                    res.listaDeErrores.Add("Falta el nombre del pasp");
                    res.result = false;
                }
                else
                {
                    conexionLinqDataContext laConexion = new conexionLinqDataContext();
                    laConexion.SP_EDITAR_PASO(req.paso.id, req.paso.paso);
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

        public ResEliminarPaso eliminarPaso(ReqEliminarPaso req)
        {
            ResEliminarPaso res = new ResEliminarPaso();
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
                    laConexion.SP_ELIMINAR_PASO(req.id);
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