using DisprzTraining.Model;

namespace DisprzTraining.Tests.MockDatas
{
    public static class MockData
    {
        public static AppointmentDTO MockAppointmentDTO = new AppointmentDTO()
        {
            appointmentStartTime = DateTime.Now.AddHours(1),
            appointmentEndTime = DateTime.Now.AddHours(2),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        };

        public static Appointment MockAppointmentByID = new Appointment()
        {
            appointmentId = Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        };


        //appointments matched with the searchDate
        public static List<Appointment> MockAppointmentWithDate = new List<Appointment>()
        {
         new Appointment{
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 08, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("27921518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 12, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 08, 13, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
        new Appointment{
            appointmentId=Guid.Parse("17981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 08, 15, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("47981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 15, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 08, 16, 30, 00),
            appointmentTitle = "StandUp",
            appointmentDescription = "Discussion on CalendarUI"
        }
        };

        //appointments matched with the searchTitle
        public static List<Appointment> MockAppointmentWithTitle = new List<Appointment>()
        {
         new Appointment{
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("27921518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 12, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
        new Appointment{
            appointmentId=Guid.Parse("17981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 07, 14, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 07, 15, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        }
        };

        //appointments matched with the searchDateAndTitle
        public static List<Appointment> MockAppointmentWithDateAndTitle = new List<Appointment>()
        {
         new Appointment{
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 08, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("27921518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 12, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 08, 13, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        }
        };

        //TotalAppointments
        public static List<Appointment> MockAppointmentList = new List<Appointment>()
        {
         new Appointment{
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
                appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
                appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("27921518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 15, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
        new Appointment{
            appointmentId=Guid.Parse("17981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 15, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("47981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 15, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 16, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
        new Appointment{
            appointmentId=Guid.Parse("57981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 16, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 17, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("67981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 17, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 18, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
        new Appointment{
            appointmentId=Guid.Parse("77981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 18, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 19, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("87981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 19, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 20, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
        new Appointment{
            appointmentId=Guid.Parse("97981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 21, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 22, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("10981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 22, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 23, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        }
        };



//Used in DAL Layer 
        public static List<AppointmentDTO> MockAppointmentDTOList = new List<AppointmentDTO>()
        {
             //10-10-2023
            new AppointmentDTO
            {
                appointmentStartTime = new DateTime(2023, 10, 10, 08, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 10, 09, 10, 10),
                appointmentTitle = "test data",
                appointmentDescription = "test data desc"
            },
            new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 10, 10, 10, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 10, 12, 10, 10),
                appointmentTitle = "test data",
                appointmentDescription = "test data desc"
            },
            new AppointmentDTO
            {
                appointmentStartTime = new DateTime(2023, 10, 10, 12, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 10, 13, 10, 10),
                appointmentTitle = "test data",
                appointmentDescription = "test data desc"
            },
            
            //11-10-2023
            new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 10, 11, 08, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 11, 09, 10, 10),
                appointmentTitle = "mock data",
                appointmentDescription = "mock data desc"
            },
            new AppointmentDTO
            {
                appointmentStartTime = new DateTime(2023, 10, 11, 10, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 11, 12, 10, 10),
                appointmentTitle = "mock data",
                appointmentDescription = "mock data desc"
            },
            new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 10, 11, 12, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 11, 13, 10, 10),
                appointmentTitle = "mock data",
                appointmentDescription = "mock data desc"
            },

             //12-10-2023
            new AppointmentDTO
            {
                appointmentStartTime = new DateTime(2023, 10, 12, 08, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 12, 09, 10, 10),
                appointmentTitle = "trial data",
                appointmentDescription = "trial data desc"
            },
            new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 10, 12, 10, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 12, 12, 10, 10),
                appointmentTitle = "trial data",
                appointmentDescription = "trial data desc"
            },
            new AppointmentDTO
            {
                appointmentStartTime = new DateTime(2023, 10, 12, 12, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 12, 13, 10, 10),
                appointmentTitle = "trial data",
                appointmentDescription = "trial data desc"
            },

             //13-10-2023
            new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 10, 13, 10, 10, 10),
                appointmentEndTime = new DateTime(2023, 10, 13, 12, 10, 10),
                appointmentTitle = "data 13",
                appointmentDescription = "trial data desc"
            }

        };
    }
}


