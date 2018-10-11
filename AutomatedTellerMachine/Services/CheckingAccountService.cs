using AutomatedTellerMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomatedTellerMachine.Services
{
    public class CheckingAccountService
    {
        private ApplicationDbContext db;

        public CheckingAccountService(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public void CreateCheckingAccount(string firstName, string lastName, string userId, decimal initialBalance)
        {
            var accountNumber = (1234567 + db.CheckingAccounts.Count()).ToString().PadLeft(10, '0');
            var checkingAccount = new CheckingAccount
            {
                AccountNumber = accountNumber,
                FirstName = firstName,
                LastName = lastName,
                Balance = 5000,
                ApplicationUserId = userId
            };
            db.CheckingAccounts.Add(checkingAccount);
            db.SaveChanges();
        }

        public void UpdateBalance(int checkingAccountId)
        {
            var checkingAccount = db.CheckingAccounts.Where(c => c.Id == checkingAccountId).First();
            checkingAccount.Balance = db.Transactions.Where(c => c.CheckingAccountId == checkingAccountId).Sum(t => t.Amount);
            db.SaveChanges();
        }
    }
}