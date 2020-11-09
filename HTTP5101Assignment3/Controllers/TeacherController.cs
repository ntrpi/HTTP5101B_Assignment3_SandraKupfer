using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTTP5101Assignment3.Controllers
{
    // A Controller which allows you to route to dynamic pages.

    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            TeacherDataController controller = new TeacherDataController();
            return View( controller.getHighestTeacherId() );
        }

        // GET: /Teacher/List
        // Get information about the teacher(s) and send it
        // to List.cshtml.
        public ActionResult List()
        {
            // Get the list of teachers from the web api.
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<String> teachers = controller.listTeachers();
            return View( teachers );
        }

        // POST: /Teacher/Show
        [HttpPost]
        public ActionResult Show( int? teacherId )
        {
            TeacherDataController controller = new TeacherDataController();
            HTTP5101Assignment3.Models.Teacher teacher = controller.getTeacher( teacherId );
            return View( teacher );
        }
    }
}