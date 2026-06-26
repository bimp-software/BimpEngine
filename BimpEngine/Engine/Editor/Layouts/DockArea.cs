using BimpEngine.Engine.Editor.Layouts.Custom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BimpEngine.Engine.Editor.Layouts
{
    public partial class DockArea : UserControl
    {
        private FlowLayoutPanel pnlTabs;
        private Panel pnlContent;

        private Dictionary<Button, Control> views =
            new Dictionary<Button, Control>();

        public DockArea()
        {
            pnlTabs = new FlowLayoutPanel();
            pnlContent = new Panel();
            this.BorderStyle = BorderStyle.FixedSingle;

            pnlTabs.Height = 30;
            pnlTabs.Dock = DockStyle.Top;

            pnlTabs.BackColor =
                Color.FromArgb(48, 48, 48);

            pnlContent.Dock = DockStyle.Fill;

            Controls.Add(pnlContent);
            Controls.Add(pnlTabs);
        }

        public void AddView(
    UserControl view,
    string title)
        {
            Button tab = new Button();

            tab.Text = title;

            tab.FlatStyle = FlatStyle.Flat;

            tab.FlatAppearance.BorderSize = 0;

            tab.BackColor =
                Color.FromArgb(40, 40, 40);

            tab.ForeColor = Color.Gainsboro;

            tab.Height = 28;
            tab.Width = 120;

            tab.Click += (s, e) =>
            {
                ShowView(view);
            };

            pnlTabs.Controls.Add(tab);

            views.Add(tab, view);

            if (views.Count == 1)
                ShowView(view);
        }

        private void ShowView(Control view)
        {
            pnlContent.Controls.Clear();

            view.Dock = DockStyle.Fill;

            pnlContent.Controls.Add(view);
        }
    }
}
