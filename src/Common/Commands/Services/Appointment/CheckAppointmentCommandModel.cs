using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Commands.Services.Appointment
{
    public class CheckAppointmentCommandModel:ICommand
    {
        public Guid Id { get; set; }
        public Guid IdDevice { get; set; }
        public string Data { get; set; }

        public CheckAppointmentCommandModel(Guid id, Guid idDevice, string data)
        {
            Id = id;
            IdDevice = idDevice;
            Data = data;
        }
    }
}
