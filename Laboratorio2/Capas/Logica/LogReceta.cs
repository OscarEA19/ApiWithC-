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
                        res.listaDeRecetas = this.armarListaDeRecetas(result);
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
            List<Ingrediente> listaIngrediente = new List<Ingrediente>();
            List<Paso> listaPaso = new List<Paso>();

            foreach (SP_MOSTRAR_RECETAResult elementoLinq in listaDeRecetasLinq)
            {
                Ingrediente ingrediente = new Ingrediente()
                {
                    idReceta = elementoLinq.id,
                    ingrediente = elementoLinq.ingrediente,
                };
                listaIngrediente.Add(ingrediente);
            }

            foreach (SP_MOSTRAR_RECETAResult elementoLinq in listaDeRecetasLinq)
            {
                Paso paso = new Paso()
                {
                    idReceta = elementoLinq.id,
                    paso = elementoLinq.paso,
                };
                listaPaso.Add(paso);    
            }


            foreach (SP_MOSTRAR_RECETAResult elementoLinq in listaDeRecetasLinq)
            {
                bool existeReceta = false;

                foreach(Receta receta in listaRecetaArmada)
                {
                    if (receta.id == elementoLinq.id)
                    {
                        existeReceta = true;
                    }
                }

                if (!existeReceta)
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
                    };
                    foreach (Ingrediente ingrediente in listaIngrediente)
                    {
                        bool existeIngrediente = false;
                        foreach(Ingrediente ingredienteValid in receta.ingredientes)
                        {
                            if (ingredienteValid.ingrediente.Equals(ingrediente.ingrediente))
                            {
                                existeIngrediente = true;
                            }
                        }

                        if (ingrediente.idReceta == receta.id && !existeIngrediente)
                        {
                            receta.ingredientes.Add(ingrediente);
                        }
                    }
                    foreach (Paso paso in listaPaso)
                    {
                        bool existePaso = false;
                        foreach (Paso pasoValid in receta.pasos)
                        {
                            if (pasoValid.paso.Equals(paso.paso))
                            {
                                existePaso = true;
                            }
                        }
                        if (paso.idReceta == receta.id && !existePaso)
                        {
                            receta.pasos.Add(paso);
                        }
                    }
                    listaRecetaArmada.Add(receta);
                }
            }
            return listaRecetaArmada;
        }


        private List<Receta> armarListaDeRecetas(List<SP_MOSTRAR_RECETA_BY_USUARIOResult> listaDeRecetasLinq)
        {
            List<Receta> listaRecetaArmada = new List<Receta>();
            List<Ingrediente> listaIngrediente = new List<Ingrediente>();
            List<Paso> listaPaso = new List<Paso>();

            foreach (SP_MOSTRAR_RECETA_BY_USUARIOResult elementoLinq in listaDeRecetasLinq)
            {
                Ingrediente ingrediente = new Ingrediente()
                {
                    idReceta = elementoLinq.id,
                    ingrediente = elementoLinq.ingrediente,
                };
                listaIngrediente.Add(ingrediente);
            }

            foreach (SP_MOSTRAR_RECETA_BY_USUARIOResult elementoLinq in listaDeRecetasLinq)
            {
                Paso paso = new Paso()
                {
                    idReceta = elementoLinq.id,
                    paso = elementoLinq.paso,
                };
                listaPaso.Add(paso);
            }


            foreach (SP_MOSTRAR_RECETA_BY_USUARIOResult elementoLinq in listaDeRecetasLinq)
            {
                bool existeReceta = false;

                foreach (Receta receta in listaRecetaArmada)
                {
                    if (receta.id == elementoLinq.id)
                    {
                        existeReceta = true;
                    }
                }

                if (!existeReceta)
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
                    };
                    foreach (Ingrediente ingrediente in listaIngrediente)
                    {
                        bool existeIngrediente = false;
                        foreach (Ingrediente ingredienteValid in receta.ingredientes)
                        {
                            if (ingredienteValid.ingrediente.Equals(ingrediente.ingrediente))
                            {
                                existeIngrediente = true;
                            }
                        }

                        if (ingrediente.idReceta == receta.id && !existeIngrediente)
                        {
                            receta.ingredientes.Add(ingrediente);
                        }
                    }
                    foreach (Paso paso in listaPaso)
                    {
                        bool existePaso = false;
                        foreach (Paso pasoValid in receta.pasos)
                        {
                            if (pasoValid.paso.Equals(paso.paso))
                            {
                                existePaso = true;
                            }
                        }
                        if (paso.idReceta == receta.id && !existePaso)
                        {
                            receta.pasos.Add(paso);
                        }
                    }
                    listaRecetaArmada.Add(receta);
                }
            }
            return listaRecetaArmada;
        }
    }
}