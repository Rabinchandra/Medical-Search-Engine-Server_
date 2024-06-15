using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server_.Models.DTOModel;
using Server_.Models.EntityModel;

namespace Server_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        public readonly MedicalSearchEngineContext _context;

        public AppointmentController()
        {
            _context = new MedicalSearchEngineContext();
        }

        // Get all the appointments
        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            return Ok(_context.Appointments.ToList());
        }

        // Add an appointment
        [HttpPost]
        public IActionResult AddAppointment([FromBody] AppointmentDTO app)
        {

            try
            {
                var newAppointment = new Appointment()
                {
                    AppointmentDate = app.AppointmentDate,
                    AppointmentTime = app.AppointmentTime,
                    DoctorId = app.DoctorId,
                    PatientId = app.PatientId,
                    Purpose = app.Purpose
                };
                _context.Appointments.Add(newAppointment);
                _context.SaveChanges();

                return CreatedAtAction(nameof(AddAppointment), newAppointment);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong while trying to add a new appointment!");
            }

        }

        [HttpGet("patient/{id}")]
        // Get appointment for a patient
        public IActionResult GetAppointmentForPatient(string id)
        {
            try
            {
                var appointments = _context.Appointments.Where(a => a.PatientId == id).ToList();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("doctor/{id}")]
        // Get appointment for a patient
        public IActionResult GetAppointmentForDoctor(string id)
        {
            try
            {
                var appointments = _context.Appointments
                                                    .Where(a => a.DoctorId == id && (a.Status == "accepted" || a.Status == "completed"))
                                                    .ToList();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
