using System.Collections.Generic;
using System.Threading.Tasks;
using TraderData.Models.FileImportModels;

namespace TraderData
{
    public interface IFileImport
    {
        Task<List<FileImport>> getAllAsync();
        Task Add(FileImport fileImport);
        Task<bool> Edit(FileImport fileImport);
        Task Delete(int id);
        Task<List<FileImport>> getAllByUser(string userId);
        Task<FileImport> getByIdAsync(int id);
        bool FileImportExists(int id);
    }
}
