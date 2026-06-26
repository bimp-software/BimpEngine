using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Editor.Layouts
{
    public class LevelDesignerLayout : IEditorLayout
    {
        public string Name => "Level Designer";

        public void Build(
            Panel container,
            Dictionary<string, EditorView> views)
        {
            TableLayoutPanel layout = new TableLayoutPanel();

            layout.Dock = DockStyle.Fill;

            layout.ColumnCount = 3;
            layout.RowCount = 2;

            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 250));

            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));

            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 350));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 220));

            //------------------------------------------------

            var hierarchy = views["Hierarchy"].Control;
            var scene = views["Scene"].Control;
            var inspector = views["Inspector"].Control;

            hierarchy.Dock = DockStyle.Fill;
            scene.Dock = DockStyle.Fill;
            inspector.Dock = DockStyle.Fill;

            //------------------------------------------------

            layout.Controls.Add(hierarchy, 0, 0);
            layout.Controls.Add(scene, 1, 0);
            layout.Controls.Add(inspector, 2, 0);

            //------------------------------------------------

            TabControl bottomTabs = new TabControl();

            bottomTabs.Dock = DockStyle.Fill;

            if (views.ContainsKey("Project"))
                AddTab(bottomTabs, views["Project"]);

            if (views.ContainsKey("Console"))
                AddTab(bottomTabs, views["Console"]);

            if (views.ContainsKey("Profiler"))
                AddTab(bottomTabs, views["Profiler"]);

            if (views.ContainsKey("Game"))
                AddTab(bottomTabs, views["Game"]);

            layout.Controls.Add(bottomTabs, 0, 1);

            layout.SetColumnSpan(bottomTabs, 3);

            //------------------------------------------------

            container.Controls.Add(layout);
        }

        private void AddTab(
            TabControl tabControl,
            EditorView view)
        {
            if (!view.Visible)
                return;

            TabPage page = new TabPage(view.Name);

            view.Control.Dock = DockStyle.Fill;

            page.Controls.Add(view.Control);

            tabControl.TabPages.Add(page);
        }
    } 
}
