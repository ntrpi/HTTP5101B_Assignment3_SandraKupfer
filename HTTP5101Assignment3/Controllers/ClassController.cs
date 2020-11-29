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
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get information about the classes and send it to List.cshtml.
        /// </summary>
        /// <returns>A View containing an IEnumerable of Class objects.</returns>
        /// <example>GET: /Class/List</example>
        [HttpGet]
        public ActionResult List()
        {
            // Get the list of teachers from the web api.
            ClassDataController controller = new ClassDataController();
            IEnumerable<Class> classes = controller.listClasses();
            return View( classes );
        }
    }
}