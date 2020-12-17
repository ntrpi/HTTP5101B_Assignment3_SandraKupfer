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
        /// Get information for the teacher with the given ID and send it to 
        /// Edit.cshtml.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View containing a Teacher object.</returns>
        /// <example>GET: /Teacher/Update/{id}</example>
        // Note that because we modified the routing configuration in 
        // App_Start/RouteConfig.cs, the name of the parameter we are
        // taking in this function MUST match the name of the optional parameter
        // in the routing configuration.
        [HttpGet]
        public ActionResult Edit( int id )
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.getTeacher( id );
            return View( teacher );
        }

        [HttpGet]
        public ActionResult Edit_Ajax( int id )
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.getTeacher( id );
            return View( teacher );
        }

        /// <summary>
        /// Update a Teacher in the database using the parameters passed in.
        /// Return an ActionResult that takes you to the list of teachers.
        /// </summary>
        /// <param name="firstName">The teacher's first name as a string.</param>
        /// <param name="lastName">The teacher's last name as a string.</param>
        /// <param name="employeeNumber">The teacher's employee number as a string.</param>
        /// <param name="hireDate">The date the teacher was hired as a string.</param>
        /// <param name="salary">A decimal holding the teacher's salary.</param>
        /// <returns>RedirectToAction that takes you to the teachers list.</returns>
        //POST : /Teacher/Update
        [HttpPost]
        public ActionResult Update( int id, string firstName, string lastName, string employeeNumber, string hireDate, decimal salary )
        {
            Teacher teacher = new Teacher( id, firstName, lastName, employeeNumber, hireDate, salary );
            string propertyError = teacher.getPropertyError();
            if( propertyError != null ) {
                return getRedirectToError( propertyError );
            }

            TeacherDataController controller = new TeacherDataController();
            controller.updateTeacher( teacher );

            return RedirectToAction( "List" );
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
        /// one of the forms in Assignment4/Index.cshtml.</example>
        [HttpPost]
        public ActionResult Results( string columnName, string columnValue )
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.findTeachers( columnName + " LIKE \"" + columnValue + "\"" );
            return View( teachers );
        }

        /// <summary>
        /// Returns a View for Add.chtml.
        /// </summary>
        /// <returns>An Add View.</returns>
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Returns a View for Add_Ajax.chtml.
        /// </summary>
        /// <returns>An Add_Ajax View.</returns>
        [HttpGet]
        public ActionResult Add_Ajax()
        {
            return View();
        }

        /// <summary>
        /// A utility function to create an ActionResult that will
        /// redirect to an error page if validation for Create fails.
        /// </summary>
        /// <param name="property">The name of the property that is invalid.</param>
        /// <returns>A RedirectToAction that will bring up the Assignment4/ShowError page.</returns>
        private ActionResult getRedirectToError( string property )
        {
            return RedirectToAction( "ShowError", "Assignment4", new {
                objectType = "teacher",
                property = property
            } );
        }

        /// <summary>
        /// Add a new Teacher to the database using the parameters passed in.
        /// Return an ActionResult that takes you to the list of teachers.
        /// </summary>
        /// <param name="firstName">The teacher's first name as a string.</param>
        /// <param name="lastName">The teacher's last name as a string.</param>
        /// <param name="employeeNumber">The teacher's employee number as a string.</param>
        /// <param name="hireDate">The date the teacher was hired as a string.</param>
        /// <param name="salary">A decimal holding the teacher's salary.</param>
        /// <returns>RedirectToAction that takes you to the teachers list.</returns>
        //POST : /Teacher/Create
        [HttpPost]
        public ActionResult Create( string firstName, string lastName, string employeeNumber, string hireDate, decimal salary )
        {
            Teacher teacher = new Teacher( firstName, lastName, employeeNumber, hireDate, salary );
            string propertyError = teacher.getPropertyError();
            if( propertyError != null ) {
                return getRedirectToError( propertyError );
            }

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