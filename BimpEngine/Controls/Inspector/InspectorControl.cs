using BimpEngine.Controls.Inspector.Componentes;
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
                btnAddComponent.Width = flpContenedor.ClientSize.Width - 6;
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
        }
    }
}
