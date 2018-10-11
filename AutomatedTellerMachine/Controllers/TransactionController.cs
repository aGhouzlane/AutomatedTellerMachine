using AutomatedTellerMachine.Models;
using AutomatedTellerMachine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomatedTellerMachine.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transaction/Deposit
        public ActionResult Deposit(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Deposit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult QuickCash(int checkingAccountId, decimal amount)
        {
            var sourceCheckingAccount = db.CheckingAccounts.Find(checkingAccountId);
            var balance = sourceCheckingAccount.Balance;
            if (balance < amount)
            {
                return View("QuickCashInsufficientFunds");
            }
            db.Transactions.Add(new Transaction { CheckingAccountId = checkingAccountId, Amount = -amount });
            db.SaveChanges();

            var service = new CheckingAccountService(db);
            service.UpdateBalance(checkingAccountId);

            return RedirectToAction("Index", "Home");
        }

    }
}