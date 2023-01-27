using System.Net;
using System.Text;
using DisprzTraining.Model;
using DisprzTraining.Model.Result;
using DisprzTraining.Tests.Helpers;
using DisprzTraining.Tests.MockDatas;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;


namespace DisprzTraining.Tests.IntegrationTest
{
    public class AppointmentAutomation : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _factory;

        public AppointmentAutomation(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private string URL = "v1/api/appointments";
        private readonly List<Guid> Id = new List<Guid>();

        private static StringContent SerializeInput(object data)
        {
            return new StringContent(JsonConvert.SerializeObject(data), Encoding.Default, "application/json");
        }

        private async Task Add()
        {
            var client = _factory.CreateClient();
            var count = MockData.MockAppointmentDTOList.Count();
            for (var i = 5; i < count; i++)
            {
                var response = await client.PostAsync(URL, SerializeInput(MockData.MockAppointmentDTOList[i]));
                var responseContent = await response.Content.ReadAsStringAsync();
                var appointmentId = JsonConvert.DeserializeObject<NewAppointmentId>(responseContent);

                if (appointmentId != null)
                    Id.Add(appointmentId.Id);
            }
        }

        private async Task Dispose()
        {
            var client = _factory.CreateClient();
            var count = Id.Count();
            for (var i = 0; i < count; i++)
            {
                await client.DeleteAsync($"{URL}/{Id[i]}");
            }
        }





        //////////// add appointment ////////////////
        [Fact]
        public async Task CreateAppointment_Return_Success_On_Creation()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment = MockData.MockAppointmentDTO;

            //Act
            var response = await client.PostAsync(URL, SerializeInput(mockAppointment));
            var responseContent = await response.Content.ReadAsStringAsync();
            var appointmentId = JsonConvert.DeserializeObject<NewAppointmentId>(responseContent);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
            Assert.True(appointmentId?.Id.HasValue());
            await client.DeleteAsync($"{URL}/{appointmentId?.Id}");
        }

        [Fact]
        public async Task CreateAppointment_Return_Conflict()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment1 = MockData.MockAppointmentDTOList[0];
            var mockAppointment2 = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 10, 10, 07, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 10, 09, 10, 10),
                appointmentTitle = "add returns conflict",
                appointmentDescription = "add returns conflict"
            };

            //Act
            //add new appointment
            var postResponse1 = await client.PostAsync(URL, SerializeInput(mockAppointment1));
            var postResponseContent1 = await postResponse1.Content.ReadAsStringAsync();
            var appointmentId = JsonConvert.DeserializeObject<NewAppointmentId>(postResponseContent1);

            //returns null when conflict occurs
            var postResponse2 = await client.PostAsync(URL, SerializeInput(mockAppointment2));
            var responseContent = await postResponse2.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, postResponse2.StatusCode);
            Assert.Equal(AppointmentErrorResponse.ConflictResponse.errorCode, errorResponse?.errorCode);
            Assert.Equal("application/json; charset=utf-8", postResponse2?.Content?.Headers?.ContentType?.ToString());
            await client.DeleteAsync($"{URL}/{appointmentId?.Id}");
        }

        [Fact]
        public async Task CreateAppointment_Return_BadReques_For_PastTimings()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(-1),
                appointmentEndTime = DateTime.Now.AddHours(2),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };

            //Act
            var response = await client.PostAsync(URL, SerializeInput(mockAppointment));
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(AppointmentErrorResponse.PastTiming.errorCode, errorResponse?.errorCode);
        }

        [Fact]
        public async Task CreateAppointment_Return_BadReques_For_InputTime_Not_Within_Range()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(5),
                appointmentEndTime = DateTime.Now.AddHours(18),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };

            //Act
            var response = await client.PostAsync(URL, SerializeInput(mockAppointment));
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(AppointmentErrorResponse.TimeRange.errorCode, errorResponse?.errorCode);
        }



        [Fact]
        public async Task CreateAppointment_Return_BadReques_For_EndTime_Less_Than_StartTime()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(5),
                appointmentEndTime = DateTime.Now.AddHours(3),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };

            //Act
            var response = await client.PostAsync(URL, SerializeInput(mockAppointment));
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(AppointmentErrorResponse.EndTimeLessThanStartTime.errorCode, errorResponse?.errorCode);
        }

        //////////// add appointment ////////////////



        //////////// update Appointment //////////////

        [Fact]
        public async Task UpdateAppointment_Return_No_Content_On_Creation()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment1 = MockData.MockAppointmentDTO;
            var mockAppointment2 = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(1).AddMinutes(40),
                appointmentEndTime = DateTime.Now.AddHours(2),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };

            //Act
            var postResponse = await client.PostAsync(URL, SerializeInput(mockAppointment1));
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var appointmentId = JsonConvert.DeserializeObject<NewAppointmentId>(responseContent);

            var putResponse = await client.PutAsync($"{URL}/{appointmentId?.Id}", SerializeInput(mockAppointment1));

            //Assert
            putResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

            await client.DeleteAsync($"{URL}/{appointmentId?.Id}");
        }

        [Fact]
        public async Task UpdateAppointment_Return_Conflict()
        {
            // Arrange
            var client = _factory.CreateClient();
            var mockAppointment1 = MockData.MockAppointmentDTOList[7];
            var mockAppointment2 = MockData.MockAppointmentDTOList[8];
            var mockUpdateAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 10, 12, 11, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 12, 13, 10, 10),
                appointmentTitle = "trial data",
                appointmentDescription = "trial data desc"
            };

            //Act
            //add new appointment1
            var postResponse1 = await client.PostAsync(URL, SerializeInput(mockAppointment1));
            var responseContent1 = await postResponse1.Content.ReadAsStringAsync();
            var appointmentId1 = JsonConvert.DeserializeObject<NewAppointmentId>(responseContent1);
            //add new appointment2
            var postResponse2 = await client.PostAsync(URL, SerializeInput(mockAppointment2));
            var responseContent2 = await postResponse2.Content.ReadAsStringAsync();
            var appointmentId2 = JsonConvert.DeserializeObject<NewAppointmentId>(responseContent2);

            var putResponse = await client.PutAsync($"{URL}/{appointmentId2?.Id}", SerializeInput(mockUpdateAppointment));
            var responseContent = await putResponse.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, putResponse.StatusCode);
            Assert.Equal(AppointmentErrorResponse.ConflictResponse.errorCode, errorResponse?.errorCode);
            await client.DeleteAsync($"{URL}/{appointmentId1?.Id}");
            await client.DeleteAsync($"{URL}/{appointmentId2?.Id}");

        }

        [Fact]
        public async Task UpdateAppointment_Return_BadRequest_On_PastTimings()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(-1),
                appointmentEndTime = DateTime.Now.AddHours(2),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var MockId = Guid.NewGuid();

            //Act
            var response = await client.PutAsync($"{URL}/{MockId}", SerializeInput(mockAppointment));
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(AppointmentErrorResponse.PastTiming.errorCode, errorResponse?.errorCode);
        }

        [Fact]
        public async Task UpdateAppointment_Return_BadRequest_For_Input_Not_Within_Range()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(5),
                appointmentEndTime = DateTime.Now.AddHours(18),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var MockId = Guid.NewGuid();

            //Act
            var response = await client.PutAsync($"{URL}/{MockId}", SerializeInput(mockAppointment));
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(AppointmentErrorResponse.TimeRange.errorCode, errorResponse?.errorCode);
        }


        [Fact]
        public async Task UpdateAppointment_Return_BadRequest_For_EndTime_Less_Than_StartTime()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(5),
                appointmentEndTime = DateTime.Now.AddHours(3),
                appointmentTitle = "add returnd bad req",
                appointmentDescription = "add returnd bad req"
            };
            var MockId = Guid.NewGuid();

            //Act
            var response = await client.PutAsync($"{URL}/{MockId}", SerializeInput(mockAppointment));
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(AppointmentErrorResponse.EndTimeLessThanStartTime.errorCode, errorResponse?.errorCode);
        }


        [Fact]
        public async Task UpdateAppointment_Return_Not_Found()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment = MockData.MockAppointmentDTO;
            var MockId = Guid.NewGuid();

            //Act
            var response = await client.PutAsync($"{URL}/{MockId}", SerializeInput(mockAppointment));
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(AppointmentErrorResponse.DataNotFound.errorCode, errorResponse?.errorCode);
        }
        ///////////// update appointment ////////////////



        //////////// get appointment by Id ////////////////

        [Fact]
        public async Task GetAppointmentBy_Id_Integration_Returns_200_Success()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment = MockData.MockAppointmentDTO;

            //Act
            var postResponse = await client.PostAsync(URL, SerializeInput(mockAppointment));
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var appointmentId = JsonConvert.DeserializeObject<NewAppointmentId>(responseContent);

            var getResponse = await client.GetAsync($"{URL}/{appointmentId?.Id}");
            var getResponseContent = await getResponse.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<Appointment>(getResponseContent);

            //Assert
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(appointmentId?.Id, appointment?.appointmentId);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal("application/json; charset=utf-8", getResponse?.Content?.Headers?.ContentType?.ToString());

            var deleteResponse = await client.DeleteAsync($"{URL}/{appointmentId?.Id}");
        }


        [Fact]
        public async Task GetAppointmentBy_Id_Integration_Returns_404_Not_Found()
        {
            //Arrange
            var client = _factory.CreateClient();
            var MockID = Guid.NewGuid();
            //Act
            var getResponse = await client.GetAsync($"{URL}/{MockID}");
            var responseContent = await getResponse.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
            //Assert
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
            Assert.Equal(AppointmentErrorResponse.DataNotFound.errorCode, errorResponse?.errorCode);
        }

        //////////// get appointment by Id ////////////////




        //////////// delete appointment ////////////////

        [Fact]
        public async Task DeleteAppointment_Returns_204_Success()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockAppointment = MockData.MockAppointmentDTOList[0];

            //Act
            var postResponse = await client.PostAsync(URL, SerializeInput(mockAppointment));
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var appointmentId = JsonConvert.DeserializeObject<NewAppointmentId>(responseContent);

            var deleteResponse = await client.DeleteAsync($"{URL}/{appointmentId?.Id}");

            //Assert
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }


        [Fact]
        public async Task DeleteAppointment_Returns_404_Not_Found()
        {
            //Arrange
            var client = _factory.CreateClient();
            var MockID = Guid.NewGuid();

            //Act
            var deleteResponse = await client.DeleteAsync($"{URL}/{MockID}");
            var responseContent = await deleteResponse.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            Assert.Equal(AppointmentErrorResponse.DataNotFound.errorCode, errorResponse?.errorCode);
        }

        //////////// delete appointment ////////////////




        //////////// get appointment ////////////////

        [Fact]
        public async Task Get_Appointments_Returns_Empty_And_200_Success_For_All_Input()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"v1/api/appointments?offSet=0&fetchCount=5&startDate=2023-10-17&endDate=2023-10-18&searchTitle=no data");
            var content = await response.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<PaginatedAppointments>(content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
            Assert.Equal(0, appointment?.appointments.Count());
            Assert.False(appointment?.isTruncated);
        }

        [Fact]

        public async Task Get_Appointments_Returns_Appointment_And_200_Success_For_All_Input()
        {
            //Arrange
            await Add();
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"v1/api/appointments?offSet=0&fetchCount=5&startDate=2023-10-12&endDate=2023-10-13&searchTitle=trial data");
            var content = await response.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<PaginatedAppointments>(content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
            Assert.Equal(3, appointment?.appointments.Count());
            Assert.False(appointment?.isTruncated);

            await Dispose();
        }

        [Fact]
        public async Task Get_Appointments_Returns_Appointment_And_200_Success_For_StartDate_And_EndDate()
        {
            //Arrange
            await Add();
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"v1/api/appointments?offSet=0&fetchCount=5&startDate=2023-10-11&endDate=2023-10-13");
            var content = await response.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<PaginatedAppointments>(content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
            Assert.Equal(5, appointment?.appointments.Count());
            Assert.False(appointment?.isTruncated);

            await Dispose();
        }

        [Fact]
        public async Task Get_Appointments_Returns_Appointment_And_200_Success_For_StartDate()
        {
            //Arrange
            await Add();
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"v1/api/appointments?offSet=0&fetchCount=5&startDate=2023-10-12");
            var content = await response.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<PaginatedAppointments>(content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
            Assert.Equal(4, appointment?.appointments.Count());
            Assert.False(appointment?.isTruncated);

            await Dispose();
        }

        [Fact]
        public async Task Get_Appointments_Returns_Appointment_And_200_Success_For_EndDate()
        {
            //Arrange
            await Add();
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"v1/api/appointments?offSet=0&fetchCount=5&endDate=2023-10-11");
            var content = await response.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<PaginatedAppointments>(content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
            Assert.Equal(1, appointment?.appointments.Count());
            Assert.False(appointment?.isTruncated);

            await Dispose();
        }

        [Fact]
        public async Task Get_Appointments_Returns_Appointment_And_200_Success_For_Title()
        {
            //Arrange
            await Add();
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"v1/api/appointments?offSet=0&fetchCount=5&searchTitle=trial data");
            var content = await response.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<PaginatedAppointments>(content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
            Assert.Equal(3, appointment?.appointments.Count());
            Assert.False(appointment?.isTruncated);
            await Dispose();
        }

        [Fact]
        public async Task Get_Appointments_Returns_Appointment_And_200_Success_For_StartDate_And_Title()
        {
            //Arrange
            await Add();
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"v1/api/appointments?offSet=0&fetchCount=5&startDate=2023-10-11&searchTitle=mock data");
            var content = await response.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<PaginatedAppointments>(content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
            Assert.Equal(1, appointment?.appointments.Count());
            Assert.False(appointment?.isTruncated);

            await Dispose();
        }

        [Fact]
        public async Task Get_Appointments_Returns_Appointment_And_200_Success_For_EndDate_And_Title()
        {
            //Arrange
            await Add();
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"v1/api/appointments?offSet=0&fetchCount=5&endDate=2023-10-14&searchTitle=data 13");
            var content = await response.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<PaginatedAppointments>(content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
            Assert.Equal(1, appointment?.appointments.Count());
            Assert.False(appointment?.isTruncated);

            await Dispose();
        }

        //////////// get appointment ////////////////


    }
}