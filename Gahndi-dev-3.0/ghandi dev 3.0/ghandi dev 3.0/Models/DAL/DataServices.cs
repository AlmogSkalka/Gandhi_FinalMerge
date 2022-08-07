using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ghandi_dev_3._0.Models.DAL
{
    public class DataServices
    {

        //User
        public User ReadUser(string email, string password)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                User tmpUser = new User();

                using (SqlCommand cmd = new SqlCommand("userValidation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            tmpUser.UserId = (int)dr["userId"];
                            tmpUser.Email = (string)dr["userEmail"];
                            tmpUser.Password = (string)dr["UsersPassword"];
                            tmpUser.FullName = (string)dr["fullName"];
                            tmpUser.DateOfBirth = (string)dr["dateOfBirth"];
                            tmpUser.Gender = (string)dr["gender"];
                            tmpUser.Address = (string)dr["userStreet"];
                            tmpUser.LocationX = (string)dr["location_x"];
                            tmpUser.LocationY = (string)dr["location_y"];
                            tmpUser.PersonalStatus = (int)dr["personalStatus"];
                            tmpUser.ProfilePicUrl = (string)dr["profilePicUrl"];
                            tmpUser.CoinsCredit = (int)dr["coinsCredit"];
                            tmpUser.ValidationAnswer = (string)dr["validationAnswer"];
                            tmpUser.City = (string)dr["userCity"];
                        }

                        if (result.Equals(1) && tmpUser.Password != password)
                            tmpUser.UserId = 0;

                        else if (result.Equals(0))
                            tmpUser.UserId = 0;

                        tmpUser.LikedItemsList = GetUserLikedItemsIds(tmpUser.UserId);  // get user's liked items ids list
                        return tmpUser;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get user", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public User GetUserById(int userId) 
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                User tmpUser = new User();

                using (SqlCommand cmd = new SqlCommand("GetUserById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            tmpUser.UserId = (int)dr["userId"];
                            tmpUser.Email = (string)dr["userEmail"];
                            tmpUser.Password = (string)dr["UsersPassword"];
                            tmpUser.FullName = (string)dr["fullName"];
                            tmpUser.DateOfBirth = (string)dr["dateOfBirth"];
                            tmpUser.Gender = (string)dr["gender"];
                            tmpUser.Address = (string)dr["userStreet"];
                            tmpUser.LocationX = (string)dr["location_x"];
                            tmpUser.LocationY = (string)dr["location_y"];
                            tmpUser.PersonalStatus = (int)dr["personalStatus"];
                            tmpUser.ProfilePicUrl = (string)dr["profilePicUrl"];
                            tmpUser.CoinsCredit = (int)dr["coinsCredit"];
                            tmpUser.ValidationAnswer = (string)dr["validationAnswer"];
                            tmpUser.City = (string)dr["userCity"];
                        }

                        tmpUser.LikedItemsList = GetUserLikedItemsIds(tmpUser.UserId);  // get user's liked items ids list
                        return tmpUser;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get user", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        } // using fot get user liked items
        public User CheckUserExists(string email)
        {

            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                User tmpUser = new User();

                using (SqlCommand cmd = new SqlCommand("UserExistCheck", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userEmail", email);


                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!result.Equals(0))
                        {
                            while (dr.Read())
                            {
                                tmpUser.UserId = (int)dr["userId"];
                                tmpUser.Email = (string)dr["userEmail"];
                                tmpUser.Password = (string)dr["UsersPassword"];
                                tmpUser.FullName = (string)dr["fullName"];
                                tmpUser.DateOfBirth = (string)dr["dateOfBirth"];
                                tmpUser.Gender = (string)dr["gender"];
                                tmpUser.Address = (string)dr["userStreet"];
                                tmpUser.LocationX = (string)dr["location_x"];
                                tmpUser.LocationY = (string)dr["location_y"];
                                tmpUser.PersonalStatus = (int)dr["personalStatus"];
                                tmpUser.ProfilePicUrl = (string)dr["profilePicUrl"];
                                tmpUser.CoinsCredit = (int)dr["coinsCredit"];
                                tmpUser.ValidationAnswer = (string)dr["validationAnswer"];
                                tmpUser.City = (string)dr["userCity"];
                            }
                        }
                        else
                        {
                            tmpUser.UserId = 0;
                        }
                        return tmpUser;
                    }

                }

            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get user", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public int InsertUser(User user)
        {
            SqlConnection con = null;
            int res = 0;
            try
            {
                // C - Connect
                con = Connect("Gandhi");

                // C - Create Command

                using (SqlCommand cmd = new SqlCommand("InsertUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userEmail", user.Email);
                    cmd.Parameters.AddWithValue("@UsersPassword", user.Password);
                    cmd.Parameters.AddWithValue("@fullName", user.FullName);
                    cmd.Parameters.AddWithValue("@dateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@gender", user.Gender);
                    cmd.Parameters.AddWithValue("@userStreet", user.Address);
                    cmd.Parameters.AddWithValue("@location_x", user.LocationX);
                    cmd.Parameters.AddWithValue("@location_y", user.LocationY);
                    cmd.Parameters.AddWithValue("@personalStatus", user.PersonalStatus);
                    cmd.Parameters.AddWithValue("@profilePicUrl", user.ProfilePicUrl);
                    cmd.Parameters.AddWithValue("@coinsCredit", 50);
                    cmd.Parameters.AddWithValue("@userCity", user.City);
                    cmd.Parameters.AddWithValue("@questionCode", user.ValidationQuestion);
                    cmd.Parameters.AddWithValue("@validationAnswer", user.ValidationAnswer);


                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    if (!result.Equals(0)) // USER INSERTED SUCCESSFULLY
                    {
                        res = (int)result;
                    }
                    return res;     // RES=0 - USER ALREADY EXISTS IN DB
                }
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed in Insert of user", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public int InsertWatchedItems(List<int> watchedItems, int userID)
        {
            SqlConnection con = null;
            int res = 0;
            try
            {
                // C - Connect
                con = Connect("Gandhi");
                foreach (var item in watchedItems)
                {
                    // C - Create Command
                    using (SqlCommand cmd = new SqlCommand("InsertUsersWatchedItems", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@itemId", item);
                        cmd.Parameters.AddWithValue("@userId", userID);

                        var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        // E - Execute
                        cmd.ExecuteNonQuery();
                        var result = returnParameter.Value;

                        if (result.Equals(1))
                        {
                            res += 1;
                        }
                    }
                }
                return res;
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed in Insert the item to user watch list", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public User InsertLikedItems(int likedItemId, int userId, char likeDirection)
        {
            SqlConnection con = null;
            try
            {
                // C - Connect
                con = Connect("Gandhi");
                var res = 0;

                // C - Create Command
                using (SqlCommand cmd = new SqlCommand("InsertUserLikedItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@itemId", likedItemId);
                    cmd.Parameters.AddWithValue("@likeDirection", likeDirection);


                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    res = (int)returnParameter.Value;
                }
                return GetUserById(userId);
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed in Insert user liked items to list", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public void InsertInterestedItem(int userId, int itemId)
        {
            SqlConnection con = null;
            try
            {
                // C - Connect
                con = Connect("Gandhi");

                // C - Create Command
                using (SqlCommand cmd = new SqlCommand("InsertInterestedItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed in Insert user interested item to list", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public void UpdateUnlikedItems(int unlikedItemsId, int userId)
        {
            SqlConnection con = null;
            try
            {
                // C - Connect
                con = Connect("Gandhi");

                int res = 0;
                // C - Create Command
                using (SqlCommand cmd = new SqlCommand("UpdateUserUnlikedItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@itemId", unlikedItemsId);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    if (result.Equals(1))
                    {
                        res += 1;
                    }
                    else
                        throw new Exception("Failed to Insert user " + unlikedItemsId + " item to user liked items list");
                }
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed in Insert user liked items to list", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public int ChangeUserPassword(string uEmail, string newPassword)
        {
            SqlConnection con = null;
            int res = 0;
            try
            {
                // C - Connect
                con = Connect("Gandhi");

                // C - Create Command
                using (SqlCommand cmd = new SqlCommand("ChangeUserPassword", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userEmail", uEmail);
                    cmd.Parameters.AddWithValue("@UserPassword", newPassword);

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;
                    if (result.Equals(1))
                        res = 1;
                }
                return res;
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed to change user password", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }

        }
        public List<User> GetUsers()
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<User> users = new List<User>();
                using (SqlCommand cmd = new SqlCommand("GetUsers", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            User tmpUser = new User();
                            tmpUser.UserId = (int)dr["userId"];
                            tmpUser.DateOfBirth = (string)dr["dateOfBirth"];
                            tmpUser.Gender = (string)dr["gender"];
                            tmpUser.PersonalStatus = (int)dr["personalStatus"];
                            users.Add(tmpUser);
                        }
                        return users;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get users", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }


        }
        public List<Item> GetUserBoughtItems(int userId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Item> itemList = new List<Item>();
                using (SqlCommand cmd = new SqlCommand("GetUsersAvailableItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userID", userId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ItemId = (int)dr["ItemId"];
                            var ItemDesc = (string)dr["ItemDesc"];
                            var ItemOriginUrl = (string)dr["ItemOriginUrl"];
                            var SaleReason = (string)dr["reasonDesc"];
                            var Price = (int)dr["price"];
                            var Color = (string)dr["colorDesc"];
                            var Size = (string)dr["sizeDesc"];
                            var ItemStatus = (string)dr["itemStatus"];
                            var UploadDate = (DateTime)dr["uploadDate"];
                            UploadDate.ToString();
                            var Brand = (string)dr["brandDesc"];
                            var ViewCounter = (int)dr["viewCounter"];
                            var Category = (string)dr["categoryDesc"];
                            var DepartmentID = (int)dr["departmentId"];
                            Item item = new Item(ItemId, ItemDesc, ItemOriginUrl, SaleReason, Price, Color, Size, ItemStatus, Category, UploadDate, Brand, ViewCounter, DepartmentID);
                            item.DBtags = ReadItemTags(ItemId);
                            item.ItemPhotos = ReadItemPhotos(ItemId);
                            itemList.Add(item);
                        }
                        return itemList;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get items", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }    // Personal Area
        public List<Item> GetUserSellItems(int userId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Item> itemList = new List<Item>();
                using (SqlCommand cmd = new SqlCommand("GetUserSaleItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userID", userId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ItemId = (int)dr["ItemId"];
                            var ItemDesc = (string)dr["ItemDesc"];
                            var ItemOriginUrl = (string)dr["ItemOriginUrl"];
                            var SaleReason = (string)dr["reasonDesc"];
                            var Price = (int)dr["price"];
                            var Color = (string)dr["colorDesc"];
                            var Size = (string)dr["sizeDesc"];
                            var ItemStatus = (string)dr["itemStatus"];
                            var UploadDate = (DateTime)dr["uploadDate"];
                            UploadDate.ToString();
                            var Brand = (string)dr["brandDesc"];
                            var ViewCounter = (int)dr["viewCounter"];
                            var Category = (string)dr["categoryDesc"];
                            var DepartmentID = (int)dr["departmentId"];
                            Item item = new Item(ItemId, ItemDesc, ItemOriginUrl, SaleReason, Price, Color, Size, ItemStatus, Category, UploadDate, Brand, ViewCounter, DepartmentID);
                            item.SellerId = userId;
                            item.DBtags = ReadItemTags(ItemId);
                            item.ItemPhotos = ReadItemPhotos(ItemId);
                            itemList.Add(item);
                        }
                        return itemList;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get items", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }     // Personal Area
        public List<Item> GetUserLikedItems(int userId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Item> itemList = new List<Item>();
                using (SqlCommand cmd = new SqlCommand("GetUserLikedItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ItemId = (int)dr["ItemId"];
                            var ItemDesc = (string)dr["ItemDesc"];
                            var ItemOriginUrl = (string)dr["ItemOriginUrl"];
                            var SaleReason = (string)dr["reasonDesc"];
                            var Price = (int)dr["price"];
                            var Color = (string)dr["colorDesc"];
                            var Size = (string)dr["sizeDesc"];
                            var ItemStatus = (string)dr["itemStatus"];
                            var UploadDate = (DateTime)dr["uploadDate"];
                            UploadDate.ToString();
                            var Brand = (string)dr["brandDesc"];
                            var ViewCounter = (int)dr["viewCounter"];
                            var Category = (string)dr["categoryDesc"];
                            var DepartmentID = (int)dr["departmentId"];
                            Item item = new Item(ItemId, ItemDesc, ItemOriginUrl, SaleReason, Price, Color, Size, ItemStatus, Category, UploadDate, Brand, ViewCounter, DepartmentID);
                            item.DBtags = ReadItemTags(ItemId);
                            item.ItemPhotos = ReadItemPhotos(ItemId);
                            itemList.Add(item);
                        }
                        return itemList;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get user liked items list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }     // Personal Area
        public List<Item> GetUserInterstedItems(int userId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Item> itemList = new List<Item>();
                using (SqlCommand cmd = new SqlCommand("GetUserInterestedItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ItemId = (int)dr["ItemId"];
                            var ItemDesc = (string)dr["ItemDesc"];
                            var ItemOriginUrl = (string)dr["ItemOriginUrl"];
                            var SaleReason = (string)dr["reasonDesc"];
                            var Price = (int)dr["price"];
                            var Color = (string)dr["colorDesc"];
                            var Size = (string)dr["sizeDesc"];
                            var ItemStatus = (string)dr["itemStatus"];
                            var UploadDate = (DateTime)dr["uploadDate"];
                            UploadDate.ToString();
                            var Brand = (string)dr["brandDesc"];
                            var ViewCounter = (int)dr["viewCounter"];
                            var Category = (string)dr["categoryDesc"];
                            var DepartmentID = (int)dr["departmentId"];
                            Item item = new Item(ItemId, ItemDesc, ItemOriginUrl, SaleReason, Price, Color, Size, ItemStatus, Category, UploadDate, Brand, ViewCounter, DepartmentID);
                            item.DBtags = ReadItemTags(ItemId);
                            item.ItemPhotos = ReadItemPhotos(ItemId);
                            itemList.Add(item);
                        }
                        return itemList;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get user interested items list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }     // Personal Area
        public List<int> GetUserLikedItemsIds(int userId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<int> userLikedItemsList = new List<int>();
                using (SqlCommand cmd = new SqlCommand("GetUserLikedItemIdsList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int tmpItemId = 0;
                            tmpItemId = (int)dr["itemId"];
                            userLikedItemsList.Add(tmpItemId);
                        }
                        return userLikedItemsList;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get user's liked items list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }     // Local storage ---> get user's liked items when sign-in
        public int UpdatePoint(int userId, int addedPoint)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                int currentScore = 0;

                using (SqlCommand cmd = new SqlCommand("UpdateCoins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@coins", addedPoint);

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            currentScore = (int)dr["coinsCredit"];
                        }
                        return currentScore;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to update users coins", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public int UpdateProfilePice(int userId, List<string> pics)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                int res = 0;
                foreach (var pic in pics)
                {


                    using (SqlCommand cmd = new SqlCommand("UpdateProfilePic", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@pic", pic);

                        var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        cmd.ExecuteNonQuery();

                        var result = returnParameter.Value;

                        if (result.Equals(1))
                            res += 1;

                    }
                }
                if (res == pics.Count)
                    return res;
                else return 0;
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to update users profile Pic", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public List<Item> GetUserSoldItems(int userId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Item> itemList = new List<Item>();
                using (SqlCommand cmd = new SqlCommand("GetUserSoldItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userID", userId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ItemId = (int)dr["ItemId"];
                            var ItemDesc = (string)dr["ItemDesc"];
                            var ItemOriginUrl = (string)dr["ItemOriginUrl"];
                            var SaleReason = (string)dr["reasonDesc"];
                            var Price = (int)dr["price"];
                            var Color = (string)dr["colorDesc"];
                            var Size = (string)dr["sizeDesc"];
                            var ItemStatus = (string)dr["itemStatus"];
                            var UploadDate = (DateTime)dr["uploadDate"];
                            UploadDate.ToString();
                            var Brand = (string)dr["brandDesc"];
                            var ViewCounter = (int)dr["viewCounter"];
                            var Category = (string)dr["categoryDesc"];
                            var DepartmentID = (int)dr["departmentId"];
                            Item item = new Item(ItemId, ItemDesc, ItemOriginUrl, SaleReason, Price, Color, Size, ItemStatus, Category, UploadDate, Brand, ViewCounter, DepartmentID);
                            item.SellerId = userId;
                            item.DBtags = ReadItemTags(ItemId);
                            item.ItemPhotos = ReadItemPhotos(ItemId);
                            itemList.Add(item);
                        }
                        return itemList;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get items", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }


        //Item
        public int InsertItem(Item item, int userId)
        {
            SqlConnection con = null;
            int res = 0;
            try
            {
                // C - Connect
                con = Connect("Gandhi");

                // C - Create Command
                using (SqlCommand cmd = new SqlCommand("InsertItem", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemDesc", item.ItemDesc);
                    cmd.Parameters.AddWithValue("@ItemOriginUrl", item.ItemOriginUrl);
                    cmd.Parameters.AddWithValue("@saleReason", item.SaleReason);
                    cmd.Parameters.AddWithValue("@price", item.Price);
                    cmd.Parameters.AddWithValue("@colorId", item.ColorId);
                    cmd.Parameters.AddWithValue("@sizeId", item.SizeId);
                    cmd.Parameters.AddWithValue("@itemStatus", item.ItemStatus);
                    cmd.Parameters.AddWithValue("@uploadDate", item.UploadDate);
                    cmd.Parameters.AddWithValue("@brandId", item.BrandId);
                    //cmd.Parameters.AddWithValue("@categoryId",1); //change to category Id after fixing front
                    cmd.Parameters.AddWithValue("@categoryId", item.CategoryId);
                    cmd.Parameters.AddWithValue("@viewCounter", item.ViewCounter);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;
                    var itemId = (int)result;

                    //var insertResult = InsertUserItem(itemId, userId);  // INSERT ITEM TO USER
                    InsertItemTags(item.TagList, itemId);                   // INSERT ITEM TAGS
                                                                            //InsertItemImages(item.ItemPhotos, itemId);           

                    if (itemId != 0)
                    {
                        res = itemId;
                    }
                    else
                    {
                        throw new Exception("item already exists in user's items");
                    }
                    return res;
                }
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed in Insert of item", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public void InsertItemTags(List<string> tagList, int itemId)
        {
            SqlConnection con = null;
            try
            {
                // C - Connect
                con = Connect("Gandhi");

                // C - Create Command
                for (int i = 0; i < tagList.Count; i++)
                {
                    using (SqlCommand cmd = new SqlCommand("InsertTag", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TagDesc", tagList[i]);
                        cmd.Parameters.AddWithValue("@itemId", itemId);

                        var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        // E - Execute
                        cmd.ExecuteNonQuery();
                        var result = (int)returnParameter.Value;
                        if (result != 2)
                        {
                            throw new Exception("something went wrong inserting items");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed Insert tags", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public int InsertItemImages(List<string> imgList, int itemId)
        {
            SqlConnection con = null;
            try
            {
                // C - Connect
                con = Connect("Gandhi");
                var result = 0;

                // C - Create Command
                foreach (var img in imgList)
                {
                    using (SqlCommand cmd = new SqlCommand("InsertItemImg", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@itemID", itemId);
                        cmd.Parameters.AddWithValue("@imgURL", img);

                        var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        // E - Execute
                        cmd.ExecuteNonQuery();
                        result += (int)returnParameter.Value;
                    }
                }

                if (result.Equals(imgList.Count))
                {
                    return 1;
                }
                else
                {
                    return 0;    // exception of trying to save same image path to DB 
                    //throw new Exception("not all images were inserted");
                }


            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed in Insert images", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public List<Item> ReadItems()
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Item> items = new List<Item>();

                using (SqlCommand cmd = new SqlCommand("ReadItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ItemId = (int)dr["ItemId"];
                            var ItemDesc = (string)dr["ItemDesc"];
                            var ItemOriginUrl = (string)dr["ItemOriginUrl"];
                            var SaleReason = (string)dr["reasonDesc"];
                            var Price = (int)dr["price"];
                            var Color = (string)dr["colorDesc"];
                            var Size = (string)dr["sizeDesc"];
                            var ItemStatus = (string)dr["itemStatus"];
                            var UploadDate = (DateTime)dr["uploadDate"];
                            UploadDate.ToString();
                            var Brand = (string)dr["brandDesc"];
                            var ViewCounter = (int)dr["viewCounter"];
                            var Category = (string)dr["categoryDesc"];
                            var DepartmentID = (int)dr["departmentId"];

                            Item item = new Item(ItemId, ItemDesc, ItemOriginUrl, SaleReason, Price, Color, Size, ItemStatus, Category, UploadDate, Brand, ViewCounter, DepartmentID);
                            Item tmpItem = GetSellerId(item);
                            tmpItem.DBtags = ReadItemTags(ItemId);
                            tmpItem.ItemPhotos = ReadItemPhotos(ItemId);
                            items.Add(tmpItem);
                        }
                        return items;
                    }

                }

            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get items: ", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public Item ReadItem(int itemId)
        {

            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                Item item = new Item();

                using (SqlCommand cmd = new SqlCommand("ReadItem", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            item.ItemId = (int)dr["ItemId"];
                            item.ItemDesc = (string)dr["ItemDesc"];
                            item.ItemOriginUrl = (string)dr["ItemOriginUrl"];
                            item.SaleReason = (string)dr["reasonDesc"];
                            item.Price = (int)dr["price"];
                            item.Color = (string)dr["colorDesc"]; // mybe we need DESC and inner JOIN 
                            item.Size = (string)dr["sizeDesc"];
                            item.ItemStatus = (string)dr["itemStatus"];
                            item.UploadDate = (DateTime)dr["uploadDate"];
                            item.UploadDate.ToString();
                            item.Brand = (string)dr["brandDesc"];
                            item.ViewCounter = (int)dr["viewCounter"];
                            item.Category = (string)dr["categoryDesc"];
                            item.DepartmentID = (int)dr["departmentId"];
                            item.ItemPhotos = ReadItemPhotos(item.ItemId);
                            item.DBtags = ReadItemTags(item.ItemId);
                            
                        }
                        return item;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get items: ", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public List<string> ReadItemPhotos(int itemID)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<string> itemPhotos = new List<string>();

                using (SqlCommand cmd = new SqlCommand("ReadItemImages", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@itemId", itemID);
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string tmpPhoto = (string)dr["imageURL"];
                            itemPhotos.Add(tmpPhoto);
                        }
                        return itemPhotos;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get items: ", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public List<Tag> ReadItemTags(int itemID)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Tag> itemTags = new List<Tag>();

                using (SqlCommand cmd = new SqlCommand("ReadItemTags", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@itemId", itemID);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Tag tag = new Tag
                            {
                                TagId = (int)dr["tagId"],
                                TagDesc = (string)dr["tagDesc"]
                            };
                            itemTags.Add(tag);
                        }
                        return itemTags;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get items: ", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public void BuyItemByMoney(int buyerId, int sellerId, int itemId)
        {
            SqlConnection con = null;
            try
            {
                // C - Connect
                con = Connect("Gandhi");

                // C - Create Command
                using (SqlCommand cmd = new SqlCommand("BuyItemByMoney", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    cmd.Parameters.AddWithValue("@sellerId", sellerId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);

                    // E - Execute
                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed in making the transaction", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public int BuyItemByCoinsCredit(int buyerId, int sellerId, int itemId, int price)
        {
            SqlConnection con = null;
            int difference = 0;
            try
            {
                // C - Connect
                con = Connect("Gandhi");

                // C - Create Command
                using (SqlCommand cmd = new SqlCommand("BuyItemByCredit", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    cmd.Parameters.AddWithValue("@sellerId", sellerId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@price", price);

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;
                    if (result.Equals(0))
                        return 0;
                    else
                    {
                        difference = (int)result;
                        return difference;
                    }
                }
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed ", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public int SellExistingItem(int itemId, int userId)
        {
            SqlConnection con = null;
            try
            {
                // C - Connect
                con = Connect("Gandhi");

                // C - Create Command
                using (SqlCommand cmd = new SqlCommand("SellExistingItem", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;
                    int res = 0;
                    if (result.Equals(1))
                        res = 1;
                    return res;
                }
            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed to sell existing item ", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public Item GetSellerId(Item item)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");

                using (SqlCommand cmd = new SqlCommand("GetSellerId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@itemId", item.ItemId);

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            item.SellerId = (int)dr["userId"];
                            item.LocationX = (string)dr["location_x"];
                            item.LocationY = (string)dr["location_y"];
                        }
                    }
                    return item;
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get user seller id", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public int MakeTransaction(int itemId, int sellerId, int buyerId, char side)
        {
            SqlConnection con = null;
            int res=0;

            try
            {
                // C - Connect
                con = Connect("Gandhi");

                // C - Create Command
                using (SqlCommand cmd = new SqlCommand("MakeTransaction", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@sellerId", sellerId);
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    cmd.Parameters.AddWithValue("@side", side);

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    if (result.Equals(1))
                    {
                        switch (side)
                        {
                            case 'S':
                                return res = 1;         // Seller is first

                            case 'B':
                                return res = 2;         // Buyer is first
                        }
                    }

                    else if (result.Equals(2))       // Transaction Complete baruch ha'shem
                    {
                        switch (side)
                        {
                            case 'S':
                                return res = 3;         // Selling state

                            case 'B':
                                return res = 4;         // Buying state
                        }
                    }    
                 
                    else
                        return res;
                    
                }

                return res;

            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed to make the transaction", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public List<User> GetInterestedUsersList(int itemId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");

                List<User> userList = new List<User>();
                using (SqlCommand cmd = new SqlCommand("ReadInterestedUsersList", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            User tmpUser = new User();
                            tmpUser.UserId = (int)dr["userId"];
                            tmpUser.Email = (string)dr["userEmail"];
                            //tmpUser.Password = (string)dr["UsersPassword"];
                            tmpUser.FullName = (string)dr["fullName"];
                            tmpUser.DateOfBirth = (string)dr["dateOfBirth"];
                            tmpUser.Gender = (string)dr["gender"];
                            tmpUser.Address = (string)dr["userStreet"];
                            tmpUser.LocationX = (string)dr["location_x"];
                            tmpUser.LocationY = (string)dr["location_y"];
                            tmpUser.PersonalStatus = (int)dr["personalStatus"];
                            tmpUser.ProfilePicUrl = (string)dr["profilePicUrl"];
                            //tmpUser.CoinsCredit = (int)dr["coinsCredit"];
                            //tmpUser.ValidationAnswer = (string)dr["validationAnswer"];
                            tmpUser.City = (string)dr["userCity"];
                            userList.Add(tmpUser);
                        }

                    }
                }
                return userList;
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get items", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }



        //QandA
        public QandA ReadQandA(string Uemail)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                QandA tmpQandA = new QandA();

                using (SqlCommand cmd = new SqlCommand("getQandA", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Uemail", Uemail);
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            tmpQandA.Question = (string)dr["question"];
                            tmpQandA.Answer = (string)dr["validationAnswer"];
                        }
                        return tmpQandA;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get users question and answer", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public List<QandA> GetAllQ()
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<QandA> tmpQandA = new List<QandA>();

                using (SqlCommand cmd = new SqlCommand("GetAllQues", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var Question = (string)dr["question"];
                            var QuestionId = (int)dr["questionCode"];
                            QandA q = new QandA(Question, QuestionId);
                            tmpQandA.Add(q);
                        }
                        return tmpQandA;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get questions", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }


        //Colors
        public List<Color> ReadColors()
        {

            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Color> colors = new List<Color>();

                using (SqlCommand cmd = new SqlCommand("getColors", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            Color color = new Color
                            {
                                ColorId = (int)dr["colorId"],
                                ColorDesc = (string)dr["colorDesc"]
                            };
                            colors.Add(color);

                        }
                        return colors;
                    }

                }

            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get colors list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }


        //SaleReasons
        public List<SaleReason> ReadReasons()
        {

            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<SaleReason> reasons = new List<SaleReason>();

                using (SqlCommand cmd = new SqlCommand("getReasons", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            SaleReason reason = new SaleReason
                            {
                                ReasonId = (int)dr["reasonId"],
                                ReasonDesc = (string)dr["reasonDesc"]
                            };
                            reasons.Add(reason);

                        }
                        return reasons;
                    }

                }

            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get reasons list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }


        //Brands
        public List<Brand> ReadBrands()
        {

            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Brand> brands = new List<Brand>();

                using (SqlCommand cmd = new SqlCommand("getBrands", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            Brand brand = new Brand
                            {
                                BrandId = (int)dr["brandId"],
                                BrandDesc = (string)dr["brandDesc"],
                                BrandUrl = (string)dr["brandIconUrl"]
                            };
                            brands.Add(brand);

                        }
                        return brands;
                    }

                }

            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get brands list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }


        //Tags
        public int InsertNewTag(Tag newTag)
        {
            SqlConnection con = null;
            int res = 0;
            try
            {
                // C - Connect
                con = Connect("Gandhi");

                // C - Create Command
                using (SqlCommand cmd = new SqlCommand("InsertTag", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TagDesc", newTag.TagDesc);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    if (result.Equals(1))
                    {
                        res = 1;        // TAG INSERTED
                    }
                    else
                    {
                        throw new Exception("tag desc already exists in tags");
                    }
                    return res;   // RES=0 ----> TAG DESC EXISTS
                }

            }

            catch (Exception ex)
            {
                // write to log file
                throw new Exception("Failed in Insert tag", ex);
            }

            finally
            {
                // C - Close Connection
                con.Close();
            }
        }
        public List<Tag> ReadTags()
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Tag> tags = new List<Tag>();
                using (SqlCommand cmd = new SqlCommand("getTags", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Tag tag = new Tag
                            {
                                TagId = (int)dr["TagId"],
                                TagDesc = (string)dr["TagDesc"]
                            };
                            tags.Add(tag);
                        }
                        return tags;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get Tags list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public List<Tag> GetUserTags(List<int> users)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Tag> tags = new List<Tag>();
                foreach (var user in users)
                {
                    using (SqlCommand cmd = new SqlCommand("GetUsersTag", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userId", user);
                        var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        cmd.ExecuteNonQuery();
                        var result = returnParameter.Value;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (result.Equals(0))
                                result = 2;
                            else
                            {
                                while (dr.Read())
                                {
                                    Tag tmpTag = new Tag();
                                    tmpTag.TagId = (int)dr["TagId"];
                                    tmpTag.TagDesc = (string)dr["TagDesc"];
                                    tags.Add(tmpTag);
                                }
                            }
                        }
                    }
                }
                return tags;
            }
            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get Tags list", ex);
            }
            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public List<Tag> FindUserWatchListTags(int userId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Tag> tags = new List<Tag>();
                using (SqlCommand cmd = new SqlCommand("GetUsersWatchListItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (result.Equals(0))
                            result = 2;
                        else
                        {
                            while (dr.Read())
                            {
                                Tag tag = new Tag
                                {
                                    TagId = (int)dr["TagId"],
                                    TagDesc = (string)dr["TagDesc"]
                                };
                                tags.Add(tag);
                            }
                        }
                    }
                    return tags;
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get Tags list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public List<Tag> FindUserLikedItemsListTags(int userId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Tag> tags = new List<Tag>();
                using (SqlCommand cmd = new SqlCommand("GetUserLikedItemsTagsList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (result.Equals(0))
                            result = 2;
                        else
                        {
                            while (dr.Read())
                            {
                                Tag tag = new Tag
                                {
                                    TagId = (int)dr["TagId"],
                                    TagDesc = (string)dr["TagDesc"]
                                };
                                tags.Add(tag);
                            }
                        }
                    }
                    return tags;
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get Tags list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }


        //Sizes
        public List<Size> ReadSizes()
        {

            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Size> sizes = new List<Size>();

                using (SqlCommand cmd = new SqlCommand("getSizes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            Size size = new Size
                            {
                                SizeId = (int)dr["sizeId"],
                                SizeDesc = (string)dr["sizeDesc"]
                            };
                            sizes.Add(size);

                        }
                        return sizes;
                    }

                }

            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get brands list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }


        // Categories
        public List<Category> ReadCategories()
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Category> categories = new List<Category>();

                using (SqlCommand cmd = new SqlCommand("GetCategories", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Category tmpCategory = new Category
                            {
                                CategoryId = (int)dr["categoryId"],
                                CategoryDesc = (string)dr["categoryDesc"],
                                DepartmentId = (int)dr["departmentId"],
                                CategoryPicUrl = (string)dr["categoryPicUrl"]
                            };

                            categories.Add(tmpCategory);
                        }

                        return categories;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get Categories list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public List<Category> ReadCategoriesByDepartment(int departmentId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Category> categories = new List<Category>();

                using (SqlCommand cmd = new SqlCommand("GetCategoriesByDepartment", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@departmentId", departmentId);
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Category tmpCategory = new Category
                            {
                                CategoryId = (int)dr["categoryId"],
                                CategoryDesc = (string)dr["categoryDesc"],
                                DepartmentId = (int)dr["departmentId"],
                                CategoryPicUrl = (string)dr["categoryPicUrl"],
                                NumOfItemsInCategory = 0
                            };
                            categories.Add(tmpCategory);
                        }
                        return categories;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get Categories list by departments", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }


        // Departments 
        public List<Department> ReadDepartments()
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Department> departments = new List<Department>();

                using (SqlCommand cmd = new SqlCommand("GetDepartments", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            Department tmpDepartment = new Department
                            {
                                DepartmentId = (int)dr["departmentId"],
                                DepartmentDesc = (string)dr["departmentDesc"]
                            };

                            departments.Add(tmpDepartment);
                        }
                        return departments;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get departments list", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }
        public List<Item> getItemsByDepartment(int departmentId)
        {
            SqlConnection con = null;
            try
            {
                // Connect
                con = Connect("Gandhi");
                List<Item> itemListByDep = new List<Item>();

                using (SqlCommand cmd = new SqlCommand("GetItemsByDepartment", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@departmentId", departmentId);
                    var returnParameter = cmd.Parameters.Add("@results", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // E - Execute
                    cmd.ExecuteNonQuery();
                    var result = returnParameter.Value;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ItemId = (int)dr["ItemId"];
                            var ItemDesc = (string)dr["ItemDesc"];
                            var ItemOriginUrl = (string)dr["ItemOriginUrl"];
                            var SaleReason = (string)dr["reasonDesc"];
                            var Price = (int)dr["price"];
                            var Color = (string)dr["colorDesc"];
                            var Size = (string)dr["sizeDesc"];
                            var ItemStatus = (string)dr["itemStatus"];
                            var UploadDate = (DateTime)dr["uploadDate"];
                            UploadDate.ToString();
                            var Brand = (string)dr["brandDesc"];
                            var ViewCounter = (int)dr["viewCounter"];
                            var CategoryDesc = (string)dr["categoryDesc"];
                            var CategoryId = (int)dr["categoryId"];
                            var DepartmentID = departmentId;
                            Item item = new Item(ItemId, ItemDesc, ItemOriginUrl, SaleReason, Price, Color, Size, ItemStatus, CategoryDesc, CategoryId, UploadDate, Brand, ViewCounter, DepartmentID);
                            Item tmpItem = GetSellerId(item);
                            tmpItem.DBtags = ReadItemTags(ItemId);
                            tmpItem.ItemPhotos = ReadItemPhotos(ItemId);
                            itemListByDep.Add(tmpItem);
                        }
                        return itemListByDep;
                    }
                }
            }

            catch (Exception ex)
            {
                // write the error to log
                throw new Exception("failed to get items list by department", ex);
            }

            finally
            {
                // Close the connection
                if (con != null)
                    con.Close();
            }
        }


        // SQL CONNECTION
        SqlConnection Connect(string connectionStringName)
        {

            string connectionString = WebConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);

            con.Open();

            return con;
        }

    }

}