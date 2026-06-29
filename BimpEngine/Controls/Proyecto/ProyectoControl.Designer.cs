namespace BimpEngine.Controls.Proyecto
{
    partial class ProyectoControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProyectoControl));
            panel1 = new Panel();
            panel2 = new Panel();
            panel4 = new Panel();
            pictureBox2 = new PictureBox();
            tbBuscarHerencia = new TextBox();
            pictureBox1 = new PictureBox();
            panel3 = new Panel();
            _tree = new TreeView();
            _icons = new ImageList(components);
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1168, 30);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel4);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1141, 30);
            panel2.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(40, 40, 40);
            panel4.Controls.Add(pictureBox2);
            panel4.Controls.Add(tbBuscarHerencia);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(1141, 30);
            panel4.TabIndex = 1;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(3, 5);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(25, 21);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // tbBuscarHerencia
            // 
            tbBuscarHerencia.BackColor = Color.FromArgb(48, 48, 48);
            tbBuscarHerencia.BorderStyle = BorderStyle.None;
            tbBuscarHerencia.Font = new Font("Segoe UI", 12F);
            tbBuscarHerencia.ForeColor = Color.White;
            tbBuscarHerencia.Location = new Point(34, 5);
            tbBuscarHerencia.Multiline = true;
            tbBuscarHerencia.Name = "tbBuscarHerencia";
            tbBuscarHerencia.Size = new Size(590, 21);
            tbBuscarHerencia.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(1141, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(27, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(40, 40, 40);
            panel3.Controls.Add(_tree);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 30);
            panel3.Name = "panel3";
            panel3.Size = new Size(1168, 245);
            panel3.TabIndex = 4;
            // 
            // _tree
            // 
            _tree.BackColor = Color.FromArgb(40, 40, 40);
            _tree.BorderStyle = BorderStyle.None;
            _tree.Dock = DockStyle.Fill;
            _tree.Font = new Font("Segoe UI", 10F);
            _tree.ForeColor = Color.White;
            _tree.FullRowSelect = true;
            _tree.HideSelection = false;
            _tree.Indent = 16;
            _tree.ItemHeight = 22;
            _tree.LineColor = Color.White;
            _tree.Location = new Point(0, 0);
            _tree.Name = "_tree";
            _tree.Size = new Size(1168, 245);
            _tree.TabIndex = 0;
            _tree.BeforeExpand += _tree_BeforeExpand;
            _tree.DrawNode += _tree_DrawNode;
            _tree.DoubleClick += _tree_DoubleClick;
            _tree.KeyDown += _tree_KeyDown;
            _tree.MouseUp += _tree_MouseUp;
            // 
            // _icons
            // 
            _icons.ColorDepth = ColorDepth.Depth32Bit;
            _icons.ImageSize = new Size(16, 16);
            _icons.TransparentColor = Color.Transparent;
            // 
            // ProyectoControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(48, 48, 48);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(panel3);
            Controls.Add(panel1);
            Name = "ProyectoControl";
            Size = new Size(1168, 275);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Panel panel3;
        private Panel panel4;
        private PictureBox pictureBox2;
        private TextBox tbBuscarHerencia;
        private TreeView _tree;
        private ImageList _icons;
    }
}
