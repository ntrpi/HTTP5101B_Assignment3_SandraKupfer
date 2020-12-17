using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP5101Assignment3.Models;

namespace HTTP5101Assignment3.Controllers
{
    public class ClassController : Controller
    {
        private ClassDataController controller = new ClassDataController();

        /// <summary>
        /// Get the highest class ID and send it to index.cshtml.
        /// </summary>
        /// <returns>A View containing an Int32 representing the highest teahcer id.</returns>
        /// <example>GET: /Class</example>
        [HttpGet]
        public ActionResult Index()
        {
            return View( new ClassDataController().getHighestId() );
        }

        /// <summary>
        /// Get information about the class(s) and send it to List.cshtml.
        /// </summary>
        /// <returns>A View containing an IEnumerable of Class objects.</returns>
        /// <example>GET: /Class/List</example>
        [HttpGet]
        public ActionResult List()
        {
            // Get the list of classes from the web api.
            IEnumerable<Class> classes = controller.listClasses();
            return View( classes );
        }

        /// <summary>
        /// Get information for the class with the given ID and send it to 
        /// Show.cshtml.
        /// </summary>
        /// <param name="classId"></param>
        /// <returns>A View containing a Class object.</returns>
        /// <example>Not sure how to show an example for this function since it
        /// uses a POST request. I can say that this function is accessed by
        /// one of the forms in Class/index.cshtml.</example>
        [HttpPost]
        public ActionResult Show( int? classId )
        {
            Class course = controller.getClass( classId );
            return View( course );
        }

        /// <summary>
        /// Get information for the class with the given ID and send it to 
        /// Show.cshtml.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View containing a Class object.</returns>
        /// <example>GET: /Class/Show/{id}</example>
        // Note that because we modified the routing configuration in 
        // App_Start/RouteConfig.cs, the name of the parameter we are
        // taking in this function MUST match the name of the optional parameter
        // in the routing configuration.
        [HttpGet]
        public ActionResult Show( int id )
        {
            Class course = controller.getClass( id );
            return View( course );
        }

        /// <summary>
        /// Get the list of classes that match the search criteria and send
        /// it to Results.chtml.
        /// </summary>
        /// <param name="columnName">A string that matches a column in the classes
        /// table in the database.</param>
        /// <param name="columnValue">A string to look for in the values for the
        /// given column.</param>
        /// <returns>A view with a potentially empty list of classes.</returns>
        /// <example>Not sure how to show an example for this function since it
        /// uses a POST request. I can say that this function is accessed by
        /// one of the forms in Class/index.cshtml.</example>
        // NOTE: This is very crude search functionality, just the minimum to show
        // that it can be done, a prototype as opposed to a full implementation. 
        // The column names are fine as they are selected, not typed, but the values 
        // could be anything. 
        [HttpPost]
        public ActionResult Results( string columnName, string columnValue )
        {
            IEnumerable<Class> classes = controller.findClasses( columnName + " LIKE \"" + columnValue + "\"" );
            return View( classes );
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
                objectType = "class",
                property = property
            } );
        }

        /// <summary>
        /// Add a new Class to the database using the parameters passed in.
        /// Return an ActionResult that takes you to the list of students.
        /// </summary>
        /// <param name="className">The class name as a string.</param>
        /// <param name="classCode">The class code as a string.</param>
        /// <param name="startDate">The date the class starts.</param>
        /// <param name="finishDate">The date the class ends.</param>
        /// <param name="teacherId">The teacher's ID as an integer.</param>
        /// <returns>RedirectToAction that takes you to the classes list.</returns>
        //POST : /Class/Create
        [HttpPost]
        public ActionResult Create( string className, string classCode, string startDate, string finishDate, int teacherId )
        {
            Class course = new Class( className, classCode, teacherId, startDate, finishDate );
            string propertyError = course.getPropertyError();
            if( propertyError != null ) {
                return getRedirectToError( propertyError );
            }

            controller.addClass( course );

            return RedirectToAction( "List" );
        }

        //GET : /Class/ConfirmDelete/{id}
        public ActionResult ConfirmDelete( int id )
        {
            Class course = (Class) controller.findClasses( "classid=" + id ).First();
            return View( course );
        }

        //POST : /Class/Delete/{id}
        [HttpPost]
        public ActionResult Delete( int id )
        {
            controller.deleteClass( id );

            // TODO: delete from studentsxclasses

            return RedirectToAction( "List" );
        }

        //POST : /Class/Delete/{id}
        [HttpPost]
        public ActionResult DeleteClasses( string columnName, string value )
        {
            IEnumerable<Class> classes = controller.findClasses( columnName + "=" + value );
            foreach( Class c in classes ) {
                Delete( c.classId );
            }
            return RedirectToAction( "List" );
        }

        /// <summary>
        /// Update a Class in the database using the parameters passed in.
        /// Return an ActionResult that takes you to the list of students.
        /// </summary>
        /// <param name="className">The class name as a string.</param>
        /// <param name="classCode">The class code as a string.</param>
        /// <param name="startDate">The date the class starts.</param>
        /// <param name="finishDate">The date the class ends.</param>
        /// <param name="teacherId">The teacher's ID as an integer.</param>
        /// <returns>RedirectToAction that takes you to the classes list.</returns>
        //POST : /Class/Create
        [HttpPost]
        public ActionResult Update( string className, string classCode, string startDate, string finishDate, int teacherId )
        {
            Class course = new Class( className, classCode, teacherId, startDate, finishDate );
            string propertyError = course.getPropertyError();
            if( propertyError != null ) {
                return getRedirectToError( propertyError );
            }

            controller.updateClass( course );

            return RedirectToAction( "List" );
        }

    }
}