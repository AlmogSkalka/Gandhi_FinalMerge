using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghandi_dev_3._0.Models;

namespace ghandi_dev_3._0.Controllers
{
    public class PersonalCostomizationController : ApiController
    {

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public List<Item> Post([FromBody] User user)
        {
            PersonalCostomization pc = new PersonalCostomization();
            return pc.FindSimilarUsers(user);
        }
       
        public List<Item> Post ([FromBody] User user, int userId)
        {
            PersonalCostomization pc = new PersonalCostomization();
            return pc.mostWatchedItems(user);
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