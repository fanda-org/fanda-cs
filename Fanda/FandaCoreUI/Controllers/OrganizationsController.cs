using Fanda.Dto;
using Fanda.Service;
using Fanda.Service.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<IActionResult> Index()
        {
            var list = await ((IListService<OrgListDto>)_service)
                .GetAll()
                .ToListAsync();
            return View(list);
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var organization = await _context.Organizations
            //    .FirstOrDefaultAsync(m => m.OrgId == id);
            OrganizationDto org = await _service.GetByIdAsync(id);
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
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var organization = await _context.Organizations.FindAsync(id);
            OrganizationDto org = await _service.GetByIdAsync(id);
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
        public async Task<IActionResult> Edit(Guid id,
            [Bind("OrgId,OrgCode,OrgName,Description,RegdNum,PAN,TAN,GSTIN,Active,DateCreated,DateModified")] OrganizationDto dto)
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
                    if (!await OrganizationExists(dto.Id))
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
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var organization = await _context.Organizations
            //    .FirstOrDefaultAsync(m => m.OrgId == id);
            OrganizationDto org = await _service.GetByIdAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            return View(org);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //var organization = await _context.Organizations.FindAsync(id);
            //_context.Organizations.Remove(organization);
            //await _context.SaveChangesAsync();
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OrganizationExists(Guid id) =>
            await _service.ExistsAsync(new Shared.BaseDuplicate { Field = Shared.DuplicateField.Id, Id = id });
    }
}
