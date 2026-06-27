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

        private string _nombreAnterior;

        public event Action<Objetos> OnObjectModified;

        public VariableControl()
        {
            InitializeComponent();

            tbName.TextChanged += (s, e) =>
            {
                if (objeto == null) return;

                if (string.IsNullOrWhiteSpace(tbName.Text))
                    return;

                objeto.Name = tbName.Text;
                OnObjectModified?.Invoke(objeto);
            };

            tbName.Leave += (s, e) =>
            {
                if (objeto == null) return;

                if (string.IsNullOrWhiteSpace(tbName.Text))
                {
                    tbName.Text = _nombreAnterior;
                    objeto.Name = _nombreAnterior;
                    OnObjectModified?.Invoke(objeto);
                }
                else
                {
                    _nombreAnterior = tbName.Text;
                }
            };

            cbEnabled.CheckedChanged += (s, e) =>
            {
                if (objeto == null) return;
                objeto.Enabled = cbEnabled.Checked;
                OnObjectModified?.Invoke(objeto);
            };
        }

        public void SetObject(Objetos obj)
        {
            objeto = obj;
            tbName.Text = objeto.Name;
            _nombreAnterior = obj.Name;
            cbTag.Text = objeto.Tag;
            cbLayer.SelectedItem = objeto.Layer;
            cbEnabled.Checked = objeto.Enabled;
            Refresh(objeto);
        }

        public void Refresh(Objetos obj)
        {
            if (obj == null) return;
            tbName.Text = obj.Name;
            cbEnabled.Checked = obj.Enabled;
        }
    }
}
