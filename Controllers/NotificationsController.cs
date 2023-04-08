using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using ms_notificaciones.Models;

namespace ms_notificaciones.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationsController : ControllerBase
{
    [Route("correo")]
    [HttpPost]
    public async Task<ActionResult> enviarCorreo(ModeloCorreo datos)
    {

        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("juan.1702013712@ucaldas.edu.co","Juan José Gamboa");
        var subject = datos.asuntoCorreo;
        var to = new EmailAddress(datos.correoDestino, datos.nombreDestino);
        var plainTextContent = "plain text content";
        var htmlContent = datos.contenidoCorreo;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);

        if(response.StatusCode == System.Net.HttpStatusCode.Accepted){
            return Ok("Correo enviado a "+datos.correoDestino);
        }else{
            return BadRequest("Error al enviar el correo a " + datos.correoDestino);
        }
    }

}
