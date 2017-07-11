using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        public string Index()
        {
            return "";
        }
       public string Welcome(string name, int ID = 1)
       {
           return ""; 
       }
    }
}
