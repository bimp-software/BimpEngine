namespace BimpEngine.Vista
{
    partial class frmLauncher
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
            panel1 = new Panel();
            btnAbrir = new Button();
            btnNew = new Button();
            label2 = new Label();
            label1 = new Label();
            panel2 = new Panel();
            btnOpen = new Button();
            label3 = new Label();
            lvRecent = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnAbrir);
            panel1.Controls.Add(btnNew);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(245, 514);
            panel1.TabIndex = 0;
            // 
            // btnAbrir
            // 
            btnAbrir.BackColor = Color.FromArgb(55, 55, 60);
            btnAbrir.FlatAppearance.BorderSize = 0;
            btnAbrir.FlatStyle = FlatStyle.Flat;
            btnAbrir.Font = new Font("Segoe UI", 12F);
            btnAbrir.Location = new Point(24, 230);
            btnAbrir.Name = "btnAbrir";
            btnAbrir.Size = new Size(197, 57);
            btnAbrir.TabIndex = 3;
            btnAbrir.Text = "Abrir Proyecto";
            btnAbrir.UseVisualStyleBackColor = false;
            btnAbrir.Click += btnAbrir_Click;
            // 
            // btnNew
            // 
            btnNew.BackColor = Color.RoyalBlue;
            btnNew.FlatAppearance.BorderSize = 0;
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNew.Location = new Point(24, 167);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(197, 57);
            btnNew.TabIndex = 2;
            btnNew.Text = "Nuevo Proyecto";
            btnNew.UseVisualStyleBackColor = false;
            btnNew.Click += btnNew_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(24, 123);
            label2.Name = "label2";
            label2.Size = new Size(162, 21);
            label2.TabIndex = 1;
            label2.Text = "Motor de VideoJuego.";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 25F);
            label1.Location = new Point(24, 67);
            label1.Name = "label1";
            label1.Size = new Size(197, 46);
            label1.TabIndex = 0;
            label1.Text = "BimpEngine";
            // 
            // panel2
            // 
            panel2.Controls.Add(btnOpen);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(lvRecent);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(245, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(596, 514);
            panel2.TabIndex = 1;
            // 
            // btnOpen
            // 
            btnOpen.BackColor = Color.RoyalBlue;
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.FlatStyle = FlatStyle.Flat;
            btnOpen.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOpen.Location = new Point(420, 448);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(153, 44);
            btnOpen.TabIndex = 4;
            btnOpen.Text = "Abrir Seleccion";
            btnOpen.UseVisualStyleBackColor = false;
            btnOpen.Click += btnOpen_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 20F);
            label3.Location = new Point(26, 73);
            label3.Name = "label3";
            label3.Size = new Size(244, 37);
            label3.TabIndex = 4;
            label3.Text = "Proyectos recientes";
            // 
            // lvRecent
            // 
            lvRecent.Activation = ItemActivation.OneClick;
            lvRecent.BackColor = Color.FromArgb(35, 35, 35);
            lvRecent.BorderStyle = BorderStyle.FixedSingle;
            lvRecent.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            lvRecent.ForeColor = Color.White;
            lvRecent.FullRowSelect = true;
            lvRecent.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvRecent.LabelWrap = false;
            lvRecent.Location = new Point(26, 123);
            lvRecent.MultiSelect = false;
            lvRecent.Name = "lvRecent";
            lvRecent.Size = new Size(547, 319);
            lvRecent.TabIndex = 0;
            lvRecent.UseCompatibleStateImageBehavior = false;
            lvRecent.View = View.Details;
            lvRecent.DoubleClick += lvRecent_DoubleClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Nombre";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Ruta";
            columnHeader2.Width = 220;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Última apertura";
            columnHeader3.Width = 120;
            // 
            // frmLauncher
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(841, 514);
            Controls.Add(panel2);
            Controls.Add(panel1);
            ForeColor = Color.White;
            MaximizeBox = false;
            MaximumSize = new Size(857, 553);
            Name = "frmLauncher";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BimpEngine - Gestor de Proyectos";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Button btnAbrir;
        private Button btnNew;
        private Label label2;
        private Label label1;
        private ListView lvRecent;
        private Label label3;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private Button btnOpen;
    }
}