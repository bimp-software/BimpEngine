using BimpEngine.Engine.Debug;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BimpEngine.Controls.Consola
{
    public partial class ConsolaControl : UserControl
    {


        public ConsolaControl()
        {
            InitializeComponent();
            Engine.Debug.Console.OnLog += OnLog;
        }

        private void OnLog(LogEntry entry)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnLog(entry));
                return;
            }

            string tipo = entry.Type switch
            {
                LogType.Warning => "⚠ Warning",
                LogType.Error => "✕ Error",
                _ => "ℹ Info"
            };

            Color color = entry.Type switch
            {
                LogType.Warning => Color.FromArgb(255, 200, 60),
                LogType.Error => Color.FromArgb(255, 80, 80),
                _ => Color.FromArgb(200, 200, 200)
            };

            var item = new ListViewItem(entry.Time.ToString("HH:mm:ss"))
            {
                ForeColor = color,
                Tag = entry
            };

            item.SubItems.Add(tipo);
            item.SubItems.Add(entry.Source ?? "Script");
            item.SubItems.Add(entry.Message);

            lvConsola.Items.Add(item);
            item.EnsureVisible();

            if (!string.IsNullOrEmpty(tbBuscarError.Text))
                FiltrarMensajes();
        }

        public void LimpiarConsola() => lvConsola.Items.Clear();

        private void FiltrarMensajes()
        {
            string filtro = tbBuscarError.Text.ToLower();

            foreach (ListViewItem item in lvConsola.Items)
            {
                bool visible = string.IsNullOrEmpty(filtro) ||
                    item.SubItems[3].Text.ToLower().Contains(filtro) ||
                    item.SubItems[1].Text.ToLower().Contains(filtro) ||
                    item.SubItems[2].Text.ToLower().Contains(filtro);

                item.ForeColor = visible
                    ? (item.Tag is LogEntry e ? e.Type switch
                    {
                        LogType.Warning => Color.FromArgb(255, 200, 60),
                        LogType.Error => Color.FromArgb(255, 80, 80),
                        _ => Color.FromArgb(200, 200, 200)
                    } : Color.White)
                    : Color.FromArgb(60, 60, 60);
            }
        }

        private void tbBuscarError_TextChanged(object sender, EventArgs e)
        {
            FiltrarMensajes();
        }
    }
}
