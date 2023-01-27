using System.ComponentModel.DataAnnotations;
using DisprzTraining.Business;
using DisprzTraining.CustomException;
using DisprzTraining.Model;
using DisprzTraining.Model.Result;
using Microsoft.AspNetCore.Mvc;

namespace DisprzTraining.Controllers
{
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentBL _appointmentBL;

        public AppointmentsController(IAppointmentBL appointmentBL)
        {
            _appointmentBL = appointmentBL;
        }

        /// <summary>
        /// Fetch All Appointments with Date/Title with offset and fetchCount
        /// </summary>
        ///<remarks>
        /// example
        ///
        ///      date : "2023-01-06"(yyyy-mm-dd)
        ///      title: "standUp"
        ///
        /// </remarks>
        /// <response code="200">Returns list of appointment or empty list if no appointments found</response>
        //- GET /api/appointments/
        [HttpGet, Route("v1/api/appointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedAppointments))]
        public IActionResult GetAllAppointments([FromQuery] GetAppointmentQueryParameters inputGetData)
        {
            var appointments = _appointmentBL.GetAllAppointments(inputGetData.offSet, 
                                                                inputGetData.fetchCount, 
                                                                inputGetData.startDate, 
                                                                inputGetData.endDate, 
                                                                inputGetData.searchTitle);
            return Ok(appointments);
        }


        /// <summary>
        /// Fetch Appointment By Id
        /// </summary>
        ///<remarks>
        /// example
        ///
        ///     Id: "2ef43h26-4524-5245-g56a-5d552v96h1f6"
        ///
        /// </remarks>
        /// <response code="200">Returns an appointment</response>
        /// <response code="404">Appointment is not found</response>
        //get appointment by Id
        [HttpGet, Route("v1/api/appointments/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Appointment))]
        public IActionResult GetappointmentById([FromRoute] Guid Id)
        {
            var appointmentById = _appointmentBL.GetAppointmentById(Id);
            return appointmentById != null ? Ok(appointmentById) : NotFound(AppointmentErrorResponse.DataNotFound);
        }


        /// <summary>
        /// Add new Appointment
        /// </summary>
        /// <remarks>
        /// example:
        ///
        ///     {
        ///        "appointmentStartTime": "2023-01-06T20:43:33.005Z",
        ///        "appointmentEndTime": "2023-01-06T20:44:43.005Z",
        ///        "appointmentTitle": "string",
        ///        "appointmentDescription":"string"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created appointmentId</response>
        /// <response code="400">Bad request if starttime greater than end time</response>
        /// <response code="409">Conflict Occured between meetings</response>
        //- POST /api/appointments
        [HttpPost, Route("v1/api/appointments")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NewAppointmentId))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public IActionResult AddAppointment([FromBody] AppointmentDTO newAppointment)
        {
            try
            {
                var createdAppointmentId = _appointmentBL.AddAppointment(newAppointment);
                return (createdAppointmentId != null) ? Created("~v1/api/apiappointments", createdAppointmentId) : Conflict(AppointmentErrorResponse.ConflictResponse);
            }
            catch (InputTimeErrorException ex)
            {
                return BadRequest(ex.InputTimeError);
            }
        }


        /// <summary>
        /// Delete an appointment
        /// </summary>
        /// <remarks>
        /// example:
        ///
        ///        Id: "2ef43h26-4524-5245-g56a-5d552v96h1f6"
        ///
        /// </remarks>
        /// <response code="204">Deletes an appointment successfully</response>
        /// <response code="404">Appointment is not found</response>
        // DELETE /api/appointments/{Id}
        [HttpDelete, Route("v1/api/appointments/{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public IActionResult DeleteAppointment([FromRoute] Guid Id)
        {
            var isDeleted = _appointmentBL.DeleteAppointment(Id);
            return (isDeleted) ? NoContent() : NotFound(AppointmentErrorResponse.DataNotFound);
        }


        /// <summary>
        /// Update an Existing appointment
        /// </summary>
        /// <remarks>
        /// example:
        ///
        ///     Id: "2ef43h26-4524-5245-g56a-5d552v96h1f6",
        ///     {
        ///        "appointmentStartTime": "2023-01-06T20:43:33.005Z",
        ///        "appointmentEndTime": "2023-01-06T20:44:43.005Z",
        ///        "appointmentTitle": "string",
        ///        "appointmentDescription":"string"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">No Content if appointment updated</response>
        /// <response code="400">Bad request if starttime greater than end time</response>
        /// <response code="409">Conflict Occured between meetings</response>
        /// <response code="404">Appointment is not found</response>
        [HttpPut, Route("v1/api/appointments/{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public IActionResult UpdateAppointment([FromRoute] Guid Id, [FromBody] AppointmentDTO updateAppointment)
        {
            try
            {
                bool? noConflict = _appointmentBL.UpdateAppointment(Id, updateAppointment);
                return (noConflict != null) ? (noConflict == true ? NoContent() : Conflict(AppointmentErrorResponse.ConflictResponse)) : NotFound(AppointmentErrorResponse.DataNotFound);
            }
            catch (InputTimeErrorException ex)
            {
                return BadRequest(ex.InputTimeError);
            }
        }
    }
}
