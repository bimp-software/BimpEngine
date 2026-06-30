using BimpEngine.Engine.Core.Interface;
using BimpEngine.Engine.Project;
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
        private bool _cargando;

        public VariableControl()
        {
            InitializeComponent();

            tbName.TextChanged += (s, e) =>
            {
                if (_cargando || objeto == null) return;

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

            cbTag.SelectedIndexChanged += (s, e) =>
            {
                if (_cargando || objeto == null) return;
                if (cbTag.SelectedItem is string tag)
                    objeto.Tag = tag;
                OnObjectModified?.Invoke(objeto);
            };

            cbLayer.SelectedIndexChanged += (s, e) =>
            {
                if (_cargando || objeto == null) return;
                if (cbLayer.SelectedItem is LayerEntry entry)
                    objeto.Layer = entry.Index;
                OnObjectModified?.Invoke(objeto);
            };
        }

        public void SetObject(Objetos obj)
        {
            objeto = obj;
            _cargando = true;

            tbName.Text = obj.Name;
            _nombreAnterior = obj.Name;
            cbEnabled.Checked = obj.Enabled;

            CargarTagsYLayers();

            cbTag.SelectedItem = obj.Tag;
            if (cbTag.SelectedIndex < 0) cbTag.SelectedIndex = 0;

            foreach (LayerEntry l in cbLayer.Items)
            {
                if (l.Index == obj.Layer)
                {
                    cbLayer.SelectedItem = l;
                    break;
                }
            }
            if (cbLayer.SelectedIndex < 0) cbLayer.SelectedIndex = 0;

            _cargando = false;
        }

        public void Refresh(Objetos obj)
        {
            if (obj == null) return;
            _cargando = true;
            tbName.Text = obj.Name;
            cbEnabled.Checked = obj.Enabled;
            _cargando = false;
        }

        public void CargarTagsYLayers()
        {
            _cargando = true;

            cbTag.Items.Clear();
            foreach (var t in TagLayerManager.Current.AllTags)
                cbTag.Items.Add(t);

            cbLayer.Items.Clear();
            foreach (var l in TagLayerManager.Current.AllLayers)
                cbLayer.Items.Add(l);

            cbLayer.DisplayMember = "Name";

            _cargando = false;
        }
    }
}
