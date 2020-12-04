using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP5101Assignment3.Models;

namespace HTTP5101Assignment3.Controllers
{
    public class StudentController : Controller
    {
        private StudentDataController controller = new StudentDataController();

        /// <summary>
        /// Get the highest student ID and send it to index.cshtml.
        /// </summary>
        /// <returns>A View containing an Int32 representing the highest teahcer id.</returns>
        /// <example>GET: /Student</example>
        [HttpGet]
        public ActionResult Index()
        {
            return View( new StudentDataController().getHighestId() );
        }

        /// <summary>
        /// Get information about the student(s) and send it to List.cshtml.
        /// </summary>
        /// <returns>A View containing an IEnumerable of Student objects.</returns>
        /// <example>GET: /Student/List</example>
        [HttpGet]
        public ActionResult List()
        {
            // Get the list of students from the web api.
            IEnumerable<Student> students = controller.listStudents();
            return View( students );
        }

        /// <summary>
        /// Get information for the student with the given ID and send it to 
        /// Show.cshtml.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>A View containing a Student object.</returns>
        /// <example>Not sure how to show an example for this function since it
        /// uses a POST request. I can say that this function is accessed by
        /// one of the forms in Student/index.cshtml.</example>
        [HttpPost]
        public ActionResult Show( int? studentId )
        {
            Student student = controller.getStudent( studentId );
            return View( student );
        }

        /// <summary>
        /// Get information for the student with the given ID and send it to 
        /// Show.cshtml.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View containing a Student object.</returns>
        /// <example>GET: /Student/Show/{id}</example>
        // Note that because we modified the routing configuration in 
        // App_Start/RouteConfig.cs, the name of the parameter we are
        // taking in this function MUST match the name of the optional parameter
        // in the routing configuration.
        [HttpGet]
        public ActionResult Show( int id )
        {
            Student student = controller.getStudent( id );
            return View( student );
        }

        /// <summary>
        /// Get the list of students that match the search criteria and send
        /// it to Results.chtml.
        /// </summary>
        /// <param name="columnName">A string that matches a column in the students
        /// table in the database.</param>
        /// <param name="columnValue">A string to look for in the values for the
        /// given column.</param>
        /// <returns>A view with a potentially empty list of students.</returns>
        /// <example>Not sure how to show an example for this function since it
        /// uses a POST request. I can say that this function is accessed by
        /// one of the forms in Student/index.cshtml.</example>
        // NOTE: This is very crude search functionality, just the minimum to show
        // that it can be done, a prototype as opposed to a full implementation. 
        // The column names are fine as they are selected, not typed, but the values 
        // could be anything. 
        [HttpPost]
        public ActionResult Results( string columnName, string columnValue )
        {
            IEnumerable<Student> students = controller.findStudents( columnName + " LIKE \"" + columnValue + "\"" );
            return View( students );
        }

        /// <summary>
        /// Returns a View for Add.chtml.
        /// </summary>
        /// <returns>An Add View.</returns>
        public ActionResult Add()
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
                objectType = "student",
                property = property
            } );
        }

        //POST : /Student/Create
        [HttpPost]
        public ActionResult Create( string firstName, string lastName, string studentNumber, string enrollDate )
        {
            if( firstName == null || firstName.Length == 0 ) {
                return getRedirectToError( "first name" );

            } else if( lastName == null || lastName.Length == 0 ) {
                return getRedirectToError( "last name" );

            } else if( studentNumber == null || studentNumber.Length == 0 ) {
                return getRedirectToError( "employee number" );

            } else if( enrollDate == null ) {
                return getRedirectToError( "enroll date" );
            }

            Student student = new Student();
            student.studentFName = firstName;
            student.studentLName = lastName;
            student.studentNumber = studentNumber;
            student.enrollDate = Convert.ToDateTime( enrollDate ); // "dd/mm/yyyy"

            controller.addStudent( student );

            return RedirectToAction( "List" );
        }

        //GET : /Student/ConfirmDelete/{id}
        public ActionResult ConfirmDelete( int id )
        {
            Student student = (Student) controller.findStudents( "studentid=" + id ).First();
            return View( student );
        }

        //POST : /Student/Delete/{id}
        [HttpPost]
        public ActionResult Delete( int id )
        {
            controller.deleteStudent( id );

            // TODO: delete from studentsxclasses

            return RedirectToAction( "List" );
        }

    }
}