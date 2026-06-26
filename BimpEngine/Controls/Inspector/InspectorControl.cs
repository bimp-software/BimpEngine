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
        public InspectorControl()
        {
            InitializeComponent();
        }

        public void ClearInspector()
        {
            flpContenedor.Controls.Clear();
       
        }

        private void AddComponent(UserControl control)
        {
            control.Dock = DockStyle.Top;
            flpContenedor.Controls.Add(control);

            flpContenedor.Controls.SetChildIndex(btnAddComponent, flpContenedor.Controls.Count - 1);
        }

        public void ShowObject(Objetos obj)
        {
            ClearInspector();

            if (obj == null)
                return;

            //--------------------------------

            VariableControl variable = new VariableControl();
            variable.SetObject(obj);

            AddComponent(variable);

        }
    }
}
