using BimpEngine.Engine.Core.Interface;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BimpEngine.Controls.Inspector.Componentes
{
    public partial class VariableControl : UserControl, IInspectorComponent
    {
        private Objetos objeto;

        public VariableControl()
        {
            InitializeComponent();
        }

        public void SetObject(Objetos obj)
        {
            objeto = obj;
            tbName.Text = objeto.Name;
            cbTag.Text = objeto.Tag;
            cbLayer.SelectedItem = objeto.Layer;
            cbEnabled.Checked = objeto.Enabled;
        }
    }
}
