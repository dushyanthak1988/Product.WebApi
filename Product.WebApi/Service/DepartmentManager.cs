using Product.WebApi.Concrete;
using Product.WebApi.Data;
using Product.WebApi.Models;

namespace Product.WebApi.Service
{
    public class DepartmentManager : IDataRepository<Department>
    {
        readonly ApplicationDbContext _Context;

        public DepartmentManager(ApplicationDbContext context)
        {
            _Context = context;
        }

        public void Add(Department entity)
        {
            _Context.Departments.Add(entity);
        }

        public void Delete(Department entity)
        {
            _Context.Departments.Remove(entity);
            _Context.SaveChanges();
        }

        public Department Get(long id)
        {
            return _Context.Departments.FirstOrDefault(x => x.DepartmentId == id);
        }

        public IEnumerable<Department> GetAll()
        {
            return _Context.Departments.ToList();
        }

        public void Update(Department entity)
        {
            var dbEntity = _Context.Departments.SingleOrDefault(x => x.DepartmentId == entity.DepartmentId);

            dbEntity.DepartmentName = entity.DepartmentName;
            _Context.SaveChanges();


        }
    }
}
