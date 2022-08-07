using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghandi_dev_3._0.Models;

namespace ghandi_dev_3._0.Controllers
{
    public class DepartmentsController : ApiController
    {
        // GET api/<controller>
        public List<Department> Get()
        {
            Department department = new Department();
            return department.ReadDepartments();
        }

        public List<Item> Get(int departmentId)
        {
            Department department = new Department();
            return department.getItemsByDepartment(departmentId);
        }

        [HttpGet]
        [Route("api/Departments/GetTopCategories")]
        public List<Category> GetTop5(int departmentId)
        {
            Department department = new Department();
            return department.getTop5Categories(departmentId);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}