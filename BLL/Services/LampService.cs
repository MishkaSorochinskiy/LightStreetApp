using DAL;
using DAL.Entities;
using System.Linq;

namespace BLL.Services
{
    public class LampService
    {
        private UnitOfWork uof { get; }
        public LampService(UnitOfWork uof)
        {
            this.uof = uof;
        }

        public IQueryable<Lamp> GetAll()
        {
            return uof.Repository<Lamp>().Get();
        }
    }
}
