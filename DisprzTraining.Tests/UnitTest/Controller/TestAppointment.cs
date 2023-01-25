using System.Net;
using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.CustomException;
using DisprzTraining.Model;
using DisprzTraining.Model.Result;
using DisprzTraining.Tests.MockDatas;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace DisprzTraining.Tests.UnitTests.Controller
{
    public class TestAppointment
    {

        //Initial SetUp

        private readonly Mock<IAppointmentBL> MockServiceBL = new();
        private readonly Guid MockGuid = Guid.NewGuid();
        AppointmentsController systemUnderTest;
        public TestAppointment()
        {
            systemUnderTest = new AppointmentsController(MockServiceBL.Object);
        }



        //////////////              //Delete appointment tests //                 ////////////////

        [Fact]
        public void DeleteAppointment_Returns_204_NoContent_On_Succesful_Deletion()
        {
            //Arrange
            MockServiceBL.Setup(service => service.DeleteAppointment(It.IsAny<Guid>())).Returns(true);

            //Act
            var result = (NoContentResult)systemUnderTest.DeleteAppointment(MockGuid);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, (HttpStatusCode)result.StatusCode);
        }

        [Fact]
        public void DeleteAppointment_Returns_404_NotFound()
        {
            //Arrange
            MockServiceBL.Setup(service => service.DeleteAppointment(It.IsAny<Guid>())).Returns(false);
            //Act
            var result = (NotFoundObjectResult)systemUnderTest.DeleteAppointment(MockGuid);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.DataNotFound);
        }

        //////////////              //Delete appointment tests //                 ////////////////




        //////////////              // Get Appointment By Id //                 ////////////////
        [Fact]
        public void GetAppointmentById_Returns_200_On_Appointment_Found()
        {
            //Arrange
            var expectedResult = MockData.MockAppointmentByID;
            MockServiceBL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).Returns(expectedResult);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetappointmentById(MockGuid);

            //Assert
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
        }

        [Fact]
        public void GetAppointmentById_Returns_404_NotFount()
        {
            //Arrange
            MockServiceBL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).Returns(() => null);
            //Act
            var result = (NotFoundObjectResult)systemUnderTest.GetappointmentById(MockGuid);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.DataNotFound);
        }
        //////////////              // Get Appointment By Id //                 ////////////////





        //////////////              //Add new appointment //                 ////////////////

        [Fact]
        public void AddNewAppointment_Returns_201_Created_And_New_Appointment_Id_On_Successful_Creation()
        {
            //Arrange
            var expectedResult = new NewAppointmentId()
            {
                Id = MockGuid
            };
            MockServiceBL.Setup(service => service.AddAppointment(It.IsAny<AppointmentDTO>())).Returns(expectedResult);

            //Act
            var result = (CreatedResult)systemUnderTest.AddAppointment(MockData.MockAppointmentDTO);

            //Assert
            Assert.Equal(HttpStatusCode.Created, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
        }

        [Fact]
        public void AddNewAppointment_Returns_409_Conflict_Between_Different_Meetings()
        {
            //Arrange
            MockServiceBL.Setup(service => service.AddAppointment(It.IsAny<AppointmentDTO>())).Returns(() => null);

            //Act
            var result = (ConflictObjectResult)systemUnderTest.AddAppointment(MockData.MockAppointmentDTO);

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.ConflictResponse);
        }

        [Fact]
        public void AddNewAppointment_Returns_400_BadRequest_On_EndTime_LessThan_StartTime()
        {
            //Arrange  
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(4),
                appointmentEndTime = DateTime.Now.AddHours(3),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            MockServiceBL.Setup(service => service.AddAppointment(It.IsAny<AppointmentDTO>())).Throws(new InputErrorException(AppointmentErrorResponse.EndTimeLessThanStartTime));

            //Act
            var result = (BadRequestObjectResult)systemUnderTest.AddAppointment(MockAppointment);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.EndTimeLessThanStartTime);
        }


        [Fact]
        public void AddNewAppointment_Returns_400_BadRequest_On_Same_StartTime_And_EndTime()
        {
            //Arrange  
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now,
                appointmentEndTime = DateTime.Now,
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            MockServiceBL.Setup(service => service.AddAppointment(It.IsAny<AppointmentDTO>())).Throws(new InputErrorException(AppointmentErrorResponse.SameTiming));
            //Act
            var result = (BadRequestObjectResult)systemUnderTest.AddAppointment(MockAppointment);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.SameTiming);
        }

        [Fact]
        public void AddNewAppointment_Returns_400_BadRequest_On__PastTimings()
        {
            //Arrange  
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(-4),
                appointmentEndTime = DateTime.Now.AddHours(-3),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            MockServiceBL.Setup(service => service.AddAppointment(It.IsAny<AppointmentDTO>())).Throws(new InputErrorException(AppointmentErrorResponse.PastTiming));

            //Act
            var result = (BadRequestObjectResult)systemUnderTest.AddAppointment(MockAppointment);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.PastTiming);
        }

        //////////////              //Add new appointment //                 ////////////////




        //////////////              //update Existing Appointment //                 ////////////////

        [Fact]
        public void UpdateAppointment_Returns_204_NoContent_On_Succesful_Updation()
        {
            //Arrange
            MockServiceBL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Returns(true);

            //Act
            var result = (NoContentResult)systemUnderTest.UpdateAppointment(MockGuid, MockData.MockAppointmentDTO);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, (HttpStatusCode)result.StatusCode);
        }

        [Fact]
        public void UpdateAppointment_Returns_409_Conflict_Between_Different_Meetings()
        {
            //Arrange
            MockServiceBL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Returns(false);

            //Act
            var result = (ConflictObjectResult)systemUnderTest.UpdateAppointment(MockGuid, MockData.MockAppointmentDTO);

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.ConflictResponse);
        }

        [Fact]
        public void UpdateAppointment_Returns_404_Not_Found_For_Meeting_NotFound_In_List()
        {
            //Arrange
            MockServiceBL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Returns(() => null);

            //Act
            var result = (NotFoundObjectResult)systemUnderTest.UpdateAppointment(MockGuid, MockData.MockAppointmentDTO);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.DataNotFound);
        }

        [Fact]
        public void UpdateExistingAppointment_Returns_400_BadRequest_On_EndTime_LessThan_StartTime()
        {
            //Arrange  
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(4),
                appointmentEndTime = DateTime.Now.AddHours(3),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            MockServiceBL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Throws(new InputErrorException(AppointmentErrorResponse.EndTimeLessThanStartTime));

            //Act
            var result = (BadRequestObjectResult)systemUnderTest.UpdateAppointment(MockGuid, MockAppointment);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.EndTimeLessThanStartTime);
        }

        [Fact]
        public void UpdateExistingAppointment_Returns_400_BadRequest_On_Same_StartTime_And_EndTime()
        {
            //Arrange  
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now,
                appointmentEndTime = DateTime.Now,
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            MockServiceBL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Throws(new InputErrorException(AppointmentErrorResponse.SameTiming));
            
            //Act
            var result = (BadRequestObjectResult)systemUnderTest.UpdateAppointment(MockGuid, MockAppointment);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.SameTiming);
        }

        [Fact]
        public void UpdateExistingAppointment_Returns_400_BadRequest_On_PastTimings()
        {
            //Arrange  
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(-4),
                appointmentEndTime = DateTime.Now.AddHours(-3),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            MockServiceBL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Throws(new InputErrorException(AppointmentErrorResponse.PastTiming));

            //Act
            var result = (BadRequestObjectResult)systemUnderTest.UpdateAppointment(MockGuid, MockAppointment);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, AppointmentErrorResponse.PastTiming);
        }

        //////////////              //update Existing Appointment //                 ////////////////






        //////////////              //Get All Appointments //                 ////////////////

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_Offset0_and_FetchCount5()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = true, appointments = MockData.MockAppointmentList.Skip(0).Take(5).ToList() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, null, null)).Returns(expectedResult);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetAllAppointments(0, 5, null, null, null);
            PaginatedAppointments? getValue = (PaginatedAppointments?)result.Value;

            //Assert
            Assert.IsType<PaginatedAppointments>(result.Value);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
            Assert.Equal(expectedResult.appointments.Count(), getValue?.appointments.Count());
            Assert.Equal(expectedResult.isTruncated, getValue?.isTruncated);
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_OffSet5_and_FetchCount5()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentList.Skip(5).Take(5).ToList() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, null, null)).Returns(expectedResult);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetAllAppointments(5, 5, null, null, null);
            PaginatedAppointments? getValue = (PaginatedAppointments?)result.Value;

            //Assert
            Assert.IsType<PaginatedAppointments>(result.Value);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
            Assert.Equal(expectedResult.appointments.Count(), getValue?.appointments.Count());
            Assert.Equal(expectedResult.isTruncated, getValue?.isTruncated);
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_Start_Date_And_End_Date_And_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDateAndTitle.Skip(0).Take(5).ToList() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns(expectedResult);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), new DateTime(2023, 01, 08), "worldCup Discussion");
            PaginatedAppointments? getValue = (PaginatedAppointments?)result.Value;

            //Assert
            Assert.IsType<PaginatedAppointments>(result.Value);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
            Assert.Equal(expectedResult.appointments.Count(), getValue?.appointments.Count());
            Assert.Equal(expectedResult.isTruncated, getValue?.isTruncated);
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Empty_Paginated_Result_With_Start_Date_And_End_Date_And_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDateAndTitle.Skip(5).Take(5).ToList() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns(expectedResult);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), new DateTime(2023, 01, 08), "worldCup Discussion");
            PaginatedAppointments? getValue = (PaginatedAppointments?)result.Value;

            //Assert
            Assert.IsType<PaginatedAppointments>(result.Value);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
            Assert.Equal(expectedResult.appointments.Count(), getValue?.appointments.Count());
            Assert.Equal(expectedResult.isTruncated, getValue?.isTruncated);
        }
        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_Start_Date_And_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDateAndTitle.Skip(0).Take(5).ToList() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), null, It.IsAny<string>())).Returns(expectedResult);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), null, "worldCup Discussion");
            PaginatedAppointments? getValue = (PaginatedAppointments?)result.Value;

            //Assert
            Assert.IsType<PaginatedAppointments>(result.Value);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
            Assert.Equal(expectedResult.appointments.Count(), getValue?.appointments.Count());
            Assert.Equal(expectedResult.isTruncated, getValue?.isTruncated);
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_End_Date_And_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDateAndTitle.Skip(0).Take(5).ToList() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, It.IsAny<DateTime>(), It.IsAny<string>())).Returns(expectedResult);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetAllAppointments(0, 5, null, new DateTime(2023, 01, 08), "worldCup Discussion");
            PaginatedAppointments? getValue = (PaginatedAppointments?)result.Value;

            //Assert
            Assert.IsType<PaginatedAppointments>(result.Value);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
            Assert.Equal(expectedResult.appointments.Count(), getValue?.appointments.Count());
            Assert.Equal(expectedResult.isTruncated, getValue?.isTruncated);
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_StartDate()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDate.Skip(0).Take(5).ToList() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), null, null)).Returns(expectedResult);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), null, null);
            PaginatedAppointments? getValue = (PaginatedAppointments?)result.Value;

            //Assert
            Assert.IsType<PaginatedAppointments>(result.Value);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
            Assert.Equal(expectedResult.appointments.Count(), getValue?.appointments.Count());
            Assert.Equal(expectedResult.isTruncated, getValue?.isTruncated);
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_EndDate()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDate.Skip(0).Take(5).ToList() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, It.IsAny<DateTime>(), null)).Returns(expectedResult);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetAllAppointments(0, 5, null, new DateTime(2023, 01, 08), null);
            PaginatedAppointments? getValue = (PaginatedAppointments?)result.Value;

            //Assert
            Assert.IsType<PaginatedAppointments>(result.Value);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
            Assert.Equal(expectedResult.appointments.Count(), getValue?.appointments.Count());
            Assert.Equal(expectedResult.isTruncated, getValue?.isTruncated);
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_Start_Date_And_End_Date()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDate.Skip(0).Take(5).ToList() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), null)).Returns(expectedResult);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), new DateTime(2023, 01, 08), null);
            PaginatedAppointments? getValue = (PaginatedAppointments?)result.Value;

            //Assert
            Assert.IsType<PaginatedAppointments>(result.Value);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
            Assert.Equal(expectedResult.appointments.Count(), getValue?.appointments.Count());
            Assert.Equal(expectedResult.isTruncated, getValue?.isTruncated);
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_Search_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithTitle.Skip(0).Take(5).ToList() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, null, It.IsAny<string>())).Returns(expectedResult);

            //Act
            var result = (OkObjectResult)systemUnderTest.GetAllAppointments(0, 5, null, null, "worldCup Discussion");
            PaginatedAppointments? getValue = (PaginatedAppointments?)result.Value;

            //Assert
            Assert.IsType<PaginatedAppointments>(result.Value);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode?)result.StatusCode);
            Assert.Equal(result.Value, expectedResult);
            Assert.Equal(expectedResult.appointments.Count(), getValue?.appointments.Count());
            Assert.Equal(expectedResult.isTruncated, getValue?.isTruncated);
        }

        //////////////              //Get All Appointments //                 ////////////////


    }
}