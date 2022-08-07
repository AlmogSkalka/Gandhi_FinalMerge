using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghandi_dev_3._0.Models;

namespace ghandi_dev_3._0.Controllers
{
    public class QandAController : ApiController
    {
        // GET api/<controller>
       

        // GET api/<controller>/5
        public QandA Get(string Uemail)
        {
            QandA QandA = new QandA();
            return QandA.ReadQandA(Uemail); 
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
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