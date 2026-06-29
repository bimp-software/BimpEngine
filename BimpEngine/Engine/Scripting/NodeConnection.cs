using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public class NodeConnection
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid FromNodeId { get; set; }
        public Guid FromPortId { get; set; }
        public Guid ToNodeId { get; set; }
        public Guid ToPortId { get; set; }
    }
}
