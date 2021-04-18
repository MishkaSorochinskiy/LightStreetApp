using DAL;
using DAL.Entities;
using System.Linq;

namespace BLL.Services
{
    public class CameraService
    {
        private UnitOfWork uof { get; }
        public CameraService(UnitOfWork uof)
        {
            this.uof = uof;
        }

        public IQueryable<Camera> GetAll()
        {
            return uof.Repository<Camera>().Get();
        }
    }
}
