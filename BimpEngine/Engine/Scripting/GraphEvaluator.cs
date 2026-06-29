using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public class GraphEvaluator
    {
        private readonly NodeGraph _graph;

        public GraphEvaluator(NodeGraph graph) => _graph = graph;

        public void FireEvent(string eventName, ScriptContext ctx)
        {
            var eventNode = _graph.GetEventNodes()
                .FirstOrDefault(n => n.Title == eventName);
            if (eventNode == null) return;

            ctx.Steps = 0;
            ExecuteNode(eventNode, ctx);
        }

        private void ExecuteNode(ScriptNode node, ScriptContext ctx)
        {
            if (!ctx.Running || ctx.Steps++ > ctx.MaxSteps) return;

            // Resolve all data inputs before executing
            ResolveInputs(node, ctx);

            // Execute and get the next exec port to follow
            var nextExecPort = node.Execute(ctx);

            if (nextExecPort == null) return;

            // Follow exec connections from that output port
            foreach (var conn in _graph.GetConnectionsFromPort(node.Id, nextExecPort.Id))
            {
                var nextNode = _graph.FindNode(conn.ToNodeId);
                if (nextNode != null)
                    ExecuteNode(nextNode, ctx);
            }
        }

        private void ResolveInputs(ScriptNode node, ScriptContext ctx)
        {
            foreach (var input in node.Inputs.Where(p => p.DataType != PortType.Exec))
            {
                var conn = _graph.GetConnectionToPort(node.Id, input.Id);
                if (conn == null) { input.RuntimeValue = input.Value; continue; }

                var srcNode = _graph.FindNode(conn.FromNodeId);
                var srcPort = _graph.FindPort(conn.FromNodeId, conn.FromPortId);

                if (srcNode == null || srcPort == null) continue;

                // Evaluate source node (data nodes don't follow exec)
                ResolveInputs(srcNode, ctx);
                srcNode.Execute(ctx);
                input.RuntimeValue = srcPort.RuntimeValue;
            }
        }
    }
}
