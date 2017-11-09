using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraderData;
using TraderData.Models.FileImportModels;

namespace TraderServices
{
    class FileImportService : IFileImport
    {
        private readonly ApplicationDbContext _context;

        public FileImportService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task Add(FileImport fileImport)
        {
            _context.Add(fileImport);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var fileImport = await getByIdAsync(id);
            _context.FileImport.Remove(fileImport);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Edit(FileImport fileImport)
        {
            try
            {
                _context.Update(fileImport);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        public async Task<List<FileImport>> getAllAsync()
        { 
            return await _context.FileImport.Include(f => f.Exchange)
                .OrderBy(x => x.ImportDate)
                .ToListAsync();
        }

        public async Task<List<FileImport>> getAllByUser(string userId)
        {
            var imports = await getAllAsync();
            return imports.Where(x => x.UserID == userId).ToList();

        }

        public async Task<FileImport> getByIdAsync(int id)
        {
            return await _context.FileImport
                .Include(f => f.Exchange)
                .SingleOrDefaultAsync(m => m.FileImportId == id);
        }

        public bool FileImportExists(int id)
        {
            return _context.FileImport.Any(e => e.FileImportId == id);
        }
    }
}
