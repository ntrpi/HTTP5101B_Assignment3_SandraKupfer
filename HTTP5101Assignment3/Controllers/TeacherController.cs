using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP5101Assignment3.Models;

namespace HTTP5101Assignment3.Controllers
{
    // This controller is the interface between the views and and the
    // TeacherDataController.
    public class TeacherController : Controller
    {
        /// <summary>
        /// Get the highest teacher ID and send it to index.cshtml.
        /// </summary>
        /// <returns>A View containing an Int32 representing the highest teahcer id.</returns>
        /// <example>GET: /Teacher</example>
        [HttpGet]
        public ActionResult Index()
        {
            return View( new TeacherDataController().getHighestId() );
        }

        /// <summary>
        /// Get information about the teacher(s) and send it to List.cshtml.
        /// </summary>
        /// <returns>A View containing an IEnumerable of Teacher objects.</returns>
        /// <example>GET: /Teacher/List</example>
        [HttpGet]
        public ActionResult List()
        {
            // Get the list of teachers from the web api.
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.listTeachers();
            return View( teachers );
        }

        /// <summary>
        /// Get information for the teacher with the given ID and send it to 
        /// Show.cshtml.
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns>A View containing a Teacher object.</returns>
        /// <example>Not sure how to show an example for this function since it
        /// uses a POST request. I can say that this function is accessed by
        /// one of the forms in Teacher/index.cshtml.</example>
        [HttpPost]
        public ActionResult Show( int? teacherId )
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.getTeacher( teacherId );
            return View( teacher );
        }

        /// <summary>
        /// Get information for the teacher with the given ID and send it to 
        /// Show.cshtml.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View containing a Teacher object.</returns>
        /// <example>GET: /Teacher/Show/{id}</example>
        // Note that because we modified the routing configuration in 
        // App_Start/RouteConfig.cs, the name of the parameter we are
        // taking in this function MUST match the name of the optional parameter
        // in the routing configuration.
        [HttpGet]
        public ActionResult Show( int id )
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.getTeacher( id );
            return View( teacher );
        }

        /// <summary>
        /// Get the list of teachers that match the search criteria and send
        /// it to Results.chtml.
        /// </summary>
        /// <param name="columnName">A string that matches a column in the teachers
        /// table in the database.</param>
        /// <param name="columnValue">A string to look for in the values for the
        /// given column.</param>
        /// <returns>A view with a potentially empty list of teachers.</returns>
        /// <example>Not sure how to show an example for this function since it
        /// uses a POST request. I can say that this function is accessed by
        /// one of the forms in Teacher/index.cshtml.</example>
        // NOTE: This is very crude search functionality, just the minimum to show
        // that it can be done, a prototype as opposed to a full implementation. 
        // The column names are fine as they are selected, not typed, but the values 
        // could be anything. 
        [HttpPost]
        public ActionResult Results( string columnName, string columnValue )
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.findTeachers( columnName + " LIKE \"" + columnValue + "\"" );
            return View( teachers );
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        private ActionResult getRedirectToError( string property )
        {
            return RedirectToAction( "ShowError", "Assignment4", new {
                objectType = "teacher",
                property = property
            } );
        }

        //POST : /Teacher/Create
        [HttpPost]
        public ActionResult Create( string firstName, string lastName, string employeeNumber, string hireDate, decimal salary )
        {
            if( firstName == null || firstName.Length == 0 ) {
                return getRedirectToError( "first name" );

            } else if( lastName == null || lastName.Length == 0 ) {
                return getRedirectToError( "last name" );

            } else if( employeeNumber == null || employeeNumber.Length == 0 ) {
                return getRedirectToError( "employee number" );

            } else if( hireDate == null ) {
                return getRedirectToError( "hire date" );
            }

            Teacher teacher = new Teacher();
            teacher.teacherFName = firstName;
            teacher.teacherLName = lastName;
            teacher.employeeNumber = employeeNumber;
            teacher.hireDate = Convert.ToDateTime( hireDate ); // "dd/mm/yyyy"
            teacher.salary = salary;

            TeacherDataController controller = new TeacherDataController();
            controller.addTeacher( teacher );

            return RedirectToAction( "List" );
        }

        //GET : /Teacher/ConfirmDelete/{id}
        public ActionResult ConfirmDelete( int id )
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = (Teacher) controller.findTeachers( "teacherid=" + id ).First();

            return View( teacher );
        }

        //POST : /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete( int id )
        {
            TeacherDataController controller = new TeacherDataController();
            controller.deleteTeacher( id );
            return RedirectToAction( "List" );
        }

    }
}