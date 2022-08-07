using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghandi_dev_3._0.Models;

namespace ghandi_dev_3._0.Controllers
{
    public class TagsController : ApiController
    {
        // GET api/<controller>
        public List<Tag> Get()
        {
           Tag tag = new Tag();
            return tag.ReadTags();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public int Post([FromBody] Tag newTag)
        {
            return newTag.InsertNewTag();
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