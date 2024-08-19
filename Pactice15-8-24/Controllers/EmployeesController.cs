using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pactice15_8_24.Models;
using Pactice15_8_24.Models.ViewModel;

namespace Pactice15_8_24.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;

        public EmployeesController(IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }



        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var salaries = await _context.Employees.Select(e => e.Salary).ToListAsync();

            var model = new AggregateEmployeeVM
            {
                MinValue = salaries.Any() ? salaries.Min() : 0,
                MaxValue = salaries.Any() ? salaries.Max() : 0,
                SumValue = salaries.Any() ? salaries.Sum() : 0,
                AvgValue = (decimal)(salaries.Any() ? salaries.Average() : 0),
                GroupByResult = await _context.Employees
                    .GroupBy(e => e.EmployeeId)
                    .Select(c => new GroupByViewModel
                    {
                        EmployeeId = c.Key,
                        MinValue = c.Min(e => e.Salary),
                        MaxValue = c.Max(e => e.Salary),
                        SumValue = c.Sum(e => e.Salary),
                        AvgValue = (decimal)c.Average(e => e.Salary),
                        Count = c.Count()
                    }).ToListAsync(),
                Employees = await _context.Employees
                    .Include(e => e.Experiences)
                    .Select(e => new EmployeeVM
                    {
                        EmployeeId = e.EmployeeId,
                        Name = e.Name,
                        IsActive = e.IsActive,
                        JoinDate = e.JoinDate,
                        Salary = e.Salary,
                        ImageUrl = e.ImageUrl,
                        Experiences = e.Experiences.Select(exp => new ExperienceViewModel
                        {
                            Title = exp.Title,
                            Duration = exp.Duration,
                        }).ToList(),
                    }).ToListAsync()
            };

            return View(model);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Name,IsActive,JoinDate,ImageName,ImageUrl,Salary")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Name,IsActive,JoinDate,ImageName,ImageUrl,Salary")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
