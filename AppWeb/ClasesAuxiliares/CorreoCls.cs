using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace AppWeb.ClasesAuxiliares
{
    public class CorreoCls
    {
        public static int EnviarCorreo(string NombreCorreo, string Asunto, string Contenido)
        {
            int Respuesta = 0;
            try
            {
                string Correo = ConfigurationManager.AppSettings["Correo"];
                string Clave = ConfigurationManager.AppSettings["Clave"];
                string Servidor = ConfigurationManager.AppSettings["Servidor"];
                int Puerto = int.Parse(ConfigurationManager.AppSettings["Puerto"]);
                //Datos Correo
                MailMessage mail = new MailMessage();
                mail.Subject = Asunto + " :)";
                mail.IsBodyHtml = true;
                mail.Body = "<h1>" + Contenido + "</h1>";
                mail.From = new MailAddress(Correo);
                mail.To.Add(new MailAddress(NombreCorreo));
                //Envío de correo
                SmtpClient smtp = new SmtpClient();
                smtp.Host = Servidor;
                smtp.EnableSsl = true;
                smtp.Port = Puerto;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Correo, Clave);
                smtp.Send(mail);
                Respuesta = 1;
            }
            catch (Exception ex)
            {
                string path = "C:\\@Jp\\AppWeb\\AppWeb\\Archivos\\Log.txt";
                string error = ex.ToString();
                using (StreamWriter writetext = new StreamWriter(path))
                {
                    writetext.WriteLine(error);
                }

                Respuesta = 0;
            }
            return Respuesta;
        }
    }
}