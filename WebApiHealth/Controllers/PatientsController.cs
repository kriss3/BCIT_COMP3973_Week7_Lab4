using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiHealth.Data;
using WebApiHealth.Models;

namespace WebApiHealth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("HealthPolicy")]
    public class PatientsController : ControllerBase
    {
        private readonly HealthContext _context;

        public PatientsController(HealthContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet]
        public IEnumerable<Patient> GetPatients()
        {
            return _context.Patients.Include(p => p.Ailments).Include(p=>p.Medications); ;
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var patient = await _context.Patients.FindAsync(id);  //No include with Async | see if Async method works;

            var patient = await _context.Patients
                                .Include(i => i.Ailments)
                                .Include(m => m.Medications)
                                .FirstOrDefaultAsync(i => i.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // GET api/patients/3/medication
        [HttpGet("{id:int}/medication")]
        public async Task<IActionResult> GetMedications(int id)
        {
            var patient = await _context.Patients
              .Include(m => m.Medications)
              .FirstOrDefaultAsync(i => i.PatientId == id);

            if (patient == null)
                return NotFound();

            return Ok(patient.Medications);
        }


        // PUT: api/Patients/5   => for updating
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient([FromRoute] int id, [FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PatientId)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Patients  => does an insert | fromBody, fromRoute, fromQuery(? in browswer), 
        // if not spefified default: from body, possibly
        [HttpPost]
        public async Task<IActionResult> PostPatient([FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.PatientId }, patient);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient([FromRoute] int id)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            try
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {
                throw;
            }
            return Ok(patient);
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}