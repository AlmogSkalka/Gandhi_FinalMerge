using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL;


namespace ghandi_dev_3._0.Models
{
    public class Department
    {
        int departmentId;
        string departmentDesc;
        List<Item> itemsListByDep;

        public Department() { }
        public Department(int departmentId, string departmentDesc)
        {
            DepartmentId = departmentId;
            DepartmentDesc = departmentDesc;
        }

        public int DepartmentId { get => departmentId; set => departmentId = value; }
        public string DepartmentDesc { get => departmentDesc; set => departmentDesc = value; }
        public List<Item> ItemListByDep { get => itemsListByDep; set => itemsListByDep = value; }

        public List<Department> ReadDepartments()
        {
            DataServices ds = new DataServices();
            List<Department> departments = ds.ReadDepartments();
            return departments;
        }

        public List<Item> getItemsByDepartment(int departmentId)
        {
            DataServices ds = new DataServices();
            List<Item> itemsList = new List<Item>();
            itemsList= ds.getItemsByDepartment(departmentId);
            itemsListByDep = itemsList;
            return itemsList;
        }

        public List<Category> getTop5Categories(int departmentId)
        {
            List<Item> itemsList = new List<Item>();
            itemsList = getItemsByDepartment(departmentId);

            List<Category> categoriesList = new List<Category>();
            DataServices ds = new DataServices();
            categoriesList = ds.ReadCategoriesByDepartment(departmentId);

            foreach (var item in itemsList)
            {
                foreach (var category in categoriesList)
                {
                    if (category.CategoryDesc == item.Category)
                    {
                        category.NumOfItemsInCategory++;
                        break;
                    }
                }
            }

            List<Category> sortedCategoriesList = categoriesList.OrderByDescending(o => o.NumOfItemsInCategory).ToList();
            List<Category> top5Categories = new List<Category>(sortedCategoriesList.Take(5));
            return top5Categories;
        }
    }
}