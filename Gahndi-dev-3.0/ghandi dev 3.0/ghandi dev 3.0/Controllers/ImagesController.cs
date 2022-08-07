using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using ghandi_dev_3._0.Models;

namespace ghandi_dev_3._0.Controllers
{
    public class imageController : ApiController
    {

        //[HttpPost]
        //[Route("api/image/uploadimage")] //front end url for posting photos
        public HttpResponseMessage Post()
        {
            List<string> imageLinks = new List<string>();
            var httpContext = HttpContext.Current;
            int id = 0;
            bool isUser = false;
            if (httpContext.Request.Files.Count > 0)
            {
                for (int i = 0; i < httpContext.Request.Files.Count; i++)
                {
                    //Create folders for items and users link below:
                    //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-create-a-file-or-folder
                    HttpPostedFile httpPostedFile = httpContext.Request.Files[i];
                    if (httpPostedFile != null)
                    {
                        string fname = httpPostedFile.FileName;
                        string[] words = fname.Split(' ');
                        id = Int32.Parse(words[1]);
                        string tmpImageLink = "";
                        if (words[0] == "Item")
                        {
                            var fileSavePath = Path.Combine(HostingEnvironment.MapPath("~/Images"), fname);
                            httpPostedFile.SaveAs(fileSavePath);
                            tmpImageLink = "Images/Items/" + id + "/" + fname;
                        }
                        else if (words[0] == "UsersProfilePics")
                        {
                            isUser = true;
                            var fileSavePath = Path.Combine(HostingEnvironment.MapPath("~/Images"), fname);
                            httpPostedFile.SaveAs(fileSavePath);
                            tmpImageLink = "Images/Users/" + id + "/" + fname;
                        }
                        imageLinks.Add(tmpImageLink);
                    }
                }
                if (isUser)
                {
                    User u = new User();
                    u.UpdateProfilePice(id, imageLinks);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Created, imageLinks);
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }


        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}