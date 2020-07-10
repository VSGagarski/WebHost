using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Commands.Services.Queue
{
    public class AddQueueItemCommandModel:ICommand
    {
        public Guid QueueId { get; set; }



    }
}
