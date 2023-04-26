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
    [Route("correo-bienvenida")]
    [HttpPost]
    public async Task<ActionResult> enviarCorreoBienvenida(ModeloCorreo datos)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);
        SendGridMessage msg = this.crearMensajeBase(datos);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("WELCOME_SENDGRID_TEMPLATE"));
        msg.SetTemplateData(new
        {
            name = datos.nombreDestino,
            message = "Bienvenido a la comunidad de la inmobiliaria"
        });
        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return Ok("Correo enviado a " + datos.correoDestino);
        }
        else
        {
            return BadRequest("Error al enviar el correo a " + datos.correoDestino);
        }
    }

    [Route("correo-recuperacion")]
    [HttpPost]
    public async Task<ActionResult> enviarCorreoRecuperacion(ModeloCorreo datos)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);
        SendGridMessage msg = this.crearMensajeBase(datos);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("WELCOME_SENDGRID_TEMPLATE"));
        msg.SetTemplateData(new
        {
            name = datos.nombreDestino,
            message = "Esta es la nueva clave... si"
        });
        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return Ok("Correo enviado a " + datos.correoDestino);
        }
        else
        {
            return BadRequest("Error al enviar el correo a " + datos.correoDestino);
        }
    }

    [Route("enviar-correo-2fa")]
    [HttpPost]
    public async Task<ActionResult> enviarCorreo2FA(ModeloCorreo datos)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);
        
        SendGridMessage msg = this.crearMensajeBase(datos);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("TWOFA_SENDGRID_TEMPLATE"));
        msg.SetTemplateData(new
        {
            nombre = datos.nombreDestino,
            mensaje = datos.contenidoCorreo,
            asunto = datos.asuntoCorreo
        });
        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return Ok("Correo enviado a " + datos.correoDestino);
        }
        else
        {
            return BadRequest("Error al enviar el correo a " + datos.correoDestino);
        }
    }

    private SendGridMessage crearMensajeBase(ModeloCorreo datos)
    {

        var from = new EmailAddress(Environment.GetEnvironmentVariable("EMAIL_FROM"), Environment.GetEnvironmentVariable("NAME_FROM"));
        var subject = datos.asuntoCorreo;
        var to = new EmailAddress(datos.correoDestino, datos.nombreDestino);
        var plainTextContent = datos.contenidoCorreo;
        var htmlContent = datos.contenidoCorreo;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        return msg;
    }





}
