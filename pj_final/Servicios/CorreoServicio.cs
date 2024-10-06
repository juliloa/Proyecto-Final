using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using pj_final.Models;


namespace pj_final.Servicios
{
	public static class CorreoServicio
	{
		private static string _Host = "smtp.gmail.com";
		private static int _Puerto = 587;

		private static string _NombreEnvia = "TutsiPop";
		private static string _Correo = "tutsipopcorreo@gmail.com";
		private static string _Clave = "aybhbvdbepmmdbva";

		public static bool Enviar(CorreoDTO correodto)
		{
			try
			{
				var email = new MimeMessage();

				email.From.Add(new MailboxAddress(_NombreEnvia, _Correo));
				email.To.Add(MailboxAddress.Parse(correodto.Para));
				email.Subject = correodto.Asunto;
				email.Body = new TextPart(TextFormat.Html)
				{
					Text = correodto.Contenido
				};

				var smtp = new SmtpClient();
				smtp.Connect(_Host, 465, SecureSocketOptions.SslOnConnect);


				smtp.Authenticate(_Correo, _Clave);
				smtp.Send(email);
				smtp.Disconnect(true);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
