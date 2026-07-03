namespace BimpEngine.Controls.Consola
{
    partial class ConsolaControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsolaControl));
            panel1 = new Panel();
            panel2 = new Panel();
            panel4 = new Panel();
            pictureBox2 = new PictureBox();
            tbBuscarError = new TextBox();
            pictureBox1 = new PictureBox();
            panel3 = new Panel();
            lvConsola = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
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
            panel1.Size = new Size(1177, 30);
            panel1.TabIndex = 4;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel4);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1150, 30);
            panel2.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(40, 40, 40);
            panel4.Controls.Add(pictureBox2);
            panel4.Controls.Add(tbBuscarError);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(1150, 30);
            panel4.TabIndex = 0;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(6, 5);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(25, 21);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 5;
            pictureBox2.TabStop = false;
            // 
            // tbBuscarError
            // 
            tbBuscarError.BackColor = Color.FromArgb(48, 48, 48);
            tbBuscarError.BorderStyle = BorderStyle.None;
            tbBuscarError.Font = new Font("Segoe UI", 12F);
            tbBuscarError.ForeColor = Color.White;
            tbBuscarError.Location = new Point(37, 5);
            tbBuscarError.Multiline = true;
            tbBuscarError.Name = "tbBuscarError";
            tbBuscarError.Size = new Size(889, 21);
            tbBuscarError.TabIndex = 4;
            tbBuscarError.TextChanged += tbBuscarError_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(1150, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(27, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(40, 40, 40);
            panel3.Controls.Add(lvConsola);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 30);
            panel3.Name = "panel3";
            panel3.Size = new Size(1177, 269);
            panel3.TabIndex = 5;
            // 
            // lvConsola
            // 
            lvConsola.BackColor = Color.FromArgb(40, 40, 40);
            lvConsola.BorderStyle = BorderStyle.None;
            lvConsola.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            lvConsola.Dock = DockStyle.Fill;
            lvConsola.ForeColor = Color.White;
            lvConsola.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvConsola.LabelWrap = false;
            lvConsola.Location = new Point(0, 0);
            lvConsola.MultiSelect = false;
            lvConsola.Name = "lvConsola";
            lvConsola.Size = new Size(1177, 269);
            lvConsola.TabIndex = 0;
            lvConsola.UseCompatibleStateImageBehavior = false;
            lvConsola.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Hora";
            columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Tipo";
            columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Origen";
            columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Mensaje";
            columnHeader4.Width = 500;
            // 
            // ConsolaControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(48, 48, 48);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(panel3);
            Controls.Add(panel1);
            Name = "ConsolaControl";
            Size = new Size(1177, 299);
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
        private TextBox tbBuscarError;
        private ListView lvConsola;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
    }
}
