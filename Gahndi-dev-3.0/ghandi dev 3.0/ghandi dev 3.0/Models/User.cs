using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL;

namespace ghandi_dev_3._0.Models
{
    public class User
    {
        int userId;
        string email;
        string password;
        string fullName;
        string dateOfBirth;
        string gender;
        string address;
        string locationX;
        string locationY;
        int personalStatus;
        string profilePicUrl;
        int coinsCredit;
        string city;
        int validationQuestion;
        string validationAnswer;
        List<int> defaultDepartments;
        List<int> likedItemsList;
        int userScore;
      
        

        public int UserId { get => userId; set => userId = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string FullName { get => fullName; set => fullName = value; }
        public string DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Address { get => address; set => address = value; }
        public string LocationX { get => locationX; set => locationX = value; }
        public string LocationY { get => locationY; set => locationY = value; }
        public int PersonalStatus { get => personalStatus; set => personalStatus = value; }
        public string ProfilePicUrl { get => profilePicUrl; set => profilePicUrl = value; }
        public int CoinsCredit { get => coinsCredit; set => coinsCredit = value; }
        public string City { get => city; set => city = value; }
        public int ValidationQuestion { get => validationQuestion; set => validationQuestion = value; }
        public string ValidationAnswer { get => validationAnswer; set => validationAnswer = value; }
        public List<int> DefaultDepartments { get => defaultDepartments;  }
        public int UserScore { get => userScore; set => userScore = value; }
        public List<int> LikedItemsList { get => likedItemsList; set => likedItemsList = value; }

        public User() { }
      
        public User(int userId, string email, string password, string fullName, string dateOfBirth, string gender, string address, string locationX, string locationY, int personalStatus, string profilePicUrl, int coinsCredit, string city, int validationQuestion, string validationAnswer)
        {
            UserId = userId;
            Email = email;
            Password = password;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            LocationX = locationX;
            LocationY = locationY;
            PersonalStatus = personalStatus;
            ProfilePicUrl = profilePicUrl;
            CoinsCredit = coinsCredit;
            City = city;
            ValidationQuestion = validationQuestion;
            ValidationAnswer = validationAnswer;
            defaultDepartments = new List<int>();
            likedItemsList = new List<int>();
        }

        public enum ExtraPoints
        {
            birthdayCoins = 50,
            shareCoins = 30,
            reviewCoins = 30,
        }
        public User Read(string email, string password)
        {
            DataServices ds = new DataServices();
            User user = ds.ReadUser(email, password);
            return user;
        }

        public User CheckUserExists(string email)
        {
            DataServices ds = new DataServices();
            User user = ds.CheckUserExists(email);
            return user;
        }

        public int Insert()
        {
            DataServices ds = new DataServices();
            return ds.InsertUser(this);
        }

        public int InsertWatchedItems(List<int> watchedItems, int userID)
        {
            DataServices ds = new DataServices();
            return ds.InsertWatchedItems(watchedItems, userID);
        }

        public User InsertLikeItems(int likedItemId, int userId, char likeDirection)
        {
            DataServices ds = new DataServices();
            return ds.InsertLikedItems(likedItemId, userId, likeDirection);
        }

    

        public void UpdateUnlikedItems(int unlikedItemsId, int userId)
        {
            DataServices ds = new DataServices();
            ds.UpdateUnlikedItems(unlikedItemsId, userId);
        }

        public int ChangePass(string uEmail, string newPassword)
        {
            DataServices ds = new DataServices();
            return ds.ChangeUserPassword(uEmail, newPassword);
        }

        public List<Item> UserItemsBought(int userId)
        {
            DataServices ds = new DataServices();
            return ds.GetUserBoughtItems(userId);
        }

        public List<Item> GetUserSellItems(int userId)
        {
            DataServices ds = new DataServices();
            return ds.GetUserSellItems(userId);
        }

        public List<Item> GetUserSoldItems(int userId)
        {
            DataServices ds = new DataServices();
            return ds.GetUserSoldItems(userId);
        }

        public List<Item> GetUserLikedItems(int userId)
        {
            DataServices ds = new DataServices();
            return ds.GetUserLikedItems(userId);
        }

        public List<Item> GetUserInterstedItems(int userId)
        {
            DataServices ds = new DataServices();
            return ds.GetUserInterstedItems(userId);
        }

        public int UpdatePoints(int userId, List<string> reasons)
        {
            int addedPoint = 0;
            foreach (var reason in reasons)
            {
                switch(reason)
                {
                    case "BDay":
                        addedPoint += (int)ExtraPoints.birthdayCoins;
                        break;

                    case "Share":
                        addedPoint += (int)ExtraPoints.shareCoins;
                        break;

                    case "Review":
                        addedPoint += (int)ExtraPoints.reviewCoins;
                        break;
                }
            }
            DataServices ds = new DataServices();
            return ds.UpdatePoint(userId, addedPoint);
        }

        public int UpdateProfilePice(int userId, List<string> pics)
        {
            DataServices ds = new DataServices();
            return ds.UpdateProfilePice(userId, pics);
        }

        public void InsertInterestedItem(int userId, int itemId)
        {
            DataServices ds = new DataServices();
            ds.InsertInterestedItem(userId, itemId);
        }


    }
}