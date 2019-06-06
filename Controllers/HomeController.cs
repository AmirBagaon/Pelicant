using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pelicant.Models;

namespace Pelicant.Controllers
{
    public class HomeController : Controller
    {
        /*
         * 
         * A commented part, used to demonstarte connection to AMT (feasibility only),
         * and connection to a database that checks whether the user already participated.
         * 
        private static Object lockObj = new object(); //(static member means it's the same for all sessions) 
            string mobile = "not_mobile";
            if (Request.Browser.IsMobileDevice)
            {
                mobile = "mobile_user";
            }

            string assignmentId = Request.QueryString["assignmentId"];
            string userId = Request.QueryString["workerId"];

            //friend assignment
            if (assignmentId == null)
            {
                Session["user_id"] = "friend1";
                Session["turkAss"] = "turkAss";
                Session["hitId"] = "hit id friend";
                //assignmentId = "aaa"; //DEBUG FOR MYSELF
            }

            //from AMT but did not took the assigment (didn't click accept)
            // check whether he has already participated (before 'accept' everyone has userId. After accepting the HIT they get the assignmentId).
            else if (assignmentId.Equals("ASSIGNMENT_ID_NOT_AVAILABLE"))
            {
                String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                using (SQLiteConnection sqlConnection1 = new SQLiteConnection(connectionString))
                {
                    SQLiteCommand cmd = new SQLiteCommand("Select UserId from [UserInfo] Where UserId='" + userId + "'");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;
                    sqlConnection1.Open();
                    string UserId = (string)cmd.ExecuteScalar();
                    sqlConnection1.Close();
                    if (UserId != null && UserId != "") //already participated
                    {
                        return View("Redirect");
                    }

                    ///// SEQUENTIAL
                    sqlConnection1.Open();
                    SQLiteCommand cmd2 = new SQLiteCommand("Select Id from [Results2] Where Used='" + 0 + "'");
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = sqlConnection1;

                    if (cmd2.ExecuteScalar() == null) //i.e., all the Used values are 1. don't update Results fields (and Redirect). 
                    {
                        sqlConnection1.Close();
                        return View("RedirectLater");
                    }
                    sqlConnection1.Close();
                    ///// 
                }
            }

            //from AMT and accepted the assigment - continue to experiment 
            //if (assignmentId == "aaa") //DEBUG FOR MYSELF
            else
            {
                Session["button"] = 1;
                Session["user_id"] = Request.QueryString["workerId"];   //save participant's user ID
                Session["turkAss"] = Request.QueryString["assignmentId"]; ;  //save participant's assignment ID
                Session["hitId"] = Request.QueryString["hitId"];
                Session["BeginTime"] = DateTime.Now.ToString();

                ////DEBUG FOR MYSELF
                //Session["user_id"] = "a6";
                //Session["turkAss"] = "aaa";
                //Session["hitId"] = "xx";


                String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                using (SQLiteConnection sqlConnection1 = new SQLiteConnection(connectionString))
                {
                    SQLiteCommand cmd = new SQLiteCommand("Select UserId from [UserInfo] Where UserId='" + Session["user_id"] + "'");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;
                    sqlConnection1.Open();
                    string UserId = (string)cmd.ExecuteScalar();
                    if (UserId == null || UserId == "")
                    {
                        //new user - insert to DB
                        DateTime curentT = DateTime.Now;
                        cmd = new SQLiteCommand("insert into [UserInfo] (UserId, AssignmentId, Mobile, BeginTime) VALUES ('" + Session["user_id"] + "','" + Session["turkAss"] + "','" + mobile + "','" + curentT.ToString() + "')");
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = sqlConnection1;
                        cmd.ExecuteNonQuery();
                        sqlConnection1.Close();

                        ///// SEQUENTIAL
                        Session["currId"] = null;
                        Session["currVal"] = null;
                        //select Id where used=0 (the first to be found) and save it in session and select also the corresponding Value and save it in session
                        lock (lockObj)
                        {
                            sqlConnection1.Open();
                            SQLiteCommand cmd2 = new SQLiteCommand("Select Id from [Results2] Where Used='" + 0 + "'");
                            cmd2.CommandType = CommandType.Text;
                            cmd2.Connection = sqlConnection1;
                            if (cmd2.ExecuteScalar() == null) //i.e., all the Used values are 1. don't update PreviousResults fields (and Redirect). 
                            {
                                sqlConnection1.Close();
                                return View("RedirectLater");
                            }

                            else //else everyting Ok and the player can proceed to the game
                            {
                                Session["currId"] = (long)cmd2.ExecuteScalar();
                                sqlConnection1.Close();

                                sqlConnection1.Open();
                                SQLiteCommand cmd3 = new SQLiteCommand("Select PrevValue from [Results2] Where Id='" + Session["currId"] + "'");
                                cmd3.CommandType = CommandType.Text;
                                cmd3.Connection = sqlConnection1;
                                long prevVal = (long)cmd3.ExecuteScalar(); ;
                                sqlConnection1.Close();

                                sqlConnection1.Open();
                                SQLiteCommand cmd5 = new SQLiteCommand("Select Value from [Results2] Where Id='" + Session["currId"] + "'");
                                cmd5.CommandType = CommandType.Text;
                                cmd5.Connection = sqlConnection1;
                                long val = (long)cmd5.ExecuteScalar(); ;
                                sqlConnection1.Close();

                                Session["prevValue1"] = prevVal;
                                Session["prevValue2"] = val;
                                Session["currVal"] = Math.Max(prevVal, val);
                                //sqlConnection1.Open();
                                //SQLiteCommand cmd4 = new SQLiteCommand("UPDATE Results SET UserId=@UserId, AssignmentId=@AssignmentId, Used=@Used WHERE Id='" + Session["currId"] + "'");
                                //cmd4.CommandType = CommandType.Text;
                                //cmd4.Connection = sqlConnection1;
                                //cmd4.Parameters.AddWithValue("@UserId", Session["user_id"]);
                                //cmd4.Parameters.AddWithValue("@AssignmentId", Session["turkAss"]);
                                //cmd4.Parameters.AddWithValue("@Used", 1);
                                //cmd4.ExecuteNonQuery();
                                //sqlConnection1.Close();
                                sqlConnection1.Open();
                                SQLiteCommand cmd4 = new SQLiteCommand("UPDATE Results2 SET Used=@Used WHERE Id='" + Session["currId"] + "'");
                                cmd4.CommandType = CommandType.Text;
                                cmd4.Connection = sqlConnection1;
                                cmd4.Parameters.AddWithValue("@Used", 1);
                                cmd4.ExecuteNonQuery();
                                sqlConnection1.Close();
                            }
                        }
                        /////
                    }
                    else
                    {
                        sqlConnection1.Close();
                        return View("Redirect");
                    }
                }
            }
            Session["value"] = -1;
            return View();
        }


         * */




        static RobotModel robotModel = new RobotModel();
        static CameraModel cameraModel = new CameraModel();



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            Console.WriteLine("^^^^$$");

            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Manager()
        {
            Console.WriteLine("^^^^$$");

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Game()
        {
            ViewData["Message"] = "Your contact page.";
            ViewData["x"] = robotModel.x;
            ViewData["y"] = robotModel.y;
            ViewData["color"] = robotModel.color;
            ViewData["width"] = robotModel.width;
            ViewData["height"] = robotModel.height;
            ViewData["cameraIP"] = cameraModel.getCameraIP;
            ViewData["cameraHeight"] = cameraModel.height;
            ViewData["cameraWidth"] = cameraModel.width;
            return View();
        }
        public ActionResult MainMenu()
        {
            ViewData["Message"] = "MainMenu";
            ViewData["x"] = robotModel.x;
            ViewData["y"] = robotModel.y;
            ViewData["color"] = robotModel.color;
            ViewData["width"] = robotModel.width;
            ViewData["height"] = robotModel.height;
            Console.WriteLine("@#$$$$");

            return View();
        }
        [HttpGet]
        public JObject GetRobotInfo()
        {
            JObject data = new JObject();
            data["x"] = robotModel.x;
            data["y"] = robotModel.y;
            return data;
        }
        [HttpPost]
        public JObject setRobotInfo(string x, string y)
        {
            robotModel.x = x;
            robotModel.y = y;
            return null;
        }


        public ActionResult Privacy()
        {
            return View();
        }

        // GET: First/MakeRemove/
        public ActionResult MakeRemove()
        {
            Console.WriteLine("IN MAKE REMOVE!!!");
            return RedirectToAction("index");
        }
    }
}
