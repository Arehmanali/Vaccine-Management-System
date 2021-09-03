using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VaccineManagementSystem.Data;
using VaccineManagementSystem.Models;

namespace VaccineManagementSystem.Controllers
{
    public class VaccinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VaccinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vaccines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vaccine.ToListAsync());
        }

        // GET: Vaccines/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Vaccines/AddDoses
        [HttpGet]
        public ActionResult AddDoses()
        {
            List<Vaccine> vaccineList = new List<Vaccine>();
            vaccineList = (from vac in _context.Vaccine
                           select vac).ToList();
            vaccineList.Insert(0,new Vaccine { Id = 0, VaccineName = "Select" });
            ViewBag.ListofVaccine = new SelectList(vaccineList, "Id", "VaccineName");
            return View();
        }

        // POST: Vaccines/AddDoses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> AddDoses(int id,[Bind("Id,VaccineName,DosesRequired,DaysBetween,TotalDoseRecieved,TotalDoseLeft")] Vaccine vaccine)
        {
            if (id != vaccine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vacList = await _context.Vaccine.AsNoTracking().ToListAsync();
                    var vac = vacList.Find(x => x.Id == id);
                    vaccine.VaccineName = vac.VaccineName;
                    vaccine.DosesRequired = vac.DosesRequired;
                    vaccine.DaysBetween = vac.DaysBetween;
                    vaccine.TotalDoseLeft = vaccine.TotalDoseRecieved+vac.TotalDoseLeft;
                    vaccine.TotalDoseRecieved = vac.TotalDoseRecieved + vaccine.TotalDoseRecieved;
                    
                    _context.Update(vaccine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccineExists(vaccine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vaccine);
        }

        // POST: Vaccines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VaccineName,DosesRequired,DaysBetween,TotalDoseRecieved,TotalDoseLeft")] Vaccine vaccine)
        {
            if (ModelState.IsValid)
            {
                vaccine.TotalDoseRecieved = 0;
                vaccine.TotalDoseLeft = 0;
                _context.Add(vaccine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vaccine);
        }

        // GET: Vaccines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Get your distinct doses required as a Dictionary
            var dosesList = _context.Vaccine.OrderBy(x =>x.DosesRequired)              // Order your elements
                                   .Select(x => x.DosesRequired)                       // Select only the county property
                                   .Distinct()                                        // Get the distinct doses required           
                                   .ToList()                                          // Execute the query
                                   .Select(x => new KeyValuePair<string, string>(Convert.ToString(x), Convert.ToString(x))) // Map them to a collection of KVPs 
                                   .ToList();
            // Bind your list to a SelectList                      
            ViewBag.DosesListItem = new SelectList(dosesList, "Key", "Value");

            var vaccine = await _context.Vaccine.FindAsync(id);
            if (vaccine == null)
            {
                return NotFound();
            }
            return View(vaccine);
        }

        // POST: Vaccines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VaccineName,DosesRequired,DaysBetween,TotalDoseRecieved,TotalDoseLeft")] Vaccine vaccine)
        {
            if (id != vaccine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vacList = await _context.Vaccine.AsNoTracking().ToListAsync();
                    var vac = vacList.Find(x => x.Id == id);
                    vaccine.TotalDoseRecieved = vac.TotalDoseRecieved;
                    vaccine.TotalDoseLeft = vac.TotalDoseLeft;
                    _context.Update(vaccine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccineExists(vaccine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vaccine);
        }

        private bool VaccineExists(int id)
        {
            return _context.Vaccine.Any(e => e.Id == id);
        }
    }
}
