﻿using System;
using System.Linq;
using System.Web.Mvc;
using medical_appointment_system.Models;
using medical_appointment_system.Services;

namespace medical_appointment_system.Controllers
{
    public class PatientsController : Controller
    {
        PatientService service = new PatientService();

        private Patient FindById(int id)
        {
            Patient patient = new Patient { UserId = id };
            var result = service.ExecuteRead("GET_BY_ID", patient).FirstOrDefault();

            if (result != null)
            {
                return result;
            }

            return null;
        }

        public ActionResult Index()
        {
            return View(service.ExecuteRead("GET_ALL", new Patient()));
        }

        public ActionResult Create()
        {
            return View(new Patient());
        }

        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                patient.Role = "paciente";
                service.ExecuteWrite("INSERT", patient);
                TempData["Success"] = "¡Paciente creado correctamente!";
                return RedirectToAction("Index");
            }
            catch (ApplicationException ex)
            {
                ViewBag.Message = ex.Message;
            }
            catch (Exception)
            {
                ViewBag.Message = "Ocurrió un error inesperado. Intenta más tarde.";
            }

            return View(patient);
        }

        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            return View(FindById(id));
        }

        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            int process = service.ExecuteWrite("UPDATE", patient);

            if (process >= 0)
            {
                TempData["Success"] = "¡Paciente actualizado correctamente!";
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            return View(FindById(id));
        }
    }
}