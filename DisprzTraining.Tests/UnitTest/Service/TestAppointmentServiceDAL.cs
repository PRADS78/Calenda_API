using DisprzTraining.DataAccess;
using DisprzTraining.Model;
using DisprzTraining.Tests.Helpers;
using DisprzTraining.Tests.MockDatas;

namespace DisprzTraining.Tests.UnitTests.Service
{
    public class TestAppointmentServiceDAL
    {
        private readonly IAppointmentDAL serviceUnderTest;
        public TestAppointmentServiceDAL()
        {
            serviceUnderTest = new AppointmentDAL();
        }

        private readonly List<Guid> Id = new List<Guid>();

        private void Add()
        {
            for (var i = 0; i < MockData.MockAppointmentDTOList.Count(); i++)
            {
                var newId = serviceUnderTest.AddAppointment(MockData.MockAppointmentDTOList[i]);
                if (newId != null)
                    Id.Add(newId.Id);
            }
        }

        private void Dispose()
        {
            for (var i = 0; i < Id.Count(); i++)
            {
                var deleted = serviceUnderTest.DeleteAppointment(Id[i]);
            }
        }




        //////////////              //Delete appointment tests //                 ////////////////

        [Fact]
        public void DeleteAppointment_Returns_True_On_Succesful_Deletion()
        {
            //Arrange
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 17, 10, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 17, 12, 10, 10),
                appointmentTitle = "delete returns true ",
                appointmentDescription = "delete returns true"
            };
            //Act
            var result = serviceUnderTest.AddAppointment(MockAppointment);
            if (result != null)
            {
                var appointmentDeleted = serviceUnderTest.DeleteAppointment(result.Id);

                //Assert
                Assert.True(appointmentDeleted);
            }
            else
                Assert.Fail("Delete Appointment Failed");
        }

        [Fact]
        public void DeleteAppointment_Returns_False_On_Not_Found()
        {
            //Arrange
            var MockId = Guid.NewGuid();

            //Act
            var appointmentDeleted = serviceUnderTest.DeleteAppointment(MockId);

            Assert.False(appointmentDeleted);
        }
        //////////////              //Delete appointment tests //                 ////////////////




        //////////////              // Get Appointment By Id //                 ////////////////

        [Fact]
        public void GetAppointment_By_ID_Returns_Appointment()
        {
            //Arrange
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 17, 10, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 17, 12, 10, 10),
                appointmentTitle = "getById returns Appointment",
                appointmentDescription = "getById returns Appointment"
            };

            //Act
            var result = serviceUnderTest.AddAppointment(MockAppointment);
            if (result != null)
            {
                var getAppointment = serviceUnderTest.GetAppointmentById(result.Id);
                var appointmentDeleted = serviceUnderTest.DeleteAppointment(result.Id);

                //Assert
                Assert.IsType<Appointment>(getAppointment);
                Assert.Equal(result.Id, getAppointment.appointmentId);
                Assert.True(appointmentDeleted);
            }
            else
                Assert.Fail("Get Appointment By Id Appointment Failed");
        }

        [Fact]
        public void GetAppointment_By_ID_Returns_Null()
        {
            //Arrange

            var MockId = Guid.NewGuid();
            //Act

            var getAppointment = serviceUnderTest.GetAppointmentById(MockId);

            //Assert
            Assert.Null(getAppointment);
        }

        //////////////              // Get Appointment By Id //                 ////////////////





        //////////////              //Add new appointment //                 ////////////////

        [Fact]
        public void AddAppointment_Returns_True_On_Succesful_Creation()
        {
            //Arrange
            var MockAppointment1 = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 18, 10, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 18, 12, 10, 10),
                appointmentTitle = "add returns true ",
                appointmentDescription = "add returns true"
            };
            //Act
            var result = serviceUnderTest.AddAppointment(MockAppointment1);

            //Assert
            Assert.IsType<NewAppointmentId>(result);
            Assert.True(result.Id.HasValue());
            if (result != null)
            {
                var appointmentDeleted = serviceUnderTest.DeleteAppointment(result.Id);
                Assert.True(appointmentDeleted);
            }
            else
                Assert.Fail("Add Appointment Failed");
        }

        [Fact]
        public void AddAppointment_Returns_False_On_Conflict_Between_Appointments()
        {
            //Arrange
            var MockAppointment1 = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 18, 10, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 18, 12, 10, 10),
                appointmentTitle = "add returns true 1",
                appointmentDescription = "add returns true 1"
            };
            var MockAppointment2 = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 18, 08, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 18, 14, 10, 10),
                appointmentTitle = "add returns true 2",
                appointmentDescription = "add returns true 2"
            };
            var MockAppointment3 = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 18, 13, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 18, 15, 10, 10),
                appointmentTitle = "add returns conflict",
                appointmentDescription = "add returns conflict"
            };

            //Act
            var result1 = serviceUnderTest.AddAppointment(MockAppointment1);
            var result2 = serviceUnderTest.AddAppointment(MockAppointment2);
            var result3 = serviceUnderTest.AddAppointment(MockAppointment3);
            if (result1 != null && result3 != null)
            {
                var appointment_1_Deleted = serviceUnderTest.DeleteAppointment(result1.Id);
                var appointment_3_Deleted = serviceUnderTest.DeleteAppointment(result3.Id);

                //Assert
                Assert.Null(result2);
                Assert.True(appointment_1_Deleted);
                Assert.True(appointment_3_Deleted);
            }
            else
                Assert.Fail("Add Appointment Conflict Failed");
        }

        //////////////              //Add new appointment //                 ////////////////





        //////////////              //update Existing Appointment // 

        // //update an existing appointment
        [Fact]
        public void UpdateAppointment_Returns_True_On_Succesful_Updation()
        {
            //Arrange
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 17, 10, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 17, 12, 10, 10),
                appointmentTitle = "update returns true ",
                appointmentDescription = "update returns true "
            };
            var MockUpdateAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 17, 11, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 17, 12, 10, 10),
                appointmentTitle = "update returns true ",
                appointmentDescription = "update returns true "
            };

            //Act
            var result1 = serviceUnderTest.AddAppointment(MockAppointment);
            if (result1 != null)
            {
                var updated = serviceUnderTest.UpdateAppointment(result1.Id, MockUpdateAppointment);
                var appointmentDeleted = serviceUnderTest.DeleteAppointment(result1.Id);

                //Assert
                Assert.True(updated);
                Assert.True(appointmentDeleted);
            }
            else
                Assert.Fail("Update Appointment Conflict Failed");
        }

        [Fact]
        public void UpdateAppointment_Returns_False_On_Conflict_Between_Appointments()
        {
            //Arrange
            var MockAppointment1 = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 18, 10, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 18, 12, 10, 10),
                appointmentTitle = "update returns conflict_1",
                appointmentDescription = "update returns conflict_1"
            };
            var MockAppointment2 = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 18, 08, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 18, 10, 10, 10),
                appointmentTitle = "update returns conflict_2",
                appointmentDescription = "update returns conflict_2"
            };
            var MockAppointment3 = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 18, 13, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 18, 15, 10, 10),
                appointmentTitle = "update returns conflict_3",
                appointmentDescription = "update returns conflict_3"
            };
            var MockUpdateAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2021, 10, 18, 11, 10, 10),
                appointmentEndTime = new DateTime(2021, 10, 18, 14, 10, 10),
                appointmentTitle = "update returns conflict_4",
                appointmentDescription = "update returns conflict_4"
            };

            //Act
            var result1 = serviceUnderTest.AddAppointment(MockAppointment1);
            var result2 = serviceUnderTest.AddAppointment(MockAppointment2);
            var result3 = serviceUnderTest.AddAppointment(MockAppointment3);
            if (result1 != null && result2 != null && result3 != null)
            {
                var updated = serviceUnderTest.UpdateAppointment(result1.Id, MockUpdateAppointment);
                var appointment_1_Deleted = serviceUnderTest.DeleteAppointment(result1.Id);
                var appointment_2_Deleted = serviceUnderTest.DeleteAppointment(result2.Id);
                var appointment_3_Deleted = serviceUnderTest.DeleteAppointment(result3.Id);

                //Assert
                Assert.False(updated);
                Assert.True(appointment_1_Deleted);
                Assert.True(appointment_2_Deleted);
                Assert.True(appointment_3_Deleted);
            }
            else
                Assert.Fail("Update Appointment Conflict Failed");
        }

        [Fact]
        public void UpdateAppointment_Returns_Null_On_No_Appointment_Found()
        {
            //Arrange
            var MockUpdateAppointment = MockData.MockAppointmentDTOList[0];
            var MockId = Guid.NewGuid();

            //Act
            var updated = serviceUnderTest.UpdateAppointment(MockId, MockUpdateAppointment);

            //Assert
            Assert.Null(updated);
        }

        //////////////              //update Existing Appointment // 




        //////////////              //Get All Appointments // 
        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_OffSet_0_And_FetchCount_5()
        {
            //Arrange
            Add();
            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, null, null, null);

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.True(result.isTruncated);
            Assert.Equal(5, result.appointments.Count());

            //dispose
            Dispose();
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_OffSet_5_And_FetchCount_5()
        {
            //Arrange
            Add();
            //Act
            var result = serviceUnderTest.GetAllAppointments(5, 5, null, null, null);

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Equal(5, result.appointments.Count());

            //dispose
            Dispose();
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_Start_Date_And_End_Date_And_Title()
        {
            //Arrange
            Add();

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 10, 10), new DateTime(2023, 10, 12), "test data");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Equal(3, result.appointments.Count());
            Assert.True(result.appointments[0].appointmentTitle == "test data");

            //dispose
            Dispose();
        }

        [Fact]
        public void GetAllAppointment_Returns_Empty_Paginated_Result_With_Start_Date_And_End_Date_And_Title()
        {
            //Arrange
            Add();

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 10, 15), new DateTime(2023, 10, 18), "test data not Found");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Empty(result.appointments);

            //dispose
            Dispose();
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_Start_Date_And_Title()
        {
            //Arrange
            Add();
            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 10, 10), null, "mock data");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Equal(3, result.appointments.Count());

            //dispose
            Dispose();
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_End_Date_And_Title()
        {
            //Arrange
            Add();
            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, null, new DateTime(2023, 10, 11), "mock data");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Equal(3, result.appointments.Count());

            //dispose
            Dispose();
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_Start_Date()
        {
            //Arrange
            Add();

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, -1, new DateTime(2023, 10, 11), null, null);

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Equal(7, result.appointments.Count());

            //dispose
            Dispose();
        }


        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_End_Date()
        {
            //Arrange
            Add();

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, -1, null, new DateTime(2023, 10, 12), null);

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Equal(9, result.appointments.Count());

            //dispose
            Dispose();
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_Start_Date_And_End_Date()
        {
            //Arrange
            Add();

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, -1, new DateTime(2023, 10, 10), new DateTime(2023, 10, 12), null);

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Equal(9, result.appointments.Count());

            //dispose
            Dispose();
        }

        [Fact]
        public void GetAllAppointment_Returns_Empty_Paginated_Result_With_Start_Date_And_End_Date()
        {
            //Arrange
            Add();

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, -1, new DateTime(2023, 10, 15), new DateTime(2023, 10, 19), null);

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Empty(result.appointments);

            //dispose
            Dispose();
        }


        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_Title()
        {
            //Arrange
            Add();

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, null, null, "test data");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Equal(3, result.appointments.Count());

            //dispose
            Dispose();
        }

        [Fact]
        public void GetAllAppointment_Returns_Empty_Paginated_Result_With_Title()
        {
            //Arrange
            Add();

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, null, null, "test data Not Found");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.False(result.isTruncated);
            Assert.Empty(result.appointments);


            //dispose
            Dispose();
        }

    }
}

