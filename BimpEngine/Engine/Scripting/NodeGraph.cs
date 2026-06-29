using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public class NodeGraph
    {
        public List<ScriptNode> Nodes { get; } = new();
        public List<NodeConnection> Connections { get; } = new();

        public void AddNode(ScriptNode node) => Nodes.Add(node);

        public void RemoveNode(Guid nodeId)
        {
            Nodes.RemoveAll(n => n.Id == nodeId);
            Connections.RemoveAll(c => c.FromNodeId == nodeId || c.ToNodeId == nodeId);
        }

        public bool Connect(Guid fromNodeId, Guid fromPortId, Guid toNodeId, Guid toPortId)
        {
            Connections.RemoveAll(c => c.ToNodeId == toNodeId && c.ToPortId == toPortId);
            Connections.Add(new NodeConnection
            {
                FromNodeId = fromNodeId,
                FromPortId = fromPortId,
                ToNodeId = toNodeId,
                ToPortId = toPortId
            });
            return true;
        }

        public void Disconnect(Guid connectionId)
            => Connections.RemoveAll(c => c.Id == connectionId);
        public ScriptNode? FindNode(Guid id) => Nodes.FirstOrDefault(n => n.Id == id);
        public NodePort? FindPort(Guid nodeId, Guid portId)
            => FindNode(nodeId)?.Ports.FirstOrDefault(p => p.Id == portId);
        public NodeConnection? GetConnectionToPort(Guid nodeId, Guid portId)
            => Connections.FirstOrDefault(c => c.ToNodeId == nodeId && c.ToPortId == portId);
        public IEnumerable<NodeConnection> GetConnectionsFromPort(Guid nodeId, Guid portId)
            => Connections.Where(c => c.FromNodeId == nodeId && c.FromPortId == portId);
        public IEnumerable<ScriptNode> GetEventNodes()
            => Nodes.Where(n => n.Category == NodeCategory.Event);
    }
}
