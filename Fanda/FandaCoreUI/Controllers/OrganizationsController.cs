using Fanda.Dto;
using Fanda.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fanda.Controllers
{
    public class OrganizationsController : Controller
    {
        //private readonly FandaContext _context;
        private readonly IOrganizationService _service;
        public OrganizationsController(IOrganizationService service /*FandaContext context*/)
        {
            //_context = context;
            _service = service;
        }

        // GET: Organizations
        public IActionResult Index() => View(_service.GetAll() /*await _context.Organizations.ToListAsync()*/);

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var organization = await _context.Organizations
            //    .FirstOrDefaultAsync(m => m.OrgId == id);
            var org = await _service.GetByIdAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            return View(org);
        }

        // GET: Organizations/Create
        public IActionResult Create() => View();

        // POST: Organizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrgId,OrgCode,OrgName,Description,RegdNum,PAN,TAN,GSTIN,Active,DateCreated,DateModified")] OrganizationDto dto)
        {
            if (ModelState.IsValid)
            {
                //organization.OrgId = Guid.NewGuid();
                //_context.Add(organization);
                //await _context.SaveChangesAsync();
                await _service.SaveAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var organization = await _context.Organizations.FindAsync(id);
            var org = await _service.GetByIdAsync(id);
            if (org == null)
            {
                return NotFound();
            }
            return View(org);
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrgId,OrgCode,OrgName,Description,RegdNum,PAN,TAN,GSTIN,Active,DateCreated,DateModified")] OrganizationDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(organization);
                    //await _context.SaveChangesAsync();
                    await _service.SaveAsync(dto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(dto.Id))
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
            return View(dto);
        }

        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var organization = await _context.Organizations
            //    .FirstOrDefaultAsync(m => m.OrgId == id);
            var org = await _service.GetByIdAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            return View(org);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //var organization = await _context.Organizations.FindAsync(id);
            //_context.Organizations.Remove(organization);
            //await _context.SaveChangesAsync();
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(string id) => _service.ExistsById(id);
    }
}
