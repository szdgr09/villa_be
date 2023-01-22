using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaNumberRepository: IRepository<VillaNumber>
    {

        Task<VillaNumber> UpdateAsync(VillaNumber entity);

        
    }
}
