using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghandi_dev_3._0.Models;

namespace ghandi_dev_3._0.Controllers
{
    public class ItemsController : ApiController
    {
        // GET api/<controller>
        public List<Item> Get()
        {
            Item item = new Item();
            return item.ReadAllItems();
        }

        public Item Get(int itemId)
        {
            Item item = new Item();
            return item.ReadItem(itemId);
        }


        [HttpPost]
        [Route("api/Items/GetInterestedUsersList")]
        public List<User> GetInterestedUsersList(int itemId)
        {
            Item item = new Item();
            return item.GetInterestedUsersList(itemId);
        }

        // POST api/<controller>
        public List<Item> Post([FromBody] User user)
        {
            Item item = new Item();
            return item.ReadItems(user);
        }
        public int Post([FromBody] Item item, int userId)
        {
            return item.Insert(userId);
        }

        public int Post([FromBody] List<string> imagesUrl, int itemId)
        {
            Item item = new Item();
            return item.InsertImagesUrl(imagesUrl, itemId);
        }

        public void Post(int buyerId, int sellerId, int itemId)
        {
            Item item = new Item();
            item.BuyItemByMoney(buyerId, sellerId, itemId);
        }

        public int Post(int buyerId, int sellerId, int itemId, int price)
        {
            Item item = new Item();
            return item.BuyItemByCoinsCredit(buyerId, sellerId, itemId, price);
        }

        //the function below does not return any value if the transaction was executed succesfully
        //we need to return any value (even '1') so both of the users will know that the transaction took place
        //public VOID Post(int itemId, int userId, char side)   // Make the transaction
        //{
        //    Item item = new Item();
        //    item.MakeTransaction(itemId, userId, side);
        //}

        public int Post(int itemId, int sellerId, int buyerId, char side)   // Make the transaction
        {
            Item item = new Item();
            return item.MakeTransaction(itemId, sellerId, buyerId, side);
        }

        // PUT api/<controller>/5
        public int Put(int itemId, int userId)
        {
            Item item = new Item();
            return item.SellExistingItem(itemId, userId);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}