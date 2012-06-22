using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Docum.Lib.Models;
using Docum.Lib.MongoDb;
using Docum.Lib.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Docum.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your quintessential contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult GetMyDocuments()
        {
            var user = new DocoUserService(new MongoRepository<DocoUser>());
            var docList = user.SelectById("4fe4bc5dc155aa1940bac1f3").DocumentList;
            return Json(docList, JsonRequestBehavior.AllowGet);
        } 

        public PartialViewResult DocumentDetails(string documentId)
        {
            var userSrv = new DocoUserService(new MongoRepository<DocoUser>());
            var docId = ObjectId.Parse(documentId);
            var res = userSrv.SelectById("4fe4bc5dc155aa1940bac1f3").DocumentList.Where(x=>x.DocId == docId).FirstOrDefault();
            return  PartialView("DocumentDetails",res );
        }
       
    }
}
