using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL;
using Newtonsoft.Json.Linq;

namespace ghandi_dev_3._0.Models
{
    public class Item
    {

        int itemId;
        string itemDesc;
        string itemOriginUrl;
        string saleReason;
        int price;
        int colorId;
        string color;
        int sizeId;
        string size;
        string itemStatus;
        int categoryId;
        string category;
        DateTime uploadDate;
        int brandId;
        string brand;
        int viewCounter;
        int departmentID;
        List<string> tagList;
        List<string> itemPhotos;
        List<Tag> dBtags;
        int itemScore;
        int sellerId;
        string locationX;
        string locationY;


        public Item() { }
        //insert
        public Item(string itemDesc, string itemOriginUrl, string saleReason, int price, int colorId, int sizeId, string itemStatus, DateTime uploadDate, int brandId, int viewCounter, int categoryId, List<string> tagList) { 

            ItemDesc = itemDesc;
            ItemOriginUrl = itemOriginUrl;
            SaleReason = saleReason;
            Price = price;
            ColorId = colorId;
            SizeId = sizeId;
            ItemStatus = itemStatus;
            UploadDate = uploadDate;
            //.ToString("yyyy - MM - dd HH: mm:ss.fff");
            BrandId = brandId;
            ViewCounter = viewCounter;
            CategoryId = categoryId;
            TagList = tagList;
        }
        public Item(int itemId, string itemDesc, string itemOriginUrl, string saleReason, int price, int colorId, string color, int sizeId, string size, string itemStatus, int categoryId, string category, DateTime uploadDate, int brandId, string brand, int viewCounter, int departmentID, List<string> tagList, List<string> itemPhotos, List<Tag> dBtags)
        {
            ItemId = itemId;
            ItemDesc = itemDesc;
            ItemOriginUrl = itemOriginUrl;
            SaleReason = saleReason;
            Price = price;
            ColorId = colorId;
            Color = color;
            SizeId = sizeId;
            Size = size;
            ItemStatus = itemStatus;
            CategoryId = categoryId;
            Category = category;
            UploadDate = uploadDate;
            //.ToString("yyyy - MM - dd HH: mm:ss.fff");
            BrandId = brandId;
            Brand = brand;
            ViewCounter = viewCounter;
            DepartmentID = departmentID;
            TagList = tagList;
            ItemPhotos = itemPhotos;
            DBtags = dBtags;
        }
       
        public Item(int itemId, string itemDesc, string itemOriginUrl, string saleReason, int price, string color, string size, string itemStatus, string category, int categoryId, DateTime uploadDate, string brand, int viewCounter, int departmentID)
        {
            ItemId = itemId;
            ItemDesc = itemDesc;
            ItemOriginUrl = itemOriginUrl;
            SaleReason = saleReason;
            Price = price;
            Color = color;
            Size = size;
            ItemStatus = itemStatus;
            Category = category;
            CategoryId = categoryId;
            UploadDate = uploadDate;
            Brand = brand;
            ViewCounter = viewCounter;
            DepartmentID = departmentID;
        }

        public Item(int itemId, string itemDesc, string itemOriginUrl, string saleReason, int price, string color, string size, string itemStatus, string category, DateTime uploadDate, string brand, int viewCounter, int departmentID)
        {
            ItemId = itemId;
            ItemDesc = itemDesc;
            ItemOriginUrl = itemOriginUrl;
            SaleReason = saleReason;
            Price = price;
            Color = color;
            Size = size;
            ItemStatus = itemStatus;
            Category = category;
            UploadDate = uploadDate;
            Brand = brand;
            ViewCounter = viewCounter;
            DepartmentID = departmentID;
        }
        public int ItemId { get => itemId; set => itemId = value; }
        public string ItemDesc { get => itemDesc; set => itemDesc = value; }
        public string ItemOriginUrl { get => itemOriginUrl; set => itemOriginUrl = value; }
        public string SaleReason { get => saleReason; set => saleReason = value; }
        public int Price { get => price; set => price = value; }
        public int ColorId { get => colorId; set => colorId = value; }
        public string Color { get => color; set => color = value; }
        public int SizeId { get => sizeId; set => sizeId = value; }
        public string Size { get => size; set => size = value; }
        public string ItemStatus { get => itemStatus; set => itemStatus = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; }
        public string Category { get => category; set => category = value; }
        public DateTime UploadDate { get => uploadDate; set => uploadDate = value; }
        public int BrandId { get => brandId; set => brandId = value; }
        public string Brand { get => brand; set => brand = value; }
        public int ViewCounter { get => viewCounter; set => viewCounter = value; }
        public int DepartmentID { get => departmentID; set => departmentID = value; }
        public List<string> TagList { get => tagList; set => tagList = value; }
        public List<string> ItemPhotos { get => itemPhotos; set => itemPhotos = value; }
        public int ItemScore { get => itemScore; set => itemScore = value; }
        public List<Tag> DBtags { get => dBtags; set => dBtags = value; }
        public int SellerId { get => sellerId; set => sellerId = value; }
        public string LocationX { get => locationX; set => locationX = value; }
        public string LocationY { get => locationY; set => locationY = value; }

        public int Insert(int userId)
        {
            DataServices ds = new DataServices();
            return ds.InsertItem(this, userId);
        }

        public int InsertImagesUrl(List<string> imgList, int itemId)
        {
            DataServices ds = new DataServices();
            return ds.InsertItemImages(imgList, itemId);
        }

        public List<Item> ReadItems(User user)
        {
            PersonalCostomization pc = new PersonalCostomization();
            List<Item> items = pc.FilterItems(user);
            return items;
        }
        public List<Item> ReadAllItems()
        {
            DataServices ds = new DataServices();
            List<Item> items = ds.ReadItems();
            return items;
        }
        public Item ReadItem(int itemId)
        {
            DataServices ds = new DataServices();
            Item item = ds.ReadItem(itemId);
            return item;
        }

        public void BuyItemByMoney(int buyerId, int sellerId, int itemId)
        {
            DataServices ds = new DataServices();
            ds.BuyItemByMoney(buyerId,sellerId,itemId);
        }

        public int BuyItemByCoinsCredit(int buyerId, int sellerId, int itemId, int price)
        {
            DataServices ds = new DataServices();
            return ds.BuyItemByCoinsCredit(buyerId, sellerId, itemId, price);
        }
        public int SellExistingItem(int itemId, int userId)
        {
            DataServices ds = new DataServices();
            return ds.SellExistingItem(itemId, userId);
        }

        public int MakeTransaction(int itemId, int sellerId, int buyerId, char side)
        {
            DataServices ds = new DataServices();
            return ds.MakeTransaction(itemId, sellerId, buyerId, side);
        }

        public List<User> GetInterestedUsersList(int itemId)
        {
            DataServices ds = new DataServices();
            return ds.GetInterestedUsersList(itemId);
        }
    }
}