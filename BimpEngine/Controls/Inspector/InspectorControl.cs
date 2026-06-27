using BimpEngine.Controls.Inspector.Componentes;
using BimpEngine.Engine.Core.Interface;
using BimpEngine.Engine.Entities;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BimpEngine.Controls.Inspector
{
    public partial class InspectorControl : UserControl
    {
        private Objetos _objetoActual;
        private Button btnAddComponent;

        public event Action<Objetos> OnObjectModified;
        public event Action<PrimitiveType> OnCreatePrimitive;
        public event Action<Objetos, Objetos> OnChildCreated;

        public InspectorControl()
        {
            InitializeComponent();

            this.AutoSize = false;
            this.MinimumSize = Size.Empty;
            this.MaximumSize = Size.Empty;

            flpContenedor.Dock = DockStyle.Fill;
            flpContenedor.FlowDirection = FlowDirection.TopDown;
            flpContenedor.WrapContents = false;
            flpContenedor.AutoScroll = true;

            // Botón creado por código
            btnAddComponent = new Button
            {
                Text = "Agregar Componente",
                Height = 38,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddComponent.FlatAppearance.BorderSize = 0;
            btnAddComponent.Click += (s, e) => AbrirMenuComponentes();

            flpContenedor.Controls.Add(btnAddComponent);

            flpContenedor.Resize += (s, e) =>
            {
                int w = flpContenedor.ClientSize.Width - 6;
                foreach (Control c in flpContenedor.Controls)
                    c.Width = w;
            };
        }

        private void AbrirMenuComponentes()
        {
            if (_objetoActual == null) return;

            var frm = new frmAddComponent(_objetoActual);
            var pos = PointToScreen(new Point(0, btnAddComponent.Bottom));
            frm.Location = pos;

            frm.OnComponentSelected += (nombre) => AgregarComponentePorNombre(nombre);
            frm.Show();
        }

        private void AgregarComponentePorNombre(string nombre)
        {
            UserControl control = nombre switch
            {
                "Mueblería" => new FurnitureControl(),
                _ => null
            };

            if (control == null) return;

            if (control is IInspectorComponent comp)
                comp.SetObject(_objetoActual);

            AddComponent(control);
        }

        public void ClearInspector()
        {
            var toRemove = flpContenedor.Controls
                .Cast<Control>()
                .Where(c => c != btnAddComponent)
                .ToList();

            foreach (var c in toRemove)
                flpContenedor.Controls.Remove(c);
        }

        private void AddComponent(UserControl control)
        {
            if (control is VariableControl vc)
                vc.OnObjectModified += (obj) => OnObjectModified?.Invoke(obj);

            if (control is TransformControl tc)
                tc.OnObjectModified += (obj) => OnObjectModified?.Invoke(obj);

            if (control is FurnitureControl fc)
                fc.OnChildCreated += (hijo, padre) => OnChildCreated?.Invoke(hijo, padre);

            int w = flpContenedor.ClientSize.Width - 6;
            control.Width = w;
            flpContenedor.Controls.Add(control);
            flpContenedor.Controls.SetChildIndex(btnAddComponent, flpContenedor.Controls.Count - 1);
        }

        public void ShowObject(Objetos obj)
        {
            _objetoActual = obj;
            ClearInspector();

            if (obj == null) return;

            var variable = new VariableControl();
            variable.SetObject(obj);
            AddComponent(variable);

            var transform = new TransformControl();
            transform.SetObject(obj);
            AddComponent(transform);
        }

        public void RefreshObject(Objetos obj)
        {
            if (obj == null) return;

            foreach (Control c in flpContenedor.Controls)
            {
                if (c is IInspectorComponent comp)
                    comp.Refresh(obj);
            }
        }
    }
}
