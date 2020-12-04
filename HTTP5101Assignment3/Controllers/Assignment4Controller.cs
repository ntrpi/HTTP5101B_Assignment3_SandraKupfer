using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP5101Assignment3.Models;

namespace HTTP5101Assignment3.Controllers
{
    public class Assignment4Controller : Controller
    {
        // GET: Assignment4
        public ActionResult Index()
        {
            MaxIds maxIds = new MaxIds();
            maxIds.teacherMaxId = new TeacherDataController().getHighestId();
            maxIds.studentMaxId = new StudentDataController().getHighestId();
            maxIds.classMaxId = new ClassDataController().getHighestId();

            ViewBag.Title = "Assignment 4";

            return View( maxIds );
        }

        [HttpGet]
        public ActionResult ShowError( string objectType, string property )
        {
            AddError addError = new AddError( objectType, property );
            return View( addError );
        }


    }
}