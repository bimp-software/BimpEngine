using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Editor
{
    public class EditorView
    {
        public string Name { get; set; }

        public UserControl Control { get; set; }

        public bool Visible { get; set; } = true;

        public EditorView(string name, UserControl control)
        {
            Name = name;
            Control = control;
        }
    }
}
