using DAL;
using DAL.Entities;
using System.Linq;

namespace BLL.Services
{
    public class LampTypeService
    {
        private UnitOfWork uof { get; }
        public LampTypeService(UnitOfWork uof)
        {
            this.uof = uof;
        }

        public IQueryable<LampType> GetAll()
        {
            return uof.Repository<LampType>().Get();
        }
    }
}
