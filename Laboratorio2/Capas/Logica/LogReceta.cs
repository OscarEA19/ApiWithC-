using Capas.accesoDatos;
using Capas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Capas.Logica
{
    public class LogReceta
    {
        public ResIngresarReceta insertarReceta(ReqIngresarReceta req)
        {
            ResIngresarReceta res = new ResIngresarReceta();
            try
            {
                //Validar datos

                if (String.IsNullOrEmpty(req.receta.nombreReceta))
                {
                    res.listaDeErrores.Add("Falta el nombre de la receta");
                    res.result = false;
                }
                else if (req.receta.calificacion == 0)
                {
                    res.listaDeErrores.Add("No se ingreso la calificacion de la receta");
                    res.result = false;
                }
                else if (req.receta.usuario.id == 0)
                {
                    res.listaDeErrores.Add("No se ingreso el id del usuario");
                    res.result = false;
                }
                else
                {
                    conexionLinqDataContext laConexion = new conexionLinqDataContext();
                    laConexion.SP_CREAR_RECETA(req.receta.nombreReceta, req.receta.calificacion, req.receta.usuario.id);
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

        public ResObtenerRecetaByUsuario obtenerRecetaByUsuario(ReqObtenerRecetaByUsuario req)
        {
            ResObtenerRecetaByUsuario res = new ResObtenerRecetaByUsuario();
            try
            {
                if (req.idUsuario == 0)
                {
                    res.listaDeErrores.Add("No se envio el id");
                    res.result = false;
                }
                else
                {
                    conexionLinqDataContext laConexion = new conexionLinqDataContext();
                    List<SP_MOSTRAR_RECETA_BY_USUARIOResult> result = laConexion.SP_MOSTRAR_RECETA_BY_USUARIO(req.idUsuario).ToList();
                    if (result.Count != 0)
                    {
                        res.receta = new Receta()
                        {

                            id = result[0].id,
                            nombreReceta = result[0].nombreReceta,
                            calificacion = result[0].calificacion,
                            status = result[0].status,
                            usuario = new Usuario()
                            {
                                username = result[0].username,
                            },
                            ingredientes = armarIngrediente(result),
                            pasos = armarPaso(result)
                        };

                        res.result = true;
                    }
                    else
                    {
                        res.listaDeErrores.Add("No se encontró el usuario: " + req.idUsuario);
                        res.result = false;
                    }

                }
            }
            catch (Exception ex)
            {
                res.listaDeErrores.Add(ex.Message);
                res.result = false;
            }
            return res;
        }

        public ResActualizarReceta actualizarRecet(ReqActualizarReceta req)
        {
            ResActualizarReceta res = new ResActualizarReceta();
            try
            {
                //Validar datos

                if (req.receta.id == 0)
                {
                    res.listaDeErrores.Add("No se ingreso el id");
                    res.result = false;
                }
                else if (String.IsNullOrEmpty(req.receta.nombreReceta))
                {

                    res.listaDeErrores.Add("Falta el nombre de la receta");
                    res.result = false;
                }
                else if (req.receta.calificacion == 0)
                {
                    res.listaDeErrores.Add("Falto editar la calificacio");
                    res.result = false;
                }
                else
                {
                    conexionLinqDataContext laConexion = new conexionLinqDataContext();
                    laConexion.SP_EDITAR_RECETA(req.receta.id, req.receta.nombreReceta, req.receta.calificacion);
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

        public ResEliminarReceta eliminarReceta(ReqEliminarReceta req)
        {
            ResEliminarReceta res = new ResEliminarReceta();
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
                    laConexion.SP_ELIMINAR_RECETA(req.id);
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

        public ResObtenerTodasLasRecetas obtenerTodasLasRecetas(ReqObtenerTodasLasRecetas req)
        {
            ResObtenerTodasLasRecetas res = new ResObtenerTodasLasRecetas();
            try
            {
                conexionLinqDataContext laConexion = new conexionLinqDataContext();
                List<SP_MOSTRAR_RECETAResult> result = laConexion.SP_MOSTRAR_RECETA().ToList();
                if (result.Count != 0)
                {
                    res.listaDeRecetas = this.armarListaDeRecetas(result);
                    res.result = true;
                }
                else
                {
                    res.listaDeErrores.Add("No se encontraron usuarios");
                    res.result = false;
                }

            }
            catch (Exception ex)
            {
                res.listaDeErrores.Add(ex.Message);
                res.result = false;
            }
            return res;
        }

        private List<Receta> armarListaDeRecetas(List<SP_MOSTRAR_RECETAResult> listaDeRecetasLinq)
        {
            List<Receta> listaRecetaArmada = new List<Receta>();
            foreach (SP_MOSTRAR_RECETAResult elementoLinq in listaDeRecetasLinq)
            {

                Receta receta = new Receta()
                {

                    id = elementoLinq.id,
                    nombreReceta = elementoLinq.nombreReceta,
                    calificacion = elementoLinq.calificacion,
                    status = elementoLinq.status,
                    usuario = new Usuario()
                    {
                        username = elementoLinq.username
                    },
                    ingredientes = armarIngrediente(listaDeRecetasLinq),
                    pasos = armarPaso(listaDeRecetasLinq)
                };
                listaRecetaArmada.Add(receta);
            }
            return listaRecetaArmada;
        }
        private List<Ingrediente> armarIngrediente(List<SP_MOSTRAR_RECETA_BY_USUARIOResult> result)
        {
            List<Ingrediente> listaIngredientes = new List<Ingrediente>();
            foreach (SP_MOSTRAR_RECETA_BY_USUARIOResult elementoLinq in result)
            {
                Ingrediente ingrediente = new Ingrediente();
                ingrediente.ingrediente = elementoLinq.ingrediente;
                listaIngredientes.Add(ingrediente);
            }
            return listaIngredientes;
        }
        private List<Ingrediente> armarIngrediente(List<SP_MOSTRAR_RECETAResult> result)
        {
            List<Ingrediente> listaIngredientes = new List<Ingrediente>();
            foreach (SP_MOSTRAR_RECETAResult elementoLinq in result)
            {
                Ingrediente ingrediente = new Ingrediente();
                ingrediente.ingrediente = elementoLinq.ingrediente;
                listaIngredientes.Add(ingrediente);
            }
            return listaIngredientes;
        }

        private List<Paso> armarPaso(List<SP_MOSTRAR_RECETA_BY_USUARIOResult> result)
        {
            List<Paso> listaPaso = new List<Paso>();
            foreach (SP_MOSTRAR_RECETA_BY_USUARIOResult elementoLinq in result)
            {
                Paso paso = new Paso();
                paso.paso = elementoLinq.paso;
                listaPaso.Add(paso);
            }
            return listaPaso;
        }
        private List<Paso> armarPaso(List<SP_MOSTRAR_RECETAResult> result)
        {
            List<Paso> listaPaso = new List<Paso>();
            foreach (SP_MOSTRAR_RECETAResult elementoLinq in result)
            {
                Paso paso = new Paso();
                paso.paso = elementoLinq.paso;
                listaPaso.Add(paso);
            }
            return listaPaso;
        }


    }
}