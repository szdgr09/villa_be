using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {

        Task<Villa> UpdateAsync(Villa entity);

        
    }
}
