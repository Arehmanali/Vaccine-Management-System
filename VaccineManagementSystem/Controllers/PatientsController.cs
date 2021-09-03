using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VaccineManagementSystem.Data;
using VaccineManagementSystem.Models;

namespace VaccineManagementSystem.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> RecievedClick(string mine)
        {
            int id = Convert.ToInt32(mine);
            var patient = await _context.Patient.FindAsync(id);

            var vacList = await _context.Vaccine.AsNoTracking().ToListAsync();
            var vacc = vacList.Find(x => x.VaccineName == patient.VaccineName);
            vacc.TotalDoseLeft = vacc.TotalDoseLeft - 1;
            patient.SecondDose = DateTime.Now.Date.ToShortDateString();
            _context.Update(patient);
            _context.Update(vacc);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            dynamic mymodel = new ExpandoObject();
            IEnumerable<Patient> PatientList = await _context.Patient.ToListAsync();
            IEnumerable<Vaccine> VaccineList = await _context.Vaccine.ToListAsync();
            mymodel.Vaccine = VaccineList;
            mymodel.Patient = PatientList;
            return View(mymodel);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            List<Vaccine> vaccineList = new List<Vaccine>();
            vaccineList = (from vac in _context.Vaccine
                           where vac.TotalDoseLeft > 0
                           select vac).ToList();
            vaccineList.Insert(0, new Vaccine { Id = 0, VaccineName = "Select" });
            ViewBag.ListofVaccine = new SelectList(vaccineList, "Id", "VaccineName");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientName,VaccineName,FirstDose,SecondDose")] Patient patient)
        {
            List<Vaccine> vaccineList = new List<Vaccine>();
            vaccineList =await (from vac in _context.Vaccine.AsNoTracking()
                           where vac.TotalDoseLeft > 0
                           select vac).ToListAsync();
            vaccineList.Insert(0, new Vaccine { Id = 0, VaccineName = "Select" });
            ViewBag.ListofVaccine = new SelectList(vaccineList, "Id", "VaccineName");


            var vacList = await _context.Vaccine.AsNoTracking().ToListAsync();
            var vacc = vacList.Find(x => x.Id == Convert.ToInt32(patient.VaccineName));
            vacc.TotalDoseLeft = vacc.TotalDoseLeft - 1;
            _context.Update(vacc);
            patient.VaccineName = vacc.VaccineName;
            DateTime dateAndTime = DateTime.Now;
            patient.FirstDose = dateAndTime.ToString("dd/MM/yyyy");
            _context.Add(patient);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }
    }
}
