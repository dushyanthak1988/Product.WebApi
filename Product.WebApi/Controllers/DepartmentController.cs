using Microsoft.AspNetCore.Mvc;
using Product.WebApi.Concrete;
using Product.WebApi.Models;

namespace Product.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDataRepository<Department> _dataRepository;

        public DepartmentController(IDataRepository<Department> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [HttpGet(Name = "GetDepartmentAll")]
        public IActionResult Get()
        {
            IEnumerable<Department> departments = _dataRepository.GetAll();
            return Ok(departments);
        }

        [HttpGet("{id}", Name = "GetDepartmentByid")]
        public IActionResult Get(long id)
        {
            Department obj = _dataRepository.Get(id);
            if (obj == null)
            {
                return NotFound("The Department record couldn't be found.");
            }
            return Ok(obj);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Department department)
        {
            if (department == null)
            {
                return BadRequest("Department is null.");
            }
            _dataRepository.Add(department);
            return CreatedAtRoute(
                  "Get",
                  new { Id = department.DepartmentId },
                  department);
        }
        // PUT: api/Department/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Department department)
        {
            if (department == null)
            {
                return BadRequest("Department is null.");
            }
            Department departmentToUpdate = _dataRepository.Get(id);
            if (departmentToUpdate == null)
            {
                return NotFound("The Department record couldn't be found.");
            }
            _dataRepository.Update(department);
            return NoContent();
        }
        // DELETE: api/Department/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Department department = _dataRepository.Get(id);
            if (department == null)
            {
                return NotFound("The Department record couldn't be found.");
            }
            _dataRepository.Delete(department);
            return NoContent();
        }
    }
}
