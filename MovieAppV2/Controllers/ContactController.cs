using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MovieAppV2.Helpers;
using MovieAppV2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security;


namespace MovieAppV2.Controllers
{
    public class ContactController : Controller
    {
        private IConfiguration configuration;
        private IWebHostEnvironment webHostEnviroment;

        public ContactController(IConfiguration _configuration, 
        IWebHostEnvironment _webHostEnviroment)
        {
            configuration = _configuration;
            webHostEnviroment = _webHostEnviroment;
        }

       
        public IActionResult Index()
        {
            return View("Index", new Contact());
        }

        [HttpPost]
       
        public IActionResult Send(Contact contact, IFormFile[] attachments )
        {
            var body = "Name:" + contact.Name + "<br>Address: " + contact.Address +
            "<br>Phone: " + contact.Phone + "<br>Content: " + contact.Content + "<br>" ;
            var mailHelper = new MailHelper(configuration);
            List<string> fileNames = null;
            if(attachments != null && attachments.Length > 0)
            {
                fileNames = new List<string>();
                foreach(IFormFile attachment in attachments)
                {
                    var path = Path.Combine(webHostEnviroment.WebRootPath, "uploads",
                     attachment.FileName);
                     using (var stream = new FileStream (path, FileMode.Create))
                     {
                         attachment.CopyToAsync(stream);    
                     }                 
                     fileNames.Add(path);    
                }   

                
            }

            
           

            if(mailHelper.Send(configuration["Gmail:Username"], contact.Email, 
            contact. Subject,  body,  fileNames))
            {   
                ViewBag.msg = "Sent Mail Succesfully";
            }
            else
            { 
                ViewBag.msg = "Failed";
            }
            return View("Index", new Contact());
        }
    }
}