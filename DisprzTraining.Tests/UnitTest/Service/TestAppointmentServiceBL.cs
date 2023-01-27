using DisprzTraining.Business;
using DisprzTraining.CustomException;
using DisprzTraining.DataAccess;
using DisprzTraining.Model;
using DisprzTraining.Model.Result;
using DisprzTraining.Tests.MockDatas;
using Moq;

namespace DisprzTraining.Tests.UnitTests.Service
{
    public class TestAppointmentServiceBL
    {

        //Initial SetUp

        private readonly Mock<IAppointmentDAL> MockServiceDAL = new();
        private readonly Guid MockGuid = Guid.NewGuid();

        AppointmentBL serviceUnderTest;
        public TestAppointmentServiceBL()
        {
            serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);
        }





        //////////////              //Delete appointment tests //                 ////////////////

        [Fact]
        public void DeleteAppointment_Returns_True_On_Succesful_Deletion()
        {
            //Arrange
            var expectedResult = true;
            MockServiceDAL.Setup(service => service.DeleteAppointment(It.IsAny<Guid>())).Returns(expectedResult);
            //Act
            var result = serviceUnderTest.DeleteAppointment(MockGuid);
            //Assert
            Assert.Equal(expectedResult, result);
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void DeleteAppointment_Returns_False_On_Not_Found()
        {
            //Arrange
            var expectedResult = false;
            MockServiceDAL.Setup(service => service.DeleteAppointment(It.IsAny<Guid>())).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.DeleteAppointment(MockGuid);
            //Assert
            Assert.Equal(expectedResult, result);
            Assert.IsType<bool>(result);
        }

        //////////////              //Delete appointment tests //                 ////////////////




        //////////////              // Get Appointment By Id //                 ////////////////

        [Fact]
        public void GetByAppointmentId_Returns_Appointment()
        {
            //Arrange
            var appointmentId = Guid.Parse("37981518-40f1-4580-946b-d47eb379453e");
            var expectedResult = MockData.MockAppointmentByID;
            MockServiceDAL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).Returns(expectedResult);
            //Act
            var result = serviceUnderTest.GetAppointmentById(appointmentId);
            //Assert
            Assert.Equal(expectedResult, result);
            Assert.IsType<Appointment>(result);
        }

        [Fact]
        public void GetByAppointmentId_Returns_Null_On_No_Appointment()
        {
            //Arrange
            Appointment? expectedResult = null;
            MockServiceDAL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.GetAppointmentById(MockGuid);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        //////////////              // Get Appointment By Id //                 ////////////////




        //////////////              //Add new appointment //                 ////////////////

        [Fact]
        public void AddAppointment_Returns_BadRequest_On_EndTime_Less_Than_StartTime()
        {
            //Arrange
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(5),
                appointmentEndTime = DateTime.Now.AddHours(3),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };
            //Act

            //Assert
            var exception = Assert.Throws<InputTimeErrorException>(() => serviceUnderTest.AddAppointment(mockAppointment));
            Assert.Equal(AppointmentErrorResponse.EndTimeLessThanStartTime, exception.InputTimeError);
        }

        [Fact]
        public void AddAppointment_Returns_BadRequest_On_Input_Not_Within_Range()
        {
            //Arrange
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(5),
                appointmentEndTime = DateTime.Now.AddHours(18),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };
            //Act

            //Assert
            var exception = Assert.Throws<InputTimeErrorException>(() => serviceUnderTest.AddAppointment(mockAppointment));
            Assert.Equal(AppointmentErrorResponse.TimeRange, exception.InputTimeError);
        }

        [Fact]
        public void AddAppointment_Returns_BadRequest_On_Same_Input_Time()
        {
            //Arrange
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 10, 17, 10, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 17, 10, 10, 10),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };
            //Act

            //Assert
            var exception = Assert.Throws<InputTimeErrorException>(() => serviceUnderTest.AddAppointment(mockAppointment));
            Assert.Equal(AppointmentErrorResponse.SameTiming, exception.InputTimeError);
        }

        [Fact]
        public void AddAppointment_Returns_BadRequest_On_Past_Timing()
        {
            //Arrange
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 01, 17, 10, 10, 10),
                appointmentEndTime = new DateTime(2023, 01, 30, 10, 10, 10),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };
            //Act

            //Assert
            var exception = Assert.Throws<InputTimeErrorException>(() => serviceUnderTest.AddAppointment(mockAppointment));
            Assert.Equal(AppointmentErrorResponse.PastTiming, exception.InputTimeError);
        }

        [Fact]
        public void AddAppointment_Returns_NewId_On_Succesful_Creation()
        {
            var expectedResult = new NewAppointmentId()
            {
                Id = MockGuid
            };
            //Arrange
            MockServiceDAL.Setup(service => service.AddAppointment(It.IsAny<AppointmentDTO>())).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.AddAppointment(MockData.MockAppointmentDTO);
            //Assert
            Assert.IsType<NewAppointmentId>(result);
            Assert.Equal(expectedResult.Id, result.Id);
        }

        [Fact]
        public void AddAppointment_Returns_Null_On_Conflict_Between_Appointments()
        {
            //Arrange
            MockServiceDAL.Setup(service => service.AddAppointment(It.IsAny<AppointmentDTO>())).Returns(() => null);

            //Act
            var result = serviceUnderTest.AddAppointment(MockData.MockAppointmentDTO);
            //Assert
            Assert.Null(result);
        }

        //////////////              //Add new appointment //                 ////////////////






        //////////////              //update Existing Appointment //                 ////////////////
        [Fact]
        public void UpdateAppointment_Returns_True_On_Succesful_Updation()
        {
            //Arrange
            var expectedResult = true;
            MockServiceDAL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).Returns(MockData.MockAppointmentByID);
            MockServiceDAL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.UpdateAppointment(MockGuid, MockData.MockAppointmentDTO);

            //Assert
            Assert.True(result);
        }


        [Fact]
        public void UpdateAppointment_Returns_False_On_Conflict_Between_Appointments()
        {
            //Arrange
            var expectedResult = false;
            MockServiceDAL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).Returns(MockData.MockAppointmentByID);
            MockServiceDAL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.UpdateAppointment(MockGuid, MockData.MockAppointmentDTO);

            //Assert
            Assert.True(result == expectedResult);
        }

        [Fact]
        public void UpdateAppointment_Returns_Null_On_No_Appointment_Found()
        {
            //Arrange  
            MockServiceDAL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Returns(() => null);

            //Act
            var result = serviceUnderTest.UpdateAppointment(MockGuid, MockData.MockAppointmentDTO);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateAppointment_Returns_BadRequest_On_EndTime_Less_Than_StartTime()
        {
            //Arrange
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(5),
                appointmentEndTime = DateTime.Now.AddHours(3),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };
            //Act

            //Assert
            var exception = Assert.Throws<InputTimeErrorException>(() => serviceUnderTest.UpdateAppointment(MockGuid, mockAppointment));
            Assert.Equal(AppointmentErrorResponse.EndTimeLessThanStartTime, exception.InputTimeError);
        }

        [Fact]
        public void UpdateAppointment_Returns_BadRequest_On_Input_Time_Not_Within_Range()
        {
            //Arrange
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(5),
                appointmentEndTime = DateTime.Now.AddHours(18),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };
            //Act

            //Assert
            var exception = Assert.Throws<InputTimeErrorException>(() => serviceUnderTest.UpdateAppointment(MockGuid, mockAppointment));
            Assert.Equal(AppointmentErrorResponse.TimeRange, exception.InputTimeError);
        }

        [Fact]
        public void updateAppointment_Returns_BadRequest_On_Same_Input_Time()
        {
            //Arrange
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 10, 17, 10, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 17, 10, 10, 10),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };
            //Act

            //Assert
            var exception = Assert.Throws<InputTimeErrorException>(() => serviceUnderTest.UpdateAppointment(MockGuid, mockAppointment));
            Assert.Equal(AppointmentErrorResponse.SameTiming, exception.InputTimeError);
        }

        [Fact]
        public void updateAppointment_Returns_BadRequest_On_Past_Timing()
        {
            //Arrange
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 01, 17, 10, 10, 10),
                appointmentEndTime = new DateTime(2023, 01, 30, 10, 10, 10),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };
            //Act

            //Assert
            var exception = Assert.Throws<InputTimeErrorException>(() => serviceUnderTest.UpdateAppointment(MockGuid, mockAppointment));
            Assert.Equal(AppointmentErrorResponse.PastTiming, exception.InputTimeError);
        }

        //////////////              //update Existing Appointment //                 ////////////////





        //////////////              //Get All Appointments //                        ////////////////

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_Offset0_and_FetchCount5()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = true, appointments = MockData.MockAppointmentList.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, null, null)).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, null, null, null);
            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(result, expectedResult);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_OffSet5_and_FetchCount5()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentList.Skip(5).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, null, null)).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.GetAllAppointments(5, 5, null, null, null);
            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(result, expectedResult);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_Start_Date_And_End_Date_And_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDateAndTitle.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), new DateTime(2023, 01, 06), "worldCup Discussion");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(result, expectedResult);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Empty_Paginated_Result_With_Start_Date_And_End_Date_And_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDateAndTitle.Skip(5).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), new DateTime(2023, 01, 06), "worldCup Discussion");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(result, expectedResult);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_Start_Date_And_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDateAndTitle.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), null, It.IsAny<string>())).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), null, "worldCup Discussion");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(result, expectedResult);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_End_Date_And_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDateAndTitle.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, It.IsAny<DateTime>(), It.IsAny<string>())).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, null, new DateTime(2023, 01, 08), "worldCup Discussion");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(result, expectedResult);
            Assert.Equal(result, expectedResult);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_StartDate()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDate.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), null, null)).Returns(expectedResult);

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), null, null);

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(result, expectedResult);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_EndDate()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDate.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, It.IsAny<DateTime>(), null)).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, null, new DateTime(2023, 01, 06), null);

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(result, expectedResult);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_Start_Date_And_EndDate()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDate.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), null)).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), new DateTime(2023, 01, 06), null);

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(result, expectedResult);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_Search_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithTitle.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, null, It.IsAny<string>())).Returns(expectedResult);
            var systemUnderTest = new AppointmentBL(MockServiceDAL.Object);

            //Act
            var result = systemUnderTest.GetAllAppointments(0, 5, null, null, "worldCup Discussion");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(result, expectedResult);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }


    }
}
