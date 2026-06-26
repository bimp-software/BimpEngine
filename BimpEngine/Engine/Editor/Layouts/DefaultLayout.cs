using BimpEngine.Engine.Editor.Layouts.Custom;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Editor.Layouts
{
    public class DefaultLayout : IEditorLayout
    {
        public string Name => "Default";

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
                new ColumnStyle(SizeType.Absolute, 300));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 250));

            //------------------------------------------------

            views["Hierarchy"].Control.Dock =
                DockStyle.Fill;

            views["Scene"].Control.Dock =
                DockStyle.Fill;

            views["Inspector"].Control.Dock =
                DockStyle.Fill;

            //------------------------------------------------

            DockArea bottomDock = new DockArea();

            bottomDock.Dock = DockStyle.Fill;

            bottomDock.AddView(
                views["Project"].Control,
                views["Project"].Name);

            bottomDock.AddView(
                views["Console"].Control,
                views["Console"].Name);

            if (views.ContainsKey("Profiler"))
            {
                bottomDock.AddView(
                    views["Profiler"].Control,
                    views["Profiler"].Name);
            }

            //------------------------------------------------

            layout.Controls.Add(
                views["Hierarchy"].Control,
                0,
                0);

            layout.Controls.Add(
                views["Scene"].Control,
                1,
                0);

            layout.Controls.Add(
                views["Inspector"].Control,
                2,
                0);

            layout.Controls.Add(
                bottomDock,
                0,
                1);

            layout.SetColumnSpan(
                bottomDock,
                3);

            container.Controls.Add(layout);
        }
    }
}
