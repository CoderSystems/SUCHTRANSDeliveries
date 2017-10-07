using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inc2SuchTrans.ViewModels;
using Inc2SuchTrans.Models;
using System.Web.Helpers;

namespace Inc2SuchTrans.BLL
{
    public class TransactionLogic
    {
        STLogisticsEntities FDB = new STLogisticsEntities();
        public static List<string> ListOfDates()
        {
            List<string> DateList = new List<string>();
            DateTime StartDate = DateTime.Now.AddMonths(-12);
            string year = DateTime.Now.Year.ToString();
            for (int i = 0; i < 12; i++)
            {
                switch (StartDate.Month.ToString())
                {
                    case "1":
                        DateList.Add("January " + StartDate.Year.ToString());
                        break;
                    case "2":
                        DateList.Add("February " + StartDate.Year.ToString());
                        break;
                    case "3":
                        DateList.Add("March " + StartDate.Year.ToString());
                        break;
                    case "4":
                        DateList.Add("April " + StartDate.Year.ToString());
                        break;
                    case "5":
                        DateList.Add("May " + StartDate.Year.ToString());
                        break;
                    case "6":
                        DateList.Add("June " + StartDate.Year.ToString());
                        break;
                    case "7":
                        DateList.Add("July " + StartDate.Year.ToString());
                        break;
                    case "8":
                        DateList.Add("August " + StartDate.Year.ToString());
                        break;
                    case "9":
                        DateList.Add("September " + StartDate.Year.ToString());
                        break;
                    case "10":
                        DateList.Add("October " + StartDate.Year.ToString());
                        break;
                    case "11":
                        DateList.Add("November " + StartDate.Year.ToString());
                        break;
                    case "12":
                        DateList.Add("December " + StartDate.Year.ToString());
                        break;
                }

                StartDate = StartDate.AddMonths(1);
            }
            return DateList;
        }

        public static string Month(string monthnum)
        {
            string Month = "";
            switch(monthnum)
            {
                case "1":
                    Month = "January";
                    break;
                case "2":
                    Month = "February";
                    break;
                case "3":
                    Month = "March";
                    break;
                case "4":
                    Month = "April";
                    break;
                case "5":
                    Month = "May";
                    break;
                case "6":
                    Month = "June";
                    break;
                case "7":
                    Month = "July";
                    break;
                case "8":
                    Month = "August";
                    break;
                case "9":
                    Month = "September";
                    break;
                case "10":
                    Month = "October";
                    break;
                case "11":
                    Month = "November";
                    break;
                case "12":
                    Month = "December";
                    break;
                default:
                    Month = "Error";
                    break;
            }
            return Month;
        }
    }
}