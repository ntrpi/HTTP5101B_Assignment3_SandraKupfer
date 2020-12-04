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

        public ActionResult Add()
        {
            return View();
        }

        private ActionResult getRedirectToError( string property )
        {
            return RedirectToAction( "ShowError", "Assignment4", new {
                objectType = "class",
                property = property
            } );
        }

        //POST : /Class/Create
        [HttpPost]
        public ActionResult Create( string className, string classCode, DateTime startDate, DateTime finishDate, int teacherId )
        {
            if( className == null || className.Length == 0 ) {
                return getRedirectToError( "class name" );

            } else if( classCode == null || classCode.Length == 0 ) {
                return getRedirectToError( "class code" );

            } else if( startDate == null ) {
                return getRedirectToError( "start date" );

            } else if( finishDate == null ) {
                return getRedirectToError( "finish date" );
            }

            Class course = new Class();
            course.className = className;
            course.classCode = classCode;
            course.startDate = Convert.ToDateTime( startDate ); // "dd/mm/yyyy"
            course.finishDate = Convert.ToDateTime( finishDate ); // "dd/mm/yyyy"
            course.teacherId = teacherId;

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

    }
}