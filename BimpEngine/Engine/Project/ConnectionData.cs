using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project
{
    public class ConnectionData
    {
        public Guid Id { get; set; }
        public Guid FromNodeId { get; set; }
        public Guid FromPortId { get; set; }
        public Guid ToNodeId { get; set; }
        public Guid ToPortId { get; set; }
    }
}
