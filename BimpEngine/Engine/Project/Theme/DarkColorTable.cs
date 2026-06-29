using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project.Theme
{
    public class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(60, 100, 180);
        public override Color MenuItemBorder => Color.FromArgb(60, 100, 180);
        public override Color MenuBorder => Color.FromArgb(55, 55, 60);
        public override Color ToolStripDropDownBackground => Color.FromArgb(40, 40, 42);
        public override Color ImageMarginGradientBegin => Color.FromArgb(40, 40, 42);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(40, 40, 42);
        public override Color ImageMarginGradientEnd => Color.FromArgb(40, 40, 42);
    }
}
