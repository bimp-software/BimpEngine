using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Editor.Layouts
{
    public class CodingLayout : IEditorLayout
    {
        public string Name => "Coding";

        public void Build(
            Panel container,
            Dictionary<string, EditorView> views)
        {
            TableLayoutPanel layout = new TableLayoutPanel();

            layout.Dock = DockStyle.Fill;

            layout.ColumnCount = 2;
            layout.RowCount = 2;

            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));

            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 300));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 220));

            var scene = views["Scene"].Control;
            var inspector = views["Inspector"].Control;

            scene.Dock = DockStyle.Fill;
            inspector.Dock = DockStyle.Fill;

            layout.Controls.Add(scene, 0, 0);
            layout.Controls.Add(inspector, 1, 0);

            TabControl bottomTabs = new TabControl();

            bottomTabs.Dock = DockStyle.Fill;

            AddTab(bottomTabs, views["Console"]);
            AddTab(bottomTabs, views["Project"]);

            if (views.ContainsKey("Profiler"))
                AddTab(bottomTabs, views["Profiler"]);

            layout.Controls.Add(bottomTabs, 0, 1);

            layout.SetColumnSpan(bottomTabs, 2);

            container.Controls.Add(layout);
        }

        private void AddTab(
            TabControl tabControl,
            EditorView view)
        {
            TabPage page = new TabPage(view.Name);

            view.Control.Dock = DockStyle.Fill;

            page.Controls.Add(view.Control);

            tabControl.TabPages.Add(page);
        }
    }
}
