using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL;

namespace ghandi_dev_3._0.Models
{

    public class PersonalCostomization //ziv you dyslexic batata ma kvar bikashti, that you'll write something properly????? ***CUSTOMIZATION***
    {
        List<int> similarUsers;
        List<int> DefaultDepartments;
        List<Tag> similarUsersTags;
        List<Tag> usersTags;
        List<Tag> userWatchListTags;
        List<Tag> userLikedItemsTags;
        List<Tag> scoredTags;
        List<Tag> finalTagsList;
        List<Item> matchingItems;
        List<Item> dBItems;
        List<Item> WatchedItems;

        User currentUser;



        public List<int> SimilarUsers { get => similarUsers; set => similarUsers = value; }
        public List<Tag> SimilarUsersTags { get => similarUsersTags; set => similarUsersTags = value; }
        public List<Tag> UsersTags { get => usersTags; set => usersTags = value; }
        public List<Tag> UserWatchListTags { get => userWatchListTags; set => userWatchListTags = value; }
        public User CurrentUser { get => currentUser; set => currentUser = value; }


        public List<Tag> ScoredTags { get => scoredTags; set => scoredTags = value; }
        public List<Item> MatchingItems { get => matchingItems; set => matchingItems = value; }
        public List<Item> DBItems { get => dBItems; set => dBItems = value; }
        public List<Item> WatchedItems1 { get => WatchedItems; set => WatchedItems = value; }
        public List<int> DefaultDepartments1 { get => DefaultDepartments; set => DefaultDepartments = value; }
        public List<Tag> UserLikedItemsTags { get => userLikedItemsTags; set => userLikedItemsTags = value; }
        public List<Tag> FinalTagsList { get => finalTagsList; set => finalTagsList = value; }

        public PersonalCostomization()
        {
            SimilarUsers = new List<int>();
            SimilarUsersTags = new List<Tag>();
            UsersTags = new List<Tag>();
            UserWatchListTags = new List<Tag>();
            UserLikedItemsTags = new List<Tag>();
            CurrentUser = new User();
            ScoredTags = new List<Tag>();
            FinalTagsList = new List<Tag>();
            MatchingItems = new List<Item>();
            DBItems = new List<Item>();
            WatchedItems1 = new List<Item>();
            DefaultDepartments = new List<int>();
        }

        public enum Departments
        {
            Women = 1,
            Men = 2,
            Kids = 3
        }

        public enum Scoreing
        {
            twoYearsDifferenceGrade = 20,
            fiveYearsDifferenceGrade = 10,
            tenYearsDifferenceGrade = 5,
            personalStatusGrade = 10,
            similarUserTagScore = 20,
            additionalsimilarUserTagScore = 10,
            userTagScore = 40,
            additionalUserTagScore = 20,
            userWatchTagScore = 60,
            userLikedItemTagScore = 90,
            additionalUserWatchTagScore = 30,
            additionalUserLikedItemTagScore = 45,
            enoughNumbersOfItems = 13,
            minimumAverageScoreOfItems = 4
        }

        //finding similar Users
        public List<Item> FindSimilarUsers(User user)
        {
            CurrentUser = user;
            switch (CurrentUser.Gender)
            {
                case "f":
                    DefaultDepartments.Add((int)Departments.Women);
                    break;

                case "m":
                    DefaultDepartments.Add((int)Departments.Men);
                    break;
            }

            if (CurrentUser.PersonalStatus == 1) //has kids
                DefaultDepartments.Add((int)Departments.Kids);

            DataServices ds = new DataServices();
            List<User> DBusers = ds.GetUsers();

            var UserBirthyear = DateTime.Parse(user.DateOfBirth).Year;
            int sumScore = 0;
            int usersCounter = 0;

            for (int i = 0; i < DBusers.Count; i++)
            {
                if (DBusers[i].Gender == user.Gender && DBusers[i].UserId != CurrentUser.UserId) // only if gender matches
                {
                    if (DBusers[i].PersonalStatus == user.PersonalStatus) // if the same personal status
                        DBusers[i].UserScore += (int)Scoreing.personalStatusGrade;

                    //scoring by age difference
                    var DBUserBirthyear = DateTime.Parse(DBusers[i].DateOfBirth).Year;
                    if (Math.Abs(UserBirthyear - DBUserBirthyear) <= 2)
                        DBusers[i].UserScore += (int)Scoreing.twoYearsDifferenceGrade;
                    else if (Math.Abs(UserBirthyear - DBUserBirthyear) <= 5)
                        DBusers[i].UserScore += (int)Scoreing.fiveYearsDifferenceGrade;
                    else if (Math.Abs(UserBirthyear - DBUserBirthyear) <= 10)
                        DBusers[i].UserScore += (int)Scoreing.tenYearsDifferenceGrade;
                    sumScore += DBusers[i].UserScore;
                    usersCounter += 1;
                }
            }
            int usersAverage = sumScore / usersCounter;
            foreach (var u in DBusers)
            {
                if (u.UserScore >= usersAverage)
                    SimilarUsers.Add(u.UserId); /// adding the user for the list of similiar
            }
            if (similarUsers != null)
                FindSimilarUsersTags();
            else FindUsersItemTags();
            return matchingItems;
        }


        //finding all similar users tags and scoring them 
        public void FindSimilarUsersTags()
        {
            DataServices ds = new DataServices();
            List<Tag> tmpTags = new List<Tag>();
            tmpTags = ds.GetUserTags(SimilarUsers);
            if (tmpTags != null)
            {
                foreach (var tag in tmpTags)
                {
                    int dups = 0;
                    for (int i = 0; i < SimilarUsersTags.Count; i++)
                    {
                        if (SimilarUsersTags[i].TagId == tag.TagId)
                        {
                            dups += 1;
                            SimilarUsersTags[i].TagScore += (int)Scoreing.additionalsimilarUserTagScore;
                        }
                    }
                    if (dups == 0)
                    {
                        tag.TagScore = (int)Scoreing.similarUserTagScore; //initial score and adding to list
                        SimilarUsersTags.Add(tag);
                    }
                }
            }
            FindUsersItemTags();
        }


        //finding users purchesed items tags
        public void FindUsersItemTags()
        {
            DataServices ds = new DataServices();
            List<int> user = new List<int>();
            user.Add(CurrentUser.UserId);
            List<Tag> tmpUserTags = new List<Tag>();
            tmpUserTags = ds.GetUserTags(user);
            foreach (var tag in tmpUserTags)
            {
                int dups = 0;
                for (int i = 0; i < UsersTags.Count; i++)
                {
                    if (UsersTags[i].TagId == tag.TagId)
                    {
                        dups += 1;
                        UsersTags[i].TagScore += (int)Scoreing.additionalUserTagScore;
                    }
                }
                if (dups == 0)
                {
                    tag.TagScore = (int)Scoreing.userTagScore; //initial score and adding to list
                    UsersTags.Add(tag);
                }
            }
            FindUserWatchListTags();
        }


        public void FindUserWatchListTags()
        {
            DataServices ds = new DataServices();
            List<Tag> tmpUserWatchTags = new List<Tag>();
            tmpUserWatchTags = ds.FindUserWatchListTags(CurrentUser.UserId);
            if (tmpUserWatchTags != null)
            {
                foreach (var tag in tmpUserWatchTags)
                {
                    int dups = 0;
                    for (int i = 0; i < UserWatchListTags.Count; i++)
                    {
                        if (UserWatchListTags[i].TagId == tag.TagId)
                        {
                            dups += 1;
                            UserWatchListTags[i].TagScore += (int)Scoreing.additionalUserWatchTagScore;
                        }
                    }
                    if (dups == 0)
                    {
                        tag.TagScore = (int)Scoreing.userWatchTagScore; //initial score and adding to list
                        UserWatchListTags.Add(tag);
                    }

                }
            }
            findUserLikedItemsTags();
        }

        public void findUserLikedItemsTags()
        {
            DataServices ds = new DataServices();
            List<Tag> tmpUserLikedItemsTags = new List<Tag>();
            tmpUserLikedItemsTags = ds.FindUserLikedItemsListTags(CurrentUser.UserId);
            if (tmpUserLikedItemsTags != null)
            {
                foreach (var tag in tmpUserLikedItemsTags)
                {
                    int dups = 0;
                    for (int i = 0; i < UserLikedItemsTags.Count; i++)
                    {
                        if (UserLikedItemsTags[i].TagId == tag.TagId)
                        {
                            dups += 1;
                            UserLikedItemsTags[i].TagScore += (int)Scoreing.additionalUserLikedItemTagScore;
                        }
                    }
                    if (dups == 0)
                    {
                        tag.TagScore = (int)Scoreing.userLikedItemTagScore; //initial score and adding to list
                        UserLikedItemsTags.Add(tag);
                    }
                }
            }
            CalculateTagsScore();
        }


        public void CalculateTagsScore()
        {
            ScoredTags = similarUsersTags.OrderBy(o => o.TagId).ToList(); ;
            List<Tag> BoughtItemsUserTagsOrderByTagId = UsersTags.OrderBy(o => o.TagId).ToList();
            List<Tag> UserWatchListTagsOrderByTagId = userWatchListTags.OrderBy(o => o.TagId).ToList();
            List<Tag> UserLikedItemsTagsOrderByTagId = UserLikedItemsTags.OrderBy(o => o.TagId).ToList();


            foreach (var tag in BoughtItemsUserTagsOrderByTagId)
            {
                int count = 0;
                for (int i = 0; i < ScoredTags.Count; i++)
                {
                    if (ScoredTags[i].TagId == tag.TagId)
                    {
                        ScoredTags[i].TagScore += tag.TagScore;
                        count = 1;
                        break;
                    }
                }
                if (count == 0)
                    ScoredTags.Add(tag);
            }


            List<Tag> tmpList = ScoredTags.OrderBy(o => o.TagId).ToList();
            foreach (var tag in UserWatchListTagsOrderByTagId)
            {
                int count = 0;
                for (int i = 0; i < tmpList.Count; i++)
                {
                    if (tmpList[i].TagId == tag.TagId)
                    {
                        tmpList[i].TagScore += tag.TagScore;
                        count = 1;
                        break;
                    }
                }
                if (count == 0)
                    tmpList.Add(tag);

            }
            ScoredTags = tmpList.OrderBy(o => o.TagId).ToList();



            foreach (var tag in UserLikedItemsTagsOrderByTagId)
            {
                int count = 0;
                for (int i = 0; i < ScoredTags.Count; i++)
                {
                    if (ScoredTags[i].TagId == tag.TagId)
                    {
                        ScoredTags[i].TagScore += tag.TagScore;
                        count = 1;
                        break;
                    }
                }
                if (count == 0)
                    ScoredTags.Add(tag);
            }

            FinalTagsList = scoredTags.OrderBy(o => o.TagId).ToList();
            CalculateItemsScore();
        }

        public void CalculateItemsScore()
        {
            DataServices ds = new DataServices();
            DBItems = ds.ReadItems();
            int itemsSumScore = 0;
            int itemsCounter = 0;

            foreach (var item in DBItems)
            {
                if (item.SellerId != currentUser.UserId)
                {
                    foreach (var tag in item.DBtags)
                    {
                        for (int i = 0; i < FinalTagsList.Count; i++)
                        {
                            if (tag.TagId == FinalTagsList[i].TagId)
                            {
                                item.ItemScore += FinalTagsList[i].TagScore;
                                break;
                            }
                        }
                    }
                }
                itemsSumScore += item.ItemScore;
                itemsCounter += 1;

            }
            int itemsAverageScore = itemsSumScore / itemsCounter;

            foreach (var item in DBItems)
            {
                if (item.ItemScore >= itemsAverageScore && DefaultDepartments.Contains(item.DepartmentID))
                    matchingItems.Add(item);
            }


            // adding items to algo if not have 12 items at least 
            while (matchingItems.Count < (int)Scoreing.enoughNumbersOfItems && itemsAverageScore > (int)Scoreing.minimumAverageScoreOfItems)
            {
                itemsAverageScore = itemsAverageScore / 2;
                foreach (var item in DBItems)
                {
                    if (!matchingItems.Contains(item))
                        if (item.ItemScore >= itemsAverageScore && DefaultDepartments.Contains(item.DepartmentID))
                            matchingItems.Add(item);
                }
            }
        }


        public List<Item> mostWatchedItems(User user)
        {
            DataServices ds = new DataServices();
            DBItems = ds.ReadItems();
            CurrentUser = user;
            if (CurrentUser.Gender == "f") //women
                DefaultDepartments.Add(1);

            else if (CurrentUser.Gender == "m")
                DefaultDepartments.Add(2); //men

            if (CurrentUser.PersonalStatus == 1) //has kids
                DefaultDepartments.Add(3);

            int sumWatch = 0;
            int numOfItemsInDep = 0;
            List<Item> tmpItems = new List<Item>();
            if (DBItems.Count >= 100)
            {
                foreach (var item in DBItems)
                {
                    if (DefaultDepartments.Contains(item.DepartmentID))
                    {
                        sumWatch += item.ViewCounter;
                        numOfItemsInDep += 1;
                    }
                }

                int viewsAverageScore = sumWatch / numOfItemsInDep;

                foreach (var item in DBItems)
                {
                    if (item.ViewCounter >= viewsAverageScore && DefaultDepartments.Contains(item.DepartmentID))
                        WatchedItems1.Add(item);
                }
                tmpItems = WatchedItems1.OrderBy(o => o.ViewCounter).ToList();
                tmpItems.Reverse();
            }
            if (DBItems.Count < 100)
            {
                foreach (var item in DBItems)
                {
                    if (DefaultDepartments.Contains(item.DepartmentID))
                    {
                        WatchedItems1.Add(item);
                    }
                }
                tmpItems = WatchedItems1.OrderBy(o => o.ViewCounter).ToList();
                tmpItems.Reverse();
            }
            return tmpItems;
        }

        public List<Item> FilterItems(User user)
        {
            DataServices ds = new DataServices();
            List<Item> DBItems = ds.ReadItems();
            List<Item> filtered = new List<Item>();
            CurrentUser = user;

            if (CurrentUser.Gender == "f")
                DefaultDepartments.Add((int)(Departments.Women));
            else if (CurrentUser.Gender == "m")
                DefaultDepartments.Add((int)(Departments.Men));
            if (CurrentUser.PersonalStatus == 1) //has kids
                DefaultDepartments.Add((int)(Departments.Kids));

            foreach (var item in DBItems)
            {
                if (item.SellerId != user.UserId)
                {
                    if (DefaultDepartments.Contains(item.DepartmentID))
                        filtered.Add(item);

                }
            }
            return filtered;
        }
    }
}