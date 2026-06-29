namespace BimpEngine.Vista
{
    partial class frmNuevoProyecto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnCrear = new Button();
            txtNombre = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtUbicacion = new TextBox();
            btnExaminar = new Button();
            lblPreview = new TextBox();
            btnSalir = new Button();
            pnlError = new Panel();
            lblError = new Label();
            pictureBox1 = new PictureBox();
            pnlError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnCrear
            // 
            btnCrear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCrear.BackColor = Color.RoyalBlue;
            btnCrear.FlatAppearance.BorderSize = 0;
            btnCrear.FlatStyle = FlatStyle.Flat;
            btnCrear.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCrear.Location = new Point(567, 216);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(153, 44);
            btnCrear.TabIndex = 5;
            btnCrear.Text = "Crear Proyecto";
            btnCrear.UseVisualStyleBackColor = false;
            btnCrear.Click += btnCrear_Click;
            // 
            // txtNombre
            // 
            txtNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNombre.BackColor = Color.FromArgb(48, 48, 48);
            txtNombre.BorderStyle = BorderStyle.None;
            txtNombre.Font = new Font("Segoe UI", 12F);
            txtNombre.ForeColor = Color.White;
            txtNombre.Location = new Point(102, 19);
            txtNombre.Multiline = true;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(618, 25);
            txtNombre.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(25, 19);
            label1.Name = "label1";
            label1.Size = new Size(71, 21);
            label1.TabIndex = 8;
            label1.Text = "Nombre:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(15, 64);
            label2.Name = "label2";
            label2.Size = new Size(81, 21);
            label2.TabIndex = 9;
            label2.Text = "Ubicacion:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(17, 109);
            label3.Name = "label3";
            label3.Size = new Size(79, 21);
            label3.TabIndex = 10;
            label3.Text = "Ruta final:";
            // 
            // txtUbicacion
            // 
            txtUbicacion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUbicacion.BackColor = Color.FromArgb(48, 48, 48);
            txtUbicacion.BorderStyle = BorderStyle.None;
            txtUbicacion.Font = new Font("Segoe UI", 12F);
            txtUbicacion.ForeColor = Color.White;
            txtUbicacion.Location = new Point(102, 64);
            txtUbicacion.Multiline = true;
            txtUbicacion.Name = "txtUbicacion";
            txtUbicacion.ReadOnly = true;
            txtUbicacion.Size = new Size(560, 25);
            txtUbicacion.TabIndex = 11;
            // 
            // btnExaminar
            // 
            btnExaminar.BackColor = Color.FromArgb(45, 45, 45);
            btnExaminar.FlatAppearance.BorderColor = Color.Gray;
            btnExaminar.FlatStyle = FlatStyle.Flat;
            btnExaminar.Font = new Font("Segoe UI", 10F);
            btnExaminar.ForeColor = Color.Gray;
            btnExaminar.Location = new Point(668, 64);
            btnExaminar.Name = "btnExaminar";
            btnExaminar.Size = new Size(52, 25);
            btnExaminar.TabIndex = 12;
            btnExaminar.Text = "...";
            btnExaminar.UseVisualStyleBackColor = false;
            btnExaminar.Click += btnExaminar_Click;
            // 
            // lblPreview
            // 
            lblPreview.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblPreview.BackColor = Color.FromArgb(45, 45, 45);
            lblPreview.BorderStyle = BorderStyle.None;
            lblPreview.Font = new Font("Segoe UI", 12F);
            lblPreview.ForeColor = SystemColors.MenuHighlight;
            lblPreview.Location = new Point(102, 109);
            lblPreview.Multiline = true;
            lblPreview.Name = "lblPreview";
            lblPreview.ReadOnly = true;
            lblPreview.Size = new Size(618, 25);
            lblPreview.TabIndex = 13;
            lblPreview.TabStop = false;
            lblPreview.Text = "Hola Mundo}";
            // 
            // btnSalir
            // 
            btnSalir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSalir.BackColor = Color.FromArgb(45, 45, 45);
            btnSalir.FlatAppearance.BorderColor = Color.Gray;
            btnSalir.FlatAppearance.BorderSize = 2;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSalir.ForeColor = Color.Gray;
            btnSalir.Location = new Point(408, 216);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(153, 44);
            btnSalir.TabIndex = 6;
            btnSalir.Text = "Cancelar";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += btnSalir_Click;
            // 
            // pnlError
            // 
            pnlError.Controls.Add(lblError);
            pnlError.Controls.Add(pictureBox1);
            pnlError.Location = new Point(15, 140);
            pnlError.Name = "pnlError";
            pnlError.Size = new Size(705, 65);
            pnlError.TabIndex = 14;
            // 
            // lblError
            // 
            lblError.Dock = DockStyle.Fill;
            lblError.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblError.Location = new Point(65, 0);
            lblError.Margin = new Padding(4);
            lblError.Name = "lblError";
            lblError.Size = new Size(640, 65);
            lblError.TabIndex = 1;
            lblError.Text = "label4";
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(65, 65);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // frmNuevoProyecto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(732, 272);
            Controls.Add(pnlError);
            Controls.Add(lblPreview);
            Controls.Add(btnExaminar);
            Controls.Add(txtUbicacion);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtNombre);
            Controls.Add(btnSalir);
            Controls.Add(btnCrear);
            ForeColor = Color.White;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmNuevoProyecto";
            Text = "frmNuevoProyecto";
            pnlError.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCrear;
        private TextBox txtNombre;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtUbicacion;
        private Button btnExaminar;
        private TextBox lblPreview;
        private Button btnSalir;
        private Panel pnlError;
        private PictureBox pictureBox1;
        private Label lblError;
    }
}