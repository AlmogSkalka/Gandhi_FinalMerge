using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghandi_dev_3._0.Models;



namespace ghandi_dev_3._0.Controllers
{
    public class UsersController : ApiController
    {
        // GET api/<controller>
        public List<QandA> Get()
        {
            QandA QandA = new QandA();
            return QandA.GetAllQ();
        }

        // GET api/<controller>/5
        public User Get(string email, string password)
        {
            User user = new User();
            return user.Read(email, password);
        }
        //for google/ facebook users
        public User Get(string email)
        {
            User user = new User();
            return user.CheckUserExists(email);
        }

        public List<Item> Get(int userId)
        {
            User user = new User();
            return user.UserItemsBought(userId);
        }

        [HttpGet]
        [Route("api/Users/GetUserSellItems")]
        public List<Item> GetUserSellItems(int userId)
        {
            User user = new User();
            return user.GetUserSellItems(userId);
        }

        [HttpGet]
        [Route("api/Users/GetUserSoldItems")]
        public List<Item> GetUserSoldItems(int userId)
        {
            User user = new User();
            return user.GetUserSoldItems(userId);
        }

        [HttpGet]
        [Route("api/Users/GetUserLikedItems")]
        public List<Item> GetUserLikedItems(int userId)   // Read user's liked items list
        {
            User user = new User();
            return user.GetUserLikedItems(userId);
        }

        [HttpGet]
        [Route("api/Users/GetUserInterstedItems")]
        public List<Item> GetUserInterstedItems(int userId)   // Read user's liked items list
        {
            User user = new User();
            return user.GetUserInterstedItems(userId);
        }

        // POST api/<controller>
        public int Post([FromBody]User user)          
        {
            return user.Insert();
        }

        //http routing demo
        //public int Messi([FromBody] User user)
        //{
        //    return user.Insert();
        //}
        public int Post([FromBody] List<int> watchedItems, int userId)
        {
            User user = new User();
            return user.InsertWatchedItems(watchedItems, userId);
        }

        
      
        public User Post(int likedItemId, int userId, char likeDirection)
        {
            User user = new User();
            return user.InsertLikeItems(likedItemId, userId, likeDirection);
        }

        public void Post(int userId, int itemId)
        {
            User user = new User();
            user.InsertInterestedItem(userId,itemId);
        }

        // PUT api/<controller>/5
        public int Put([FromBody]string newPassword, string Uemail)
        {
            User user = new User();
            return user.ChangePass(Uemail,newPassword);
        }
        public int Put([FromBody] List<string> reasons, int userId)
        {
            User user = new User();
            return user.UpdatePoints(userId, reasons);
        }

        public void Put([FromBody] int unlikedItemsId, int userId)
        {
            User user = new User();
            user.UpdateUnlikedItems(unlikedItemsId, userId);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}