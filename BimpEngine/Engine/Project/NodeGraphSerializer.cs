using BimpEngine.Engine.Scripting;
using BimpEngine.Engine.Scripting.Enum;
using BimpEngine.Engine.Scripting.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BimpEngine.Engine.Project
{
    public static class NodeGraphSerializer
    {
        private static readonly JsonSerializerOptions Opts = new() { WriteIndented = true };
        public static string Serialize(NodeGraph graph)
        {
            var data = new NodeGraphData();

            foreach (var node in graph.Nodes)
            {
                var nd = new NodeData
                {
                    Id = node.Id,
                    Type = node.GetType().FullName ?? node.GetType().Name,
                    X = node.Position.X,
                    Y = node.Position.Y
                };

                foreach (var port in node.Ports)
                {
                    if (port.Value != null)
                    {
                        nd.PortValues.Add(new PortValueData
                        {
                            PortId = port.Id,
                            Value = JsonSerializer.Serialize(port.Value)
                        });
                    }
                }

                data.Nodes.Add(nd);
            }

            foreach (var conn in graph.Connections)
            {
                data.Connections.Add(new ConnectionData
                {
                    Id = conn.Id,
                    FromNodeId = conn.FromNodeId,
                    FromPortId = conn.FromPortId,
                    ToNodeId = conn.ToNodeId,
                    ToPortId = conn.ToPortId
                });
            }

            foreach (var v in graph.Variables)
            {
                data.Variables.Add(new VariableData
                {
                    Nombre = v.Nombre,
                    Tipo = v.Tipo.ToString(),
                    ValorDefecto = v.ValorDefecto != null ? JsonSerializer.Serialize(v.ValorDefecto) : null,
                    ReferenciaName = v.ReferenciaName
                });
            }

            return JsonSerializer.Serialize(data, Opts);
        }

        public static NodeGraph Deserialize(string json)
        {
            var graph = new NodeGraph();

            if (string.IsNullOrWhiteSpace(json) || json == "{}")
                return graph;

            NodeGraphData data;
            try { data = JsonSerializer.Deserialize<NodeGraphData>(json, Opts) ?? new(); }
            catch { return graph; }

            var nodeMap = new Dictionary<Guid, ScriptNode>();

            foreach (var nd in data.Nodes)
            {
                var node = CreateNode(nd.Type);
                if (node == null) continue;

                SetId(node, nd.Id);
                node.Position = new Point(nd.X, nd.Y);

                foreach (var pv in nd.PortValues)
                {
                    var port = node.Ports.FirstOrDefault(p => p.Id == pv.PortId);
                    if (port != null && pv.Value != null)
                    {
                        try { port.Value = JsonSerializer.Deserialize<object>(pv.Value); }
                        catch { /* ignorar valores inválidos */ }
                    }
                }

                graph.Nodes.Add(node);
                nodeMap[nd.Id] = node;
            }
            foreach (var cd in data.Connections)
            {
                graph.Connections.Add(new NodeConnection
                {
                    Id = cd.Id,
                    FromNodeId = cd.FromNodeId,
                    FromPortId = cd.FromPortId,
                    ToNodeId = cd.ToNodeId,
                    ToPortId = cd.ToPortId
                });
            }
            foreach (var vd in data.Variables)
            {
                var v = new GraphVariable
                {
                    Nombre = vd.Nombre,
                    ReferenciaName = vd.ReferenciaName
                };

                if (System.Enum.TryParse<VariableType>(vd.Tipo, out var tipo))
                    v.Tipo = tipo;

                if (vd.ValorDefecto != null)
                {
                    try { v.ValorDefecto = JsonSerializer.Deserialize<object>(vd.ValorDefecto); }
                    catch { }
                }

                graph.Variables.Add(v);
            }

            return graph;
        }

        private static ScriptNode? CreateNode(string typeName)
        {
            string shortName = typeName.Contains('.') ? typeName[(typeName.LastIndexOf('.') + 1)..] : typeName;

            return shortName switch
            {
                "OnStartNode" => new OnStartNode(),
                "OnClickNode" => new OnClickNode(),
                "OnUpdateNode" => new OnUpdateNode(),
                "TeclaPresionadaNode" => new TeclaPresionadaNode(),
                "TemporizadorNode" => new TemporizadorNode(),
                "MoverObjetoNode" => new MoverObjetoNode(),
                "RotarObjetoNode" => new RotarObjetoNode(),
                "EscalarObjetoNode" => new EscalarObjetoNode(),
                "ImprimirNode" => new ImprimirNode(),
                "SiEntoncesNode" => new SiEntoncesNode(),
                "CompararNode" => new CompararNode(),
                "SumarNode" => new SumarNode(),
                "RestarNode" => new RestarNode(),
                "MultiplicarNode" => new MultiplicarNode(),
                "SinusNode" => new SinusNode(),
                "NumeroConstanteNode" => new NumeroConstanteNode(),
                "ObtenerVariableNode" => new ObtenerVariableNode(),
                "SetVariableNode" => new SetVariableNode(),
                "ObtenerObjetoNode" => new ObtenerObjetoNode(),
                _ => null  
            };
        }

        private static void SetId(ScriptNode node, Guid id)
        {
            var field = typeof(ScriptNode).GetField(
                "<Id>k__BackingField",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            field?.SetValue(node, id);
        }
    }
}
