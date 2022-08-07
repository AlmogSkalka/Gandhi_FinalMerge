using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL;

namespace ghandi_dev_3._0.Models
{
    public class Category
    {
        int categoryId;
        string categoryDesc;
        int departmentId;
        int numOfItemsInCategory;
        string categoryPicUrl;

        public Category() { }

        public Category(int categoryId, string categoryDesc, int departmentId, string categoryPicUrl)
        {
            CategoryId = categoryId;
            CategoryDesc = categoryDesc;
            DepartmentId = departmentId;
            CategoryPicUrl = categoryPicUrl;
            NumOfItemsInCategory = 0;
        }

        public int CategoryId { get => categoryId; set => categoryId = value; }
        public string CategoryDesc { get => categoryDesc; set => categoryDesc = value; }
        public int DepartmentId { get => departmentId; set => departmentId = value; }
        public int NumOfItemsInCategory { get => numOfItemsInCategory; set => numOfItemsInCategory = value; }
        public string CategoryPicUrl { get => categoryPicUrl; set => categoryPicUrl = value; }

        public List<Category> ReadCategories() {

            DataServices ds = new DataServices();
            List<Category> categories = ds.ReadCategories();
            return categories;
        }
    }
}