namespace BimpEngine.Controls.Herencia
{
    partial class HerenciaControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HerenciaControl));
            panel1 = new Panel();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            pnlContenedor = new Panel();
            ListHerencia = new TreeView();
            panel3 = new Panel();
            pictureBox2 = new PictureBox();
            tbBuscarHerencia = new TextBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnlContenedor.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(255, 30);
            panel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(70, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(158, 30);
            panel2.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(228, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(27, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(40, 40, 40);
            label1.Dock = DockStyle.Left;
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(5, 0, 0, 0);
            label1.Size = new Size(70, 30);
            label1.TabIndex = 0;
            label1.Text = "Herencia";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlContenedor
            // 
            pnlContenedor.Controls.Add(ListHerencia);
            pnlContenedor.Controls.Add(panel3);
            pnlContenedor.Dock = DockStyle.Fill;
            pnlContenedor.Location = new Point(0, 30);
            pnlContenedor.Name = "pnlContenedor";
            pnlContenedor.Size = new Size(255, 592);
            pnlContenedor.TabIndex = 2;
            // 
            // ListHerencia
            // 
            ListHerencia.BackColor = Color.FromArgb(40, 40, 40);
            ListHerencia.BorderStyle = BorderStyle.None;
            ListHerencia.Dock = DockStyle.Fill;
            ListHerencia.ForeColor = Color.White;
            ListHerencia.FullRowSelect = true;
            ListHerencia.ItemHeight = 20;
            ListHerencia.LineColor = Color.White;
            ListHerencia.Location = new Point(0, 34);
            ListHerencia.Name = "ListHerencia";
            ListHerencia.ShowLines = false;
            ListHerencia.Size = new Size(255, 558);
            ListHerencia.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(40, 40, 40);
            panel3.Controls.Add(pictureBox2);
            panel3.Controls.Add(tbBuscarHerencia);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(255, 34);
            panel3.TabIndex = 1;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(3, 1);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(25, 30);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // tbBuscarHerencia
            // 
            tbBuscarHerencia.BackColor = Color.FromArgb(48, 48, 48);
            tbBuscarHerencia.BorderStyle = BorderStyle.None;
            tbBuscarHerencia.Font = new Font("Segoe UI", 12F);
            tbBuscarHerencia.ForeColor = Color.White;
            tbBuscarHerencia.Location = new Point(32, 5);
            tbBuscarHerencia.Multiline = true;
            tbBuscarHerencia.Name = "tbBuscarHerencia";
            tbBuscarHerencia.Size = new Size(220, 25);
            tbBuscarHerencia.TabIndex = 0;
            // 
            // HerenciaControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(48, 48, 48);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlContenedor);
            Controls.Add(panel1);
            Name = "HerenciaControl";
            Size = new Size(255, 622);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnlContenedor.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Label label1;
        private Panel pnlContenedor;
        private TreeView ListHerencia;
        private Panel panel3;
        private PictureBox pictureBox2;
        private TextBox tbBuscarHerencia;
    }
}
