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
        public event Action<PrimitiveType> OnCreatePrimitive;

        public event Action<Objetos> OnObjectModified;

        private Button btnAddComponent;

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

            // Botón creado completamente por código
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

            flpContenedor.Controls.Add(btnAddComponent);

            flpContenedor.Resize += (s, e) =>
            {
                int w = flpContenedor.ClientSize.Width - 6;
                btnAddComponent.Width = w;

                foreach (Control c in flpContenedor.Controls)
                    c.Width = w;
            };
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
            if (flpContenedor.Controls.Count > 0 && flpContenedor.Controls[0] != btnAddComponent)
            {
                var separador = new Panel
                {
                    Height = 4,
                    BackColor = Color.FromArgb(40, 40, 40)
                };
                separador.Width = flpContenedor.ClientSize.Width - 6;
                flpContenedor.Controls.Add(separador);
            }

            if (control is VariableControl vc)
                vc.OnObjectModified += (obj) => OnObjectModified?.Invoke(obj);

            if (control is TransformControl tc)
                tc.OnObjectModified += (obj) => OnObjectModified?.Invoke(obj);

            control.Width = flpContenedor.ClientSize.Width - 6;
            flpContenedor.Controls.Add(control);
            flpContenedor.Controls.SetChildIndex(btnAddComponent, flpContenedor.Controls.Count - 1);
        }

        public void ShowObject(Objetos obj)
        {
            ClearInspector();

            if (obj == null)
                return;

            VariableControl variable = new VariableControl();
            variable.SetObject(obj);
            AddComponent(variable);

            TransformControl transform = new TransformControl();
            transform.SetObject(obj);
            AddComponent(transform);

            //// Componentes específicos según el tipo
            //if (obj is CameraObject camera)
            //{
            //    var camControl = new CameraControl();
            //    camControl.SetObject(camera);
            //    AddComponent(camControl);
            //}
            //else if (obj is PrimitiveObject)
            //{
            //    var meshControl = new MeshRendererControl();
            //    meshControl.SetObject(obj);
            //    AddComponent(meshControl);
            //}
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
