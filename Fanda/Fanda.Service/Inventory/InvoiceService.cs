using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Common.Utility;
using Fanda.Data.Context;
using Fanda.Data.Inventory;
using Fanda.ViewModel.Inventory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service.Inventory
{
    public interface IInvoiceService
    {
        Task<List<InvoiceViewModel>> GetAllAsync(Guid orgId);

        Task<InvoiceViewModel> GetByIdAsync(Guid invoiceId);

        Task<InvoiceViewModel> SaveAsync(Guid orgId, InvoiceViewModel invoiceVM);

        Task<bool> DeleteAsync(Guid invoiceId);

        string ErrorMessage { get; }
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public InvoiceService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<InvoiceViewModel>> GetAllAsync(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var invoices = await _context.Invoices
                .Where(p => p.OrgId == p.OrgId)
                .AsNoTracking()
                //.ProjectTo<InvoiceViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return _mapper.Map<List<InvoiceViewModel>>(invoices);
        }

        public async Task<InvoiceViewModel> GetByIdAsync(Guid invoiceId)
        {
            var invoice = await _context.Invoices
                .ProjectTo<InvoiceViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(inv => inv.InvoiceId == invoiceId);

            if (invoice != null)
                return invoice;

            throw new KeyNotFoundException("Invoice not found");
        }

        public async Task<InvoiceViewModel> SaveAsync(Guid orgId, InvoiceViewModel invoiceVM)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var invoice = _mapper.Map<Invoice>(invoiceVM);
            if (invoice.InvoiceId == Guid.Empty)
            {
                invoice.OrgId = orgId;
                invoice.DateCreated = DateTime.Now;
                invoice.DateModified = null;
                _context.Invoices.Add(invoice);
            }
            else
            {
                var dbInvoice = await _context.Invoices
                    .Where(i => i.InvoiceId == invoice.InvoiceId)
                    .Include(i => i.InvoiceItems).ThenInclude(ii => ii.Stock)
                    .SingleOrDefaultAsync();
                if (dbInvoice == null)
                {
                    invoice.DateCreated = DateTime.Now;
                    invoice.DateModified = null;
                    _context.Invoices.Add(invoice);
                }
                else
                {
                    invoice.DateModified = DateTime.Now;
                    // delete all linet items that no longer exists
                    foreach (var dbLineItem in dbInvoice.InvoiceItems)
                    {
                        if (invoice.InvoiceItems.All(ii => ii.InvItemId != dbLineItem.InvItemId))
                            _context.Set<InvoiceItem>().Remove(dbLineItem);
                    }
                    // copy current (incoming) values to db
                    _context.Entry(dbInvoice).CurrentValues.SetValues(invoice);
                    var itemPairs = from curr in invoice.InvoiceItems//.Select(pi => pi.IngredientProduct)
                                    join db in dbInvoice.InvoiceItems//.Select(pi => pi.IngredientProduct)
                                      on curr.InvItemId equals db.InvItemId into grp
                                    from db in grp.DefaultIfEmpty()
                                    select new { curr, db };
                    foreach (var pair in itemPairs)
                    {
                        if (pair.db != null)
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                        else
                            _context.Set<InvoiceItem>().Add(pair.curr);
                    }
                }
            }
            await _context.SaveChangesAsync();
            invoiceVM = _mapper.Map<InvoiceViewModel>(invoice);
            return invoiceVM;
        }

        public async Task<bool> DeleteAsync(Guid invoiceId)
        {
            var invoice = await _context.Invoices
                .FindAsync(invoiceId);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Invoice not found");
        }
    }
}