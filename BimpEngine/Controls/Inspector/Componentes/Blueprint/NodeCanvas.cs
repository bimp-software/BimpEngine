using BimpEngine.Engine.Scripting;
using BimpEngine.Engine.Scripting.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace BimpEngine.Controls.Inspector.Componentes.Blueprint
{
    /// <summary>
    /// Full-screen node editor canvas.
    /// - Drag nodes to reposition them
    /// - Drag from an output port dot to an input port dot to connect
    /// - Right-click empty space → add node menu
    /// - Right-click on node → delete
    /// - Middle-mouse or Alt+drag → pan
    /// - Ctrl+scroll → zoom
    /// </summary>
    public class NodeCanvas : Control
    {
        // ── Graph ─────────────────────────────────────────────────────────
        public NodeGraph Graph { get; private set; } = new();

        // ── View transform ────────────────────────────────────────────────
        private PointF _pan = new(0, 0);
        private float _zoom = 1f;
        private const float ZoomMin = 0.3f, ZoomMax = 2.5f;

        // ── Interaction state ─────────────────────────────────────────────
        private ScriptNode? _draggingNode;
        private Point _dragOffset;
        private ScriptNode? _connectFromNode;
        private NodePort? _connectFromPort;
        private Point _mouseScreen;

        // ── Pan ───────────────────────────────────────────────────────────
        private bool _panning;
        private Point _panStart;
        private PointF _panOrigin;

        // ── Layout constants ──────────────────────────────────────────────
        private const int HeaderH = 26;
        private const int PortH = 22;
        private const int PortRadius = 6;
        private const int PortMargin = 12;

        public NodeCanvas()
        {
            DoubleBuffered = true;
            BackColor = Color.FromArgb(25, 25, 28);
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint, true);
        }

        public void SetGraph(NodeGraph graph)
        {
            Graph = graph;
            Invalidate();
        }

        // ── World ↔ Screen ────────────────────────────────────────────────
        private PointF WorldToScreen(PointF w)
            => new(w.X * _zoom + _pan.X, w.Y * _zoom + _pan.Y);

        private PointF ScreenToWorld(Point s)
            => new((s.X - _pan.X) / _zoom, (s.Y - _pan.Y) / _zoom);

        // ── Paint ─────────────────────────────────────────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            DrawGrid(g);

            g.TranslateTransform(_pan.X, _pan.Y);
            g.ScaleTransform(_zoom, _zoom);

            // Draw connections
            foreach (var conn in Graph.Connections)
                DrawConnection(g, conn);

            // Draw in-progress connection
            if (_connectFromNode != null && _connectFromPort != null)
            {
                var fromPt = GetPortCenter(_connectFromNode, _connectFromPort);
                var toPt = ScreenToWorld(_mouseScreen);
                DrawBezier(g, fromPt, toPt,
                    NodePort.ColorFor(_connectFromPort.DataType), 2f);
            }

            // Draw nodes
            foreach (var node in Graph.Nodes)
                DrawNode(g, node);

            g.ResetTransform();
        }

        private void DrawGrid(Graphics g)
        {
            using var pen = new Pen(Color.FromArgb(38, 38, 42), 1f);
            using var pen2 = new Pen(Color.FromArgb(50, 50, 55), 1f);
            int gridSmall = 20, gridBig = 100;

            float offX = _pan.X % (gridSmall * _zoom);
            float offY = _pan.Y % (gridSmall * _zoom);

            for (float x = offX; x < Width; x += gridSmall * _zoom)
            {
                bool big = ((int)((x - offX) / (gridSmall * _zoom)) % (gridBig / gridSmall)) == 0;
                g.DrawLine(big ? pen2 : pen, x, 0, x, Height);
            }
            for (float y = offY; y < Height; y += gridSmall * _zoom)
            {
                bool big = ((int)((y - offY) / (gridSmall * _zoom)) % (gridBig / gridSmall)) == 0;
                g.DrawLine(big ? pen2 : pen, 0, y, Width, y);
            }
        }

        private void DrawNode(Graphics g, ScriptNode node)
        {
            var pos = new PointF(node.Position.X, node.Position.Y);
            var size = node.Size;
            var rect = new RectangleF(pos.X, pos.Y, size.Width, size.Height);

            // Shadow
            using var shadow = new SolidBrush(Color.FromArgb(80, 0, 0, 0));
            g.FillRectangle(shadow, rect.X + 4, rect.Y + 4, rect.Width, rect.Height);

            // Body
            using var bodyBrush = new SolidBrush(Color.FromArgb(48, 48, 52));
            g.FillRectangle(bodyBrush, rect);

            // Header
            using var headBrush = new SolidBrush(node.HeaderColor);
            g.FillRectangle(headBrush, rect.X, rect.Y, rect.Width, HeaderH);

            // Border
            bool sel = _draggingNode == node;
            using var border = new Pen(sel ? Color.White : Color.FromArgb(70, 70, 75), 1.5f);
            g.DrawRectangle(border, rect.X, rect.Y, rect.Width, rect.Height);

            // Title
            using var titleFont = new Font("Segoe UI", 8f, FontStyle.Bold);
            g.DrawString(node.Title, titleFont, Brushes.White,
                new RectangleF(rect.X + 8, rect.Y + 5, rect.Width - 16, HeaderH),
                new StringFormat { LineAlignment = StringAlignment.Center });

            // Ports
            int inRow = 0, outRow = 0;
            foreach (var port in node.Ports)
            {
                float py;
                PointF dotPt;

                if (port.Direction == PortDirection.Input)
                {
                    py = pos.Y + HeaderH + 8 + inRow * PortH;
                    dotPt = new PointF(pos.X - 1, py + PortH / 2f);
                    DrawPortDot(g, dotPt, port);
                    using var pf = new Font("Segoe UI", 7.5f);
                    g.DrawString(port.Name, pf, Brushes.LightGray,
                        dotPt.X + PortRadius + 4, dotPt.Y - 7);
                    var sp = WorldToScreen(dotPt); port.ScreenPos = new Point((int)sp.X, (int)sp.Y);
                    inRow++;
                }
                else
                {
                    py = pos.Y + HeaderH + 8 + outRow * PortH;
                    dotPt = new PointF(pos.X + size.Width + 1, py + PortH / 2f);
                    DrawPortDot(g, dotPt, port);
                    using var pf = new Font("Segoe UI", 7.5f);
                    var sf = new StringFormat { Alignment = StringAlignment.Far };
                    g.DrawString(port.Name, pf, Brushes.LightGray,
                        new RectangleF(pos.X, dotPt.Y - 7, size.Width - PortRadius - 4, PortH), sf);
                    var sp = WorldToScreen(dotPt); port.ScreenPos = new Point((int)sp.X, (int)sp.Y);
                    outRow++;
                }
            }
        }

        private void DrawPortDot(Graphics g, PointF center, NodePort port)
        {
            var r = new RectangleF(center.X - PortRadius, center.Y - PortRadius,
                                      PortRadius * 2, PortRadius * 2);
            var fill = NodePort.ColorFor(port.DataType);

            // Filled if connected
            bool connected = port.Direction == PortDirection.Output
                ? Graph.GetConnectionsFromPort(FindNodeOf(port)?.Id ?? Guid.Empty, port.Id).Any()
                : Graph.GetConnectionToPort(FindNodeOf(port)?.Id ?? Guid.Empty, port.Id) != null;

            using var b = new SolidBrush(connected ? fill : Color.FromArgb(40, 40, 44));
            g.FillEllipse(b, r);
            using var p = new Pen(fill, 1.5f);
            g.DrawEllipse(p, r);
        }

        private void DrawConnection(Graphics g, NodeConnection conn)
        {
            var fromNode = Graph.FindNode(conn.FromNodeId);
            var fromPort = Graph.FindPort(conn.FromNodeId, conn.FromPortId);
            var toNode = Graph.FindNode(conn.ToNodeId);
            var toPort = Graph.FindPort(conn.ToNodeId, conn.ToPortId);

            if (fromNode == null || fromPort == null || toNode == null || toPort == null) return;

            var from = GetPortCenter(fromNode, fromPort);
            var to = GetPortCenter(toNode, toPort);
            DrawBezier(g, from, to, NodePort.ColorFor(fromPort.DataType), 2.2f);
        }

        private static void DrawBezier(Graphics g, PointF from, PointF to, Color color, float width)
        {
            float dx = MathF.Abs(to.X - from.X) * 0.6f + 30f;
            var p1 = new PointF(from.X + dx, from.Y);
            var p2 = new PointF(to.X - dx, to.Y);
            using var pen = new Pen(color, width) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            g.DrawBezier(pen, from, p1, p2, to);
        }

        private PointF GetPortCenter(ScriptNode node, NodePort port)
        {
            int inRow = 0, outRow = 0;
            foreach (var p in node.Ports)
            {
                float py = port.Direction == PortDirection.Input
                    ? node.Position.Y + HeaderH + 8 + inRow * PortH + PortH / 2f
                    : node.Position.Y + HeaderH + 8 + outRow * PortH + PortH / 2f;

                if (p == port)
                {
                    float px = port.Direction == PortDirection.Input
                        ? node.Position.X - 1
                        : node.Position.X + node.Size.Width + 1;
                    return new PointF(px, py);
                }
                if (p.Direction == PortDirection.Input) inRow++;
                else outRow++;
            }
            return new PointF(node.Position.X, node.Position.Y);
        }

        private ScriptNode? FindNodeOf(NodePort port)
            => Graph.Nodes.FirstOrDefault(n => n.Ports.Contains(port));

        // ── Mouse ─────────────────────────────────────────────────────────
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            _mouseScreen = e.Location;

            if (e.Button == MouseButtons.Middle ||
                (e.Button == MouseButtons.Left && ModifierKeys == Keys.Alt))
            {
                _panning = true;
                _panStart = e.Location;
                _panOrigin = _pan;
                return;
            }

            var world = ScreenToWorld(e.Location);

            if (e.Button == MouseButtons.Left)
            {
                // Hit test port first
                var (node, port) = HitTestPort(world);
                if (node != null && port != null)
                {
                    if (port.Direction == PortDirection.Output)
                    {
                        _connectFromNode = node;
                        _connectFromPort = port;
                    }
                    else if (port.Direction == PortDirection.Input)
                    {
                        // Disconnect existing and start reconnecting
                        var existing = Graph.GetConnectionToPort(node.Id, port.Id);
                        if (existing != null)
                        {
                            var srcNode = Graph.FindNode(existing.FromNodeId);
                            var srcPort = Graph.FindPort(existing.FromNodeId, existing.FromPortId);
                            Graph.Disconnect(existing.Id);
                            _connectFromNode = srcNode;
                            _connectFromPort = srcPort;
                        }
                    }
                    Invalidate(); return;
                }

                // Hit test node body
                var hitNode = HitTestNode(world);
                if (hitNode != null)
                {
                    _draggingNode = hitNode;
                    _dragOffset = new Point(
                        (int)(world.X - hitNode.Position.X),
                        (int)(world.Y - hitNode.Position.Y));
                    // Bring to front
                    Graph.Nodes.Remove(hitNode);
                    Graph.Nodes.Add(hitNode);
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                var hitNode = HitTestNode(world);
                if (hitNode != null)
                    ShowNodeMenu(hitNode, e.Location);
                else
                    ShowAddMenu(e.Location, world);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _mouseScreen = e.Location;

            if (_panning)
            {
                _pan = new PointF(
                    _panOrigin.X + (e.X - _panStart.X),
                    _panOrigin.Y + (e.Y - _panStart.Y));
                Invalidate(); return;
            }

            if (_draggingNode != null)
            {
                var world = ScreenToWorld(e.Location);
                _draggingNode.Position = new Point(
                    (int)(world.X - _dragOffset.X),
                    (int)(world.Y - _dragOffset.Y));
                Invalidate(); return;
            }

            if (_connectFromNode != null) Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_panning) { _panning = false; return; }

            if (_draggingNode != null) { _draggingNode = null; Invalidate(); return; }

            if (_connectFromNode != null && _connectFromPort != null && e.Button == MouseButtons.Left)
            {
                var world = ScreenToWorld(e.Location);
                var (toNode, toPort) = HitTestPort(world);

                if (toNode != null && toPort != null &&
                    toPort.Direction == PortDirection.Input &&
                    toNode != _connectFromNode &&
                    toPort.DataType == _connectFromPort.DataType)
                {
                    Graph.Connect(_connectFromNode.Id, _connectFromPort.Id,
                                  toNode.Id, toPort.Id);
                }
                _connectFromNode = null;
                _connectFromPort = null;
                Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            float oldZoom = _zoom;
            _zoom = Math.Clamp(_zoom + e.Delta * 0.001f, ZoomMin, ZoomMax);

            // Zoom toward mouse position
            float scale = _zoom / oldZoom;
            _pan.X = e.X + (_pan.X - e.X) * scale;
            _pan.Y = e.Y + (_pan.Y - e.Y) * scale;
            Invalidate();
        }

        // ── Hit testing ───────────────────────────────────────────────────
        private ScriptNode? HitTestNode(PointF world)
        {
            for (int i = Graph.Nodes.Count - 1; i >= 0; i--)
            {
                var n = Graph.Nodes[i];
                var r = new RectangleF(n.Position.X, n.Position.Y, n.Size.Width, n.Size.Height);
                if (r.Contains(world)) return n;
            }
            return null;
        }

        private (ScriptNode? node, NodePort? port) HitTestPort(PointF world)
        {
            float threshold = PortRadius * 1.8f;
            foreach (var node in Graph.Nodes)
            {
                foreach (var port in node.Ports)
                {
                    var center = GetPortCenter(node, port);
                    float dx = world.X - center.X;
                    float dy = world.Y - center.Y;
                    if (MathF.Sqrt(dx * dx + dy * dy) <= threshold)
                        return (node, port);
                }
            }
            return (null, null);
        }

        // ── Context menus ─────────────────────────────────────────────────
        private void ShowAddMenu(Point screen, PointF world)
        {
            var menu = new ContextMenuStrip();
            menu.BackColor = Color.FromArgb(38, 38, 42);
            menu.ForeColor = Color.White;

            void AddItem(string cat, string label, Func<ScriptNode> factory)
            {
                ToolStripMenuItem? catItem = null;
                foreach (ToolStripItem item in menu.Items)
                    if (item.Text == cat) { catItem = item as ToolStripMenuItem; break; }
                if (catItem == null)
                {
                    catItem = new ToolStripMenuItem(cat) { ForeColor = Color.LightGray, Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
                    menu.Items.Add(catItem);
                }
                var child = new ToolStripMenuItem(label) { ForeColor = Color.Black };
                child.Click += (s, e) =>
                {
                    var node = factory();
                    node.Position = new Point((int)world.X, (int)world.Y);
                    Graph.AddNode(node);
                    Invalidate();
                };
                catItem.DropDownItems.Add(child);
            }

            // Events
            AddItem("🔴 Eventos", "Al Iniciar", () => new OnStartNode());
            AddItem("🔴 Eventos", "Al Hacer Clic", () => new OnClickNode());
            AddItem("🔴 Eventos", "Cada Frame", () => new OnUpdateNode());
            // Actions
            AddItem("🔵 Acciones", "Mover Objeto", () => new MoverObjetoNode());
            AddItem("🔵 Acciones", "Rotar Objeto", () => new RotarObjetoNode());
            AddItem("🔵 Acciones", "Escalar Objeto", () => new EscalarObjetoNode());
            AddItem("🔵 Acciones", "Imprimir", () => new ImprimirNode());
            // Conditions
            AddItem("🟡 Condiciones", "Si / Sino", () => new SiEntoncesNode());
            AddItem("🟡 Condiciones", "Comparar", () => new CompararNode());
            // Variables
            AddItem("🟢 Variables", "Número", () => new NumeroConstanteNode());
            AddItem("🟢 Variables", "Obtener Variable", () => new ObtenerVariableNode());
            AddItem("🟢 Variables", "Asignar Variable", () => new SetVariableNode());
            // Math
            AddItem("🟣 Matemáticas", "Sumar", () => new SumarNode());
            AddItem("🟣 Matemáticas", "Restar", () => new RestarNode());
            AddItem("🟣 Matemáticas", "Multiplicar", () => new MultiplicarNode());
            AddItem("🟣 Matemáticas", "Seno", () => new SinusNode());

            menu.Show(this, screen);
        }

        private void ShowNodeMenu(ScriptNode node, Point screen)
        {
            var menu = new ContextMenuStrip();
            menu.BackColor = Color.FromArgb(38, 38, 42);
            var del = new ToolStripMenuItem("🗑 Eliminar nodo") { ForeColor = Color.Salmon };
            del.Click += (s, e) => { Graph.RemoveNode(node.Id); Invalidate(); };
            menu.Items.Add(del);
            var dup = new ToolStripMenuItem("📋 Duplicar") { ForeColor = Color.White };
            dup.Click += (s, e) =>
            {
                // Simple clone by re-creating same type
                var clone = (ScriptNode)Activator.CreateInstance(node.GetType())!;
                clone.Position = new Point(node.Position.X + 30, node.Position.Y + 30);
                Graph.AddNode(clone); Invalidate();
            };
            menu.Items.Add(dup);
            menu.Show(this, screen);
        }
    }
}