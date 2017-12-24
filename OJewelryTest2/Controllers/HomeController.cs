using OJewelryTest2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OJewelryTest2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Populate model from DB
            /*
            HomeIndexViewModel m = new HomeIndexViewModel();
            OJewelryEntities dc = new OJewelryEntities();
            var ClientList = (from client in dc.Clients
                              join company in dc.Companies on client.Id equals company.Id

                              select new HomeIndexViewModel()
                              {
                                  ID = client.Id,
                                  ClientName = client.Name,
                                  ClientPhone = client.Phone,
                                  ClientEmail = client.Email,
                                  CompanyName = company.Name
                              }
                      ).ToList();
            */
            return ClientList();
        }

        public ActionResult ClientList()
        {
            // Populate model from DB
            HomeIndexViewModel m = new HomeIndexViewModel();
            OJewelryEntities dc = new OJewelryEntities();
            var ClientList = (from client in dc.Clients
                              join company in dc.Companies on client.Id equals company.Id

                              select new HomeIndexViewModel()
                              {
                                  ID = client.Id,
                                  ClientName = client.Name,
                                  ClientPhone = client.Phone,
                                  ClientEmail = client.Email,
                                  CompanyName = company.Name
                              }
                      ).ToList();
            return View("ClientList", ClientList);
        }

        public ActionResult ClientCreate()
        {
            //HomeIndexViewModel m = new HomeIndexViewModel();
            return View();
        }

        [HttpPost]
        public ActionResult ClientCreate(FormCollection form)
        {
            var dc = new OJewelryEntities();
            Client cli = new Client()
            {
                Name = form["ClientName"],
                Email = form["ClientEmail"],
                Phone = form["ClientPhone"]
            };
            dc.Clients.Add(cli);
            Company com = new Company()
            {
                Name = form["CompanyName"],
                ClientID = cli.Id           
            };
            dc.Companies.Add(com);
            if (String.IsNullOrEmpty(cli.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
            }
            if (ModelState.IsValid)
            {
                dc.SaveChanges();
                return ClientList();
            }
            return View(cli);
        }

        public ActionResult ClientDetail(int? id)
        {
            int i;
            if (!id.HasValue)
            { i = 1; } else { i = id.Value; }
            ViewBag.Message = "Client Detail";
            OJewelryEntities dc = new OJewelryEntities();
            var cli = (from client in dc.Clients
                       join company in dc.Companies on client.Id equals company.Id
                       where client.Id.Equals(i)
                       select new HomeIndexViewModel()
                       {
                           ID = client.Id,
                           ClientName = client.Name,
                           ClientPhone = client.Phone,
                           ClientEmail = client.Email,
                           CompanyName = company.Name
                       }).Single();
            /*
            var cli = from row in dc.Clients
                      where row.Id.Equals(i)
                      select row;
            */
            return View(cli);
        }

        public ActionResult ClientEdit(int? id)
        {
            int i;
            if (!id.HasValue)
            {
                return View();
            }
            else { i = id.Value; }
            ViewBag.Message = "Client Edit";
            OJewelryEntities dc = new OJewelryEntities();
            var cli = (from client in dc.Clients
                       //join company in dc.Companies on client.Id equals company.Id
                       where client.Id.Equals(i)
                       select new HomeIndexViewModel()
                       {
                           ID = client.Id,
                           ClientName = client.Name,
                           ClientPhone = client.Phone,
                           ClientEmail = client.Email,
                           //CompanyName = company.Name
                       }).Single();
            /*
            var cli = from row in dc.Clients
                      where row.Id.Equals(i)
                      select row;
            */
            return View(cli);
        }

        [HttpPost]
        public ActionResult ClientEdit(FormCollection form)
        {
            ViewBag.Message = "Client Edit (post)";
            var dc = new OJewelryEntities();
            // Get Client from DB based on ID
            int i = Int32.Parse(form["id"]);
            var cli = dc.Clients.Find(i);

            //Client cli = new Client()
            {
                cli.Name = form["ClientName"];
                cli.Email = form["ClientEmail"];
                cli.Phone = form["ClientPhone"];
            };
            TryUpdateModel(cli, new string[] { "Name",  "ClientEmail" }, form.ToValueProvider());
            if (String.IsNullOrEmpty(cli.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
            }
            // Update cli in DB
            if (ModelState.IsValid)
            {
                
                dc.SaveChanges();
                return ClientList();
            }
            //populate a model wth the data
            HomeIndexViewModel m = new HomeIndexViewModel()
            {
                ID = cli.Id,
                ClientName = cli.Name,
                ClientPhone = cli.Phone,
                ClientEmail = cli.Email,
            };
            return View(m);
        }

        public ActionResult ClientDelete(int? id)
        {
            
            var dc = new OJewelryEntities();
            /*
            int i;
            if (!id.HasValue)
            { i = 1; }
            else { i = id.Value; }
            // Get Client from DB based on ID
            var cli = dc.Clients.Find(i);
            //populate a model wth the data
            HomeIndexViewModel m = new HomeIndexViewModel()
            {
                ID = cli.Id,
                ClientName = cli.Name,
                ClientPhone = cli.Phone,
                ClientEmail = cli.Email,
            };
            return View(m);
            */
            var cli = dc.Clients.Find(id);
            var comp = dc.Companies.Where(co => co.ClientID == cli.Id);
            dc.Companies.Remove(comp.Single());
            dc.Clients.Remove(cli);
            dc.SaveChanges();
            return ClientList();
        }

        /*
        [HttpPost]
        public ActionResult ClientDelete(FormCollection form)
        {
            var dc = new OJewelryEntities();
            int i = Int32.Parse(form["id"]);
            var cli = dc.Clients.Find(i);
            dc.Clients.Remove(cli);
            return ClientList();
        }
        */
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}