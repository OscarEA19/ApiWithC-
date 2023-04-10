using Capas.accesoDatos;
using Capas.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Capas.Logica
{
    public class LogUsuario
    {
        public ResLoginUser loginUser(ReqLoginUser req)
        {
            ResLoginUser res = new ResLoginUser();
            try
            {
                //Validate data
                if (String.IsNullOrEmpty(req.user.email))
                {
                    res.listaDeErrores.Add("No se ha ingresado el correo electronico");
                    res.result = false;
                }
                else if (String.IsNullOrEmpty(req.user.password))
                {
                    res.listaDeErrores.Add("No se ha ingresado la contraseña para verificar");
                    res.result = false;
                }
                else
                {
                    conexionLinqDataContext connect = new conexionLinqDataContext();
                    List<SP_VERIFIED_USER_LOGINResult> responseDataBase = connect.SP_VERIFIED_USER_LOGIN(req.user.email).ToList();
                    if (responseDataBase.Count() == 0)
                    {
                        res.listaDeErrores.Add("Usuario y password incorrecto");
                        res.result = false;
                    }
                    else
                    {
                        string passwordInput = req.user.password;
                        string passwordEncrypt = encrypt(passwordInput);

                        string passwordUser = responseDataBase[0].password;

                        if (passwordEncrypt.Equals(passwordUser)){
                            res.result=true;
                        }
                        else
                        {
                            res.listaDeErrores.Add("Usuario y password incorrecto");
                            res.result = false;
                        }
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
        public static string encrypt(string password)
        {
            if (password == null)
            {
                return null;
            }
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(password);
            return Convert.ToBase64String(encryted);
        }
        public ResIngresarUsuario insertarUsuario(ReqIngresarUsuario req)
        {
            ResIngresarUsuario res = new ResIngresarUsuario();
            try
            {
                //Validar datos
                if (String.IsNullOrEmpty(req.usuario.username))
                {

                    res.listaDeErrores.Add("NombreUsuario falta");
                    res.result = false;
                }
                else if (String.IsNullOrEmpty(req.usuario.password))
                {
                    res.listaDeErrores.Add("No se ingreso la contrasena");
                    res.result = false;
                }
                else if (String.IsNullOrEmpty(req.usuario.email))
                {
                    res.listaDeErrores.Add("No se ingreso el correo");
                    res.result = false;
                }
                else
                {
                    String passwordEncryp = encrypt(req.usuario.password);
                    conexionLinqDataContext laConexion = new conexionLinqDataContext();
                    laConexion.SP_CREAR_USUARIO(req.usuario.username, passwordEncryp, req.usuario.email); ;
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

        public ResObtenerUsuario obtenerUsuario(ReqObtenerUsuario req)
        {
            ResObtenerUsuario res = new ResObtenerUsuario();
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
                    List<SP_VER_UN_USUARIO_BY_USERNAMEResult> result = laConexion.SP_VER_UN_USUARIO_BY_USERNAME(req.id).ToList();
                    if (result.Count != 0)
                    {
                        res.usuario = new Usuario()
                        {
                            id = result[0].id,
                            username = result[0].username,
                            status = result[0].status,
                            isAdmin = result[0].isAdmin,
                            email = result[0].email,

                        };

                        res.result = true;
                    }
                    else
                    {
                        res.listaDeErrores = new List<string>();
                        res.listaDeErrores.Add("No se encontró el usuario: " + req.id);
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

        public ResActualizarUsuario actualizarUsuario(ReqActualizarUsuario req)
        {
            ResActualizarUsuario res = new ResActualizarUsuario();
            try
            {
                //Validar datos
                bool ejecutarUpdateAdmin = false;

                if (req.usuario.id == 0)
                {
                    res.listaDeErrores.Add("No se ingreso el id");
                    res.result = false;
                }
                if (String.IsNullOrEmpty(req.usuario.username))
                {
                    res.listaDeErrores.Add("Falta el nombre de usuario");
                    res.result = false;
                    ejecutarUpdateAdmin = true;
                }

                if (String.IsNullOrEmpty(req.usuario.email))
                {
                    res.listaDeErrores.Add("Falta el email");
                    res.result = false;
                    ejecutarUpdateAdmin = true;
                }

                conexionLinqDataContext laConexion = new conexionLinqDataContext();
                if (!ejecutarUpdateAdmin)
                {

                    laConexion.SP_EDITAR_USUARIO(req.usuario.id, req.usuario.username, req.usuario.email);
                    res.result = true;
                }
                else
                {
                    laConexion.SP_HACER_USUARIO_ADMIN(req.usuario.id);
                    res.listaDeErrores = new List<String>();
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

        public ResEliminarUsuario eliminarUsuario(ReqEliminarUsuario req)
        {
            ResEliminarUsuario res = new ResEliminarUsuario();
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
                    laConexion.SP_ELIMINAR_USUARIO(req.id);
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

        public ResObtenerTodosLosUsuarios obtenerTodosLosUsuarios(ReqObtenerTodosLosUsuarios req)
        {
            ResObtenerTodosLosUsuarios res = new ResObtenerTodosLosUsuarios();
            try
            {

                conexionLinqDataContext laConexion = new conexionLinqDataContext();
                List<SP_VER_TODOS_USUARIOSResult> result = laConexion.SP_VER_TODOS_USUARIOS().ToList();
                if (result.Count != 0)
                {
                    res.listaDeUsuarios = this.armarListaDeUsuarios(result);

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
                res.listaDeErrores = new List<string>();
                res.listaDeErrores.Add(ex.Message);
                res.result = false;
            }
            return res;
        }

        private List<Usuario> armarListaDeUsuarios(List<SP_VER_TODOS_USUARIOSResult> listaDeUsuariosLinq)
        {
            List<Usuario> listaDeUsuariosArmada = new List<Usuario>();
            foreach (SP_VER_TODOS_USUARIOSResult elementoLinq in listaDeUsuariosLinq)
            {
                Usuario elUsuario = new Usuario();
                elUsuario.id = elementoLinq.id;
                elUsuario.username = elementoLinq.username;
                elUsuario.status = elementoLinq.status;
                elUsuario.isAdmin = elementoLinq.isAdmin;
                elUsuario.email = elementoLinq.email;

                listaDeUsuariosArmada.Add(elUsuario);
            }
            return listaDeUsuariosArmada;
        }
    }

   


}