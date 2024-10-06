using Microsoft.AspNetCore.Mvc;
using pj_final.Models;
using pj_final.Datos;
using pj_final.Servicios;

namespace pj_final.Controllers
{
    public class InicioController : Controller
    {
        //GET : Inicio
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            //de esta forma enviamos la clave ya convertida en SHA256
            users usuario = DBusers.validar(email, UtilidadServicio.ConvertirSHA256(password));

            if (usuario != null)
            {
                if (!usuario.confirmado)
                {
                    ViewBag.Mensaje = $"Falta confirmar su cuenta. Se le envió un correo a {email}";
                }
                else if (usuario.restablecer)
                {
                    ViewBag.Mensaje = $"Se ha solicitado restablecer su cuenta, por favor revise su bandeja de correo {email}";
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias con esas credenciales";
            }

            return View();
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(users usuario)
        {
            if (usuario.password != usuario.confirmar_clave)
            {
                ViewBag.Nombre = usuario.full_name;
                ViewBag.Email = usuario.email;
                ViewBag.Mensaje = "Las contraseñas no coinciden";

                return View();
            }

            if (DBusers.obtener(usuario.email) == null)
            {
                usuario.password = UtilidadServicio.ConvertirSHA256(usuario.password);
                usuario.token = UtilidadServicio.GenerarToken();
                usuario.restablecer = false;
                usuario.confirmado = false;
                bool respuesta = DBusers.registrar(usuario);

                if (respuesta)
                {
                    //string path = HttpContext.Server.MapPath("~/Plantilla/Confirmar.html");
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "Plantilla", "Confirmar.html");
                    string content = System.IO.File.ReadAllText(path);
                    string url = $"{Request.Scheme}://{Request.Host}/Inicio/Confirmar?token={usuario.token}";

                    string HtmlBody = string.Format(content, usuario.full_name, url);

                    CorreoDTO correoDTO = new CorreoDTO()
                    {
                        Para = usuario.email,
                        Asunto = "Correo confirmacion",
                        Contenido = HtmlBody,
                    };

                    bool enviado = CorreoServicio.Enviar(correoDTO);
                    ViewBag.Creado = true;
                    ViewBag.Mensaje = $"Su cuenta ha sido creada, hemos enviado un mensaje al correo {usuario.email} para confirmar su cuenta";
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo crear la cuenta";
                }
            }
            else
            {
                ViewBag.Mensaje = "El correo ya está registrado";

            }
            return View();
        }
        public IActionResult Confirmar(string token)
        {
            ViewBag.Respuesta = DBusers.confirmar(token);
            return View();
        }
        public IActionResult Restablecer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Restablecer(string email)
        {
            users usuario = DBusers.obtener(email);
            ViewBag.Correo = email;
            if (usuario != null)
            {
                bool respuesta = DBusers.restablecerActualizar(1, usuario.password, usuario.token);

                if (respuesta)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "Plantilla", "Restablecer.html");
                    string content = System.IO.File.ReadAllText(path);
                    string url = $"{Request.Scheme}://{Request.Host}/Inicio/Actualizar?token={usuario.token}";

                    string HtmlBody = string.Format(content, usuario.full_name, url);

                    CorreoDTO correoDTO = new CorreoDTO()
                    {
                        Para = email,
                        Asunto = "Restablecer cuenta",
                        Contenido = HtmlBody,
                    };

                    bool enviado = CorreoServicio.Enviar(correoDTO);
                    ViewBag.Restablecido = true;
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo restablecer la cuenta";
                }
            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias con el correo";
            }
            return View();
        }
        public IActionResult Actualizar(string token)
        {
            ViewBag.Token = token;
            return View();

        }

        [HttpPost]
        public IActionResult Actualizar(string token, string password, string confirmar_clave) 
        {
            ViewBag.Token = token;
            if (password != confirmar_clave)
            {
                ViewBag.Mensaje = "La contraseña no coincide";
                return View();
            }

            bool respuesta = DBusers.restablecerActualizar(0, UtilidadServicio.ConvertirSHA256(password),token);

            if (respuesta)
                ViewBag.Restablecido = true;
            else
                ViewBag.Mensaje = "No se pudo actualizar";
            return View();


        }
    }
}
