using Flockbuster.Services.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Flockbuster.Services
{
    public class AdminServices
    {

        public List<User> UserList { get; set; } = new();

        public List<RentalObject> ListOfRentalObjects { get; set; } = new();

        public List<NeverForget> HistoryLog { get; set; } = new();

        private const int AllowedRentalDays = 14;

        string firmanavn;

        public AdminServices(string firmanavn)
        {
            UserList.Add(new Admin(1, "Aron", "Palsson", "aropx0001", "1234"));
            UserList.Add(new Admin(2, "Emil", "Palsson", "emil0001", "1234"));
            UserList.Add(new Admin(3, "Thomas", "Palsson", "thomas0001", "1234"));
            UserList.Add(new Admin(4, "Brian", "Palsson", "brian0001", "1234"));
            UserList.Add(new Standard(101, "Mohammed", "Mosleh", "mohammed", "1234"));
            UserList.Add(new Standard(102, "Mhillip", "Mosleh", "phillip", "1234"));
            UserList.Add(new Standard(103, "Max", "Mosleh", "max", "1234"));

            this.firmanavn = firmanavn;
            LoadRO();
        }

        public int CreateUser(string firstname, string lastname, UserType accountType, string username, string password)
        {
            int userID = UserList.Max(x => x.accountID);

            switch (accountType)
            {
                case UserType.Standard:
                    UserList.Add(new Standard(++userID, firstname, lastname, username, password));
                    break;

                case UserType.Admin:
                    UserList.Add(new Admin(++userID, firstname, lastname, username, password));
                    break;
            }

            return userID;
        }

        public void UpdateUserName(User updatedUser)
        {
            User user = UserList.FirstOrDefault(x => x.accountID == updatedUser.accountID);

            if (user != null)
            {
                user.firstname = updatedUser.firstname;
                user.lastname = updatedUser.lastname;
            }
        }

        public int AddRO(RentalObject newRO)
        {
            int itemID = 0;
            if (ListOfRentalObjects.Count is 0)
            {
                itemID = 1;
            }
            else
            {
                itemID = ListOfRentalObjects.Max(x => x.ItemID) + 1;
            }
            newRO.ItemID = itemID;
            ListOfRentalObjects.Add(newRO);

            return newRO.ItemID;
        }

        public double? AddBalance(int id, double? balance)
        {
            User user = UserList.FirstOrDefault(x => x.accountID == id);
            if (user == null || balance == null || balance < 0)
            {
                return user?.balance;
            }

            user.balance += balance;
            return user.balance;
        }

        public double? AddBalanceV2(int id, double? balance)
        {
            if (balance < 0 || balance > 10000)
            {
                return 0;
            }
            double? newBalance = UserList.Find(x => x.accountID == id).balance += balance;
            return newBalance;
        }

        public User IdentifyUserByID(int userID)
        {
            return UserList.FirstOrDefault(x => x.accountID == userID);
        }

        public RentalObject FindROWithID(int itemID)
        {
            RentalObject rentalObject;
            rentalObject = ListOfRentalObjects.FirstOrDefault(x => x.ItemID == itemID);
            return rentalObject;
        }

        public void UpdateRO(RentalObject newRO)
        {
            RentalObject? rentalObject = FindROWithID(newRO.ItemID);
            if (rentalObject is not null)
            {
                rentalObject.Titel = newRO.Titel;
                rentalObject.Category = newRO.Category;
                rentalObject.ReleaseYear = newRO.ReleaseYear;
                rentalObject.Instructor = newRO.Instructor;
                rentalObject.Price = newRO.Price;
                rentalObject.InStock = newRO.InStock;
            }
        }

        public void DeleteRO(int itemID)
        {
            RentalObject foundRentalObject = FindROWithID(itemID);
            ListOfRentalObjects.Remove(foundRentalObject);
        }

        DateOnly? resetDate = null;

        public void RentObject(int foundRO, int userID)
        {
            RentalObject? rentalObject = FindROWithID(foundRO);

            User? user = IdentifyUserByID(userID);

            if (IsUserOrRONull(foundRO, userID) || user.balance < rentalObject.Price || !rentalObject.InStock)
            {
                return;
            }

            rentalObject.InStock = false;
            user.balance -= rentalObject.Price;
            rentalObject.RentDate = DateOnly.FromDateTime(DateTime.Now);
            rentalObject.ReturnDate = DateOnly.FromDateTime(DateTime.Now).AddDays(14);
            if (user.MyLoans is null) user.MyLoans = new List<RentalObject>();
            rentalObject.LoaningNow = user.accountID;
            user.MyLoans?.Add(rentalObject);
        }

        public int CalculateRentalDays(int rentalObjectID, int userID)
        {
            if (IsUserOrRONull(rentalObjectID, userID))
            {
                return 0;
            }

            RentalObject rentalObject = FindROWithID(rentalObjectID);

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);


            return currentDate.DayNumber - rentalObject.RentDate.Value.DayNumber;
        }

        public double CalculateLateReturnFee(int rentalObjectID, int userID)
        {
            RentalObject rentalObject = FindROWithID(rentalObjectID);

            User? user = IdentifyUserByID(userID);

            int totalDaysRented = CalculateRentalDays(rentalObject.ItemID, user.accountID);
            int howManyDaysLate = totalDaysRented - AllowedRentalDays;

            if (howManyDaysLate > 0)
            {
                return howManyDaysLate * 5;
            }
            return 0.0;
        }

        public void ReturnObject(int rentalObjectID, int userID)
        {
            if (IsUserOrRONull(rentalObjectID, userID))
            {
                return;
            }

            RentalObject returnObject = FindROWithID(rentalObjectID);
            User? user = IdentifyUserByID(userID);

            if (user.MyLoans.Contains(returnObject))
            {
                int daysLate = CalculateRentalDays(rentalObjectID, userID) - AllowedRentalDays;
                if (daysLate > 0)
                {
                    user.balance -= daysLate * 5;
                }
            }

            AddToHistoryLog(returnObject, userID);

            returnObject.InStock = true;
            returnObject.LoaningNow = null;
            returnObject.RentDate = resetDate;
            returnObject.ReturnDate = resetDate;
            user.MyLoans.Remove(returnObject);
        }

        public bool IsUserOrRONull(int rentalObjectID, int userID)
        {
            RentalObject? rentalObject = FindROWithID(rentalObjectID);
            User? user = IdentifyUserByID(userID);

            // True hvis en af dem er null. Ellers false
            return rentalObject == null || user == null;
        }

        public User HtmlLogin(string username, string password)
        {
            return UserList.FirstOrDefault(x => x.username == username && x.password == password);
        }

        public void LoadRO()
        {
            ListOfRentalObjects.Add(new RentalObject { ItemID = 1, Titel = "Dora the explorer", Category = new List<Category> { Category.Adventure, Category.Fantasy }, ReleaseYear = 2019, Instructor = "Jan Pytlik", Price = 49, InStock = true });
            ListOfRentalObjects.Add(new RentalObject { ItemID = 2, Titel = "The Ring", Category = new List<Category> { Category.Horror }, ReleaseYear = 2003, Instructor = "Albert Einstein", Price = 49, InStock = true, });
            ListOfRentalObjects.Add(new RentalObject { ItemID = 3, Titel = "Kung fu panda", Category = new List<Category> { Category.Animation, Category.Adventure, Category.Comedy }, ReleaseYear = 2008, Instructor = "Rune Klan", Price = 69, InStock = true, });
            ListOfRentalObjects.Add(new RentalObject { ItemID = 4, Titel = "Scarface", Category = new List<Category> { Category.Action, Category.Thriller }, ReleaseYear = 1983, Instructor = "Donald Trump", Price = 59, InStock = true });
            ListOfRentalObjects.Add(new RentalObject { ItemID = 5, Titel = "Wish dragon", Category = new List<Category> { Category.Animation, Category.Adventure }, ReleaseYear = 2021, Instructor = "Joe Biden", Price = 69, InStock = true, });
        }

        public void AddToHistoryLog(RentalObject foundObject, int userID)
        {
            NeverForget ThisGoesToHistoryList = new();

            ThisGoesToHistoryList.UserID = userID;
            ThisGoesToHistoryList.ItemID = foundObject.ItemID;
            ThisGoesToHistoryList.Title = foundObject.Titel;
            ThisGoesToHistoryList.Price = foundObject.Price;
            ThisGoesToHistoryList.RentDate = foundObject.RentDate;
            ThisGoesToHistoryList.ReturnDate = foundObject.ReturnDate;

            HistoryLog.Add(ThisGoesToHistoryList);
        }

        public List<RentalObject> GetAllFalses()
        {
            return ListOfRentalObjects.Where(x => x.InStock is false).ToList();

            //quarry
            //var q = from RO in ListOfRentalObjects where RO.InStock is false select RO;
            //return q.ToList();
        }
        public List<NeverForget> GetHistoryFromUser(int userid)
        {
            return HistoryLog.Where(x => x.UserID == userid).ToList();
        }
    }
}

