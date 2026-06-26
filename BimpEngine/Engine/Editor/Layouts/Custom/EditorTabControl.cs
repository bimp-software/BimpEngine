using System.Drawing;
using System.Windows.Forms;

namespace BimpEngine.Engine.Editor.Layouts.Custom
{
    public class EditorTabControl : TabControl
    {
        private readonly Color BackgroundColor =
            Color.FromArgb(48, 48, 48);

        private readonly Color TabColor =
            Color.FromArgb(40, 40, 40);

        private readonly Color SelectedTabColor =
            Color.FromArgb(55, 55, 55);

        private readonly Color TextColor =
            Color.Gainsboro;

        public EditorTabControl()
        {
            DrawMode = TabDrawMode.OwnerDrawFixed;

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(120, 28);

            BackColor = Color.FromArgb(48, 48, 48);

            Appearance = TabAppearance.Normal;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(48, 48, 48));

            base.OnPaint(e);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            Graphics g = e.Graphics;

            Rectangle rect = GetTabRect(e.Index);

            bool selected = SelectedIndex == e.Index;

            using (SolidBrush brush = new SolidBrush(
                selected
                ? Color.FromArgb(55, 55, 55)
                : Color.FromArgb(40, 40, 40)))
            {
                g.FillRectangle(brush, rect);
            }

            if (selected)
            {
                g.FillRectangle(
                    new SolidBrush(Color.FromArgb(0, 122, 204)),
                    rect.X,
                    rect.Bottom - 2,
                    rect.Width,
                    2);
            }

            TextRenderer.DrawText(
                g,
                TabPages[e.Index].Text,
                Font,
                rect,
                Color.Gainsboro,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(48, 48, 48));
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control is TabPage page)
            {
                page.BackColor =
                    Color.FromArgb(48, 48, 48);

                page.ForeColor =
                    Color.Gainsboro;
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (!DesignMode)
            {
                this.Multiline = true;
                this.Multiline = false;
            }
        }
    }
}