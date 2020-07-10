using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Commands.Services.Queue
{
    public class CreateQueueCommandModel:ICommand
    {

        public string QueueTextTag { get; set; }

        public string Location { get; set; }
        public string Description { get; set; }

    }
}
