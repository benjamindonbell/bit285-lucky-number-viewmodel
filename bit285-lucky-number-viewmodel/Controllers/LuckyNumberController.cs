﻿using bit285_lucky_number_viewmodel.Models;
using bit285_lucky_number_viewmodel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bit285_lucky_number_database.Controllers
{
    public class LuckyNumberController : Controller
    {
        LuckyNumberDbContext dbc = new LuckyNumberDbContext();

        // GET: LuckyNumber
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Player p)
        {
            if (ModelState.IsValid) //always test the model state before writing to database
            {
                //TODO: update database with Player info

                dbc.Players.Add(p);
                dbc.SaveChanges();
                Session["currentID"] = p.PlayerId;


                //TODO: Save playerid to session variable

                Session["currentID"] = p.PlayerId;

                //TODO: Redirect to the Spin Action
                return RedirectToAction("Spin");

            }
            return View();
        }

        public ActionResult Spin()
        {
            //Get the current player from the Database
            int id = Convert.ToInt32(Session["currentID"]);
            Player currentPlayer = dbc.Players.Single(p => p.PlayerId == id);

            //TODO: Create a new Spin ViewModel instance to send to the View 
            // (Fill in its data with info from the Player)

            //Player mySpin = new Player();
            SpinViewModel mySpin = new SpinViewModel();
            mySpin.Number = currentPlayer.Number;
            mySpin.Balance = currentPlayer.Balance;
            return View(mySpin);
        }

        [HttpPost]
        public ActionResult Spin(SpinViewModel mySpin)
        {
            //Get the current player from the Database
            int id = Convert.ToInt32(Session["currentId"]);
            Player currentPlayer = dbc.Players.Single(p => p.PlayerId == id);
            mySpin.Number = currentPlayer.Number;
            //TODO: Update the Spin ViewModel with current player's lucky number
            

            //Game Play - Spin
            if (currentPlayer.Balance > 0)
            {
                currentPlayer.Balance -= 1; //charge for a spin
                mySpin.Spin();
            }

            //Game Play - Check Winning
            if (mySpin.isWinner)
            {
                currentPlayer.Balance += 2; //collect winnings
            }

            //TODO: Update the Spin ViewModel with the cuurent player's new balance
            mySpin.Balance = currentPlayer.Balance;
            dbc.SaveChanges();
            //TODO: Update Database

            return View(mySpin);
        }
    }
}