namespace BimpEngine.Controls.Inspector.Componentes
{
    partial class TransformControl
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
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            panel5 = new Panel();
            panel14 = new Panel();
            tbScale_Y = new TextBox();
            panel15 = new Panel();
            tbScale_Z = new TextBox();
            panel16 = new Panel();
            tbScale_X = new TextBox();
            label5 = new Label();
            panel4 = new Panel();
            panel11 = new Panel();
            tbRotation_Y = new TextBox();
            panel12 = new Panel();
            tbRotation_Z = new TextBox();
            panel13 = new Panel();
            tbRotation_X = new TextBox();
            label4 = new Label();
            panel3 = new Panel();
            panel9 = new Panel();
            tbPosition_Y = new TextBox();
            panel10 = new Panel();
            tbPosition_Z = new TextBox();
            panel8 = new Panel();
            tbPosition_X = new TextBox();
            label3 = new Label();
            panel6 = new Panel();
            panel7 = new Panel();
            label7 = new Label();
            label8 = new Label();
            label6 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel5.SuspendLayout();
            panel14.SuspendLayout();
            panel15.SuspendLayout();
            panel16.SuspendLayout();
            panel4.SuspendLayout();
            panel11.SuspendLayout();
            panel12.SuspendLayout();
            panel13.SuspendLayout();
            panel3.SuspendLayout();
            panel9.SuspendLayout();
            panel10.SuspendLayout();
            panel8.SuspendLayout();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(225, 26);
            panel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(40, 40, 40);
            label1.Dock = DockStyle.Left;
            label1.ForeColor = Color.White;
            label1.ImeMode = ImeMode.NoControl;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(5, 0, 0, 0);
            label1.Size = new Size(103, 26);
            label1.TabIndex = 0;
            label1.Text = "Transformación";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(40, 40, 40);
            panel2.Controls.Add(panel5);
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(panel3);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 49);
            panel2.Name = "panel2";
            panel2.Size = new Size(225, 105);
            panel2.TabIndex = 3;
            // 
            // panel5
            // 
            panel5.Controls.Add(panel14);
            panel5.Controls.Add(panel15);
            panel5.Controls.Add(panel16);
            panel5.Controls.Add(label5);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 70);
            panel5.Name = "panel5";
            panel5.Size = new Size(225, 35);
            panel5.TabIndex = 3;
            // 
            // panel14
            // 
            panel14.Controls.Add(tbScale_Y);
            panel14.Dock = DockStyle.Fill;
            panel14.Location = new Point(117, 0);
            panel14.Name = "panel14";
            panel14.Size = new Size(54, 35);
            panel14.TabIndex = 6;
            // 
            // tbScale_Y
            // 
            tbScale_Y.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbScale_Y.BackColor = Color.FromArgb(48, 48, 48);
            tbScale_Y.BorderStyle = BorderStyle.None;
            tbScale_Y.Font = new Font("Segoe UI", 12F);
            tbScale_Y.ForeColor = Color.White;
            tbScale_Y.Location = new Point(6, 5);
            tbScale_Y.Name = "tbScale_Y";
            tbScale_Y.Size = new Size(43, 22);
            tbScale_Y.TabIndex = 3;
            // 
            // panel15
            // 
            panel15.Controls.Add(tbScale_Z);
            panel15.Dock = DockStyle.Right;
            panel15.Location = new Point(171, 0);
            panel15.Name = "panel15";
            panel15.Size = new Size(54, 35);
            panel15.TabIndex = 7;
            // 
            // tbScale_Z
            // 
            tbScale_Z.BackColor = Color.FromArgb(48, 48, 48);
            tbScale_Z.BorderStyle = BorderStyle.None;
            tbScale_Z.Font = new Font("Segoe UI", 12F);
            tbScale_Z.ForeColor = Color.White;
            tbScale_Z.Location = new Point(5, 5);
            tbScale_Z.Name = "tbScale_Z";
            tbScale_Z.Size = new Size(45, 22);
            tbScale_Z.TabIndex = 3;
            // 
            // panel16
            // 
            panel16.Controls.Add(tbScale_X);
            panel16.Dock = DockStyle.Left;
            panel16.Location = new Point(63, 0);
            panel16.Name = "panel16";
            panel16.Size = new Size(54, 35);
            panel16.TabIndex = 5;
            // 
            // tbScale_X
            // 
            tbScale_X.BackColor = Color.FromArgb(48, 48, 48);
            tbScale_X.BorderStyle = BorderStyle.None;
            tbScale_X.Font = new Font("Segoe UI", 12F);
            tbScale_X.ForeColor = Color.White;
            tbScale_X.Location = new Point(5, 5);
            tbScale_X.Name = "tbScale_X";
            tbScale_X.Size = new Size(45, 22);
            tbScale_X.TabIndex = 3;
            // 
            // label5
            // 
            label5.Dock = DockStyle.Left;
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Size = new Size(63, 35);
            label5.TabIndex = 2;
            label5.Text = "Escala";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            panel4.Controls.Add(panel11);
            panel4.Controls.Add(panel12);
            panel4.Controls.Add(panel13);
            panel4.Controls.Add(label4);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 35);
            panel4.Name = "panel4";
            panel4.Size = new Size(225, 35);
            panel4.TabIndex = 2;
            // 
            // panel11
            // 
            panel11.Controls.Add(tbRotation_Y);
            panel11.Dock = DockStyle.Fill;
            panel11.Location = new Point(117, 0);
            panel11.Name = "panel11";
            panel11.Size = new Size(54, 35);
            panel11.TabIndex = 6;
            // 
            // tbRotation_Y
            // 
            tbRotation_Y.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbRotation_Y.BackColor = Color.FromArgb(48, 48, 48);
            tbRotation_Y.BorderStyle = BorderStyle.None;
            tbRotation_Y.Font = new Font("Segoe UI", 12F);
            tbRotation_Y.ForeColor = Color.White;
            tbRotation_Y.Location = new Point(6, 5);
            tbRotation_Y.Name = "tbRotation_Y";
            tbRotation_Y.Size = new Size(43, 22);
            tbRotation_Y.TabIndex = 3;
            // 
            // panel12
            // 
            panel12.Controls.Add(tbRotation_Z);
            panel12.Dock = DockStyle.Right;
            panel12.Location = new Point(171, 0);
            panel12.Name = "panel12";
            panel12.Size = new Size(54, 35);
            panel12.TabIndex = 7;
            // 
            // tbRotation_Z
            // 
            tbRotation_Z.BackColor = Color.FromArgb(48, 48, 48);
            tbRotation_Z.BorderStyle = BorderStyle.None;
            tbRotation_Z.Font = new Font("Segoe UI", 12F);
            tbRotation_Z.ForeColor = Color.White;
            tbRotation_Z.Location = new Point(5, 5);
            tbRotation_Z.Name = "tbRotation_Z";
            tbRotation_Z.Size = new Size(45, 22);
            tbRotation_Z.TabIndex = 3;
            // 
            // panel13
            // 
            panel13.Controls.Add(tbRotation_X);
            panel13.Dock = DockStyle.Left;
            panel13.Location = new Point(63, 0);
            panel13.Name = "panel13";
            panel13.Size = new Size(54, 35);
            panel13.TabIndex = 5;
            // 
            // tbRotation_X
            // 
            tbRotation_X.BackColor = Color.FromArgb(48, 48, 48);
            tbRotation_X.BorderStyle = BorderStyle.None;
            tbRotation_X.Font = new Font("Segoe UI", 12F);
            tbRotation_X.ForeColor = Color.White;
            tbRotation_X.Location = new Point(5, 5);
            tbRotation_X.Name = "tbRotation_X";
            tbRotation_X.Size = new Size(45, 22);
            tbRotation_X.TabIndex = 3;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Left;
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(63, 35);
            label4.TabIndex = 2;
            label4.Text = "Rotación";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            panel3.Controls.Add(panel9);
            panel3.Controls.Add(panel10);
            panel3.Controls.Add(panel8);
            panel3.Controls.Add(label3);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(225, 35);
            panel3.TabIndex = 0;
            // 
            // panel9
            // 
            panel9.Controls.Add(tbPosition_Y);
            panel9.Dock = DockStyle.Fill;
            panel9.Location = new Point(117, 0);
            panel9.Name = "panel9";
            panel9.Size = new Size(54, 35);
            panel9.TabIndex = 3;
            // 
            // tbPosition_Y
            // 
            tbPosition_Y.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbPosition_Y.BackColor = Color.FromArgb(48, 48, 48);
            tbPosition_Y.BorderStyle = BorderStyle.None;
            tbPosition_Y.Font = new Font("Segoe UI", 12F);
            tbPosition_Y.ForeColor = Color.White;
            tbPosition_Y.Location = new Point(6, 5);
            tbPosition_Y.Name = "tbPosition_Y";
            tbPosition_Y.Size = new Size(43, 22);
            tbPosition_Y.TabIndex = 3;
            // 
            // panel10
            // 
            panel10.Controls.Add(tbPosition_Z);
            panel10.Dock = DockStyle.Right;
            panel10.Location = new Point(171, 0);
            panel10.Name = "panel10";
            panel10.Size = new Size(54, 35);
            panel10.TabIndex = 4;
            // 
            // tbPosition_Z
            // 
            tbPosition_Z.BackColor = Color.FromArgb(48, 48, 48);
            tbPosition_Z.BorderStyle = BorderStyle.None;
            tbPosition_Z.Font = new Font("Segoe UI", 12F);
            tbPosition_Z.ForeColor = Color.White;
            tbPosition_Z.Location = new Point(5, 5);
            tbPosition_Z.Name = "tbPosition_Z";
            tbPosition_Z.Size = new Size(45, 22);
            tbPosition_Z.TabIndex = 3;
            // 
            // panel8
            // 
            panel8.Controls.Add(tbPosition_X);
            panel8.Dock = DockStyle.Left;
            panel8.Location = new Point(63, 0);
            panel8.Name = "panel8";
            panel8.Size = new Size(54, 35);
            panel8.TabIndex = 2;
            // 
            // tbPosition_X
            // 
            tbPosition_X.BackColor = Color.FromArgb(48, 48, 48);
            tbPosition_X.BorderStyle = BorderStyle.None;
            tbPosition_X.Font = new Font("Segoe UI", 12F);
            tbPosition_X.ForeColor = Color.White;
            tbPosition_X.Location = new Point(5, 5);
            tbPosition_X.Name = "tbPosition_X";
            tbPosition_X.Size = new Size(45, 22);
            tbPosition_X.TabIndex = 2;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Left;
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(63, 35);
            label3.TabIndex = 1;
            label3.Text = "Posición";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            panel6.Controls.Add(panel7);
            panel6.Controls.Add(label2);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(0, 26);
            panel6.Name = "panel6";
            panel6.Size = new Size(225, 23);
            panel6.TabIndex = 4;
            // 
            // panel7
            // 
            panel7.Controls.Add(label7);
            panel7.Controls.Add(label8);
            panel7.Controls.Add(label6);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(63, 0);
            panel7.Name = "panel7";
            panel7.Size = new Size(162, 23);
            panel7.TabIndex = 1;
            // 
            // label7
            // 
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label7.Location = new Point(54, 0);
            label7.Name = "label7";
            label7.Size = new Size(52, 23);
            label7.TabIndex = 2;
            label7.Text = "Y";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.Dock = DockStyle.Right;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label8.Location = new Point(106, 0);
            label8.Name = "label8";
            label8.Size = new Size(56, 23);
            label8.TabIndex = 3;
            label8.Text = "Z";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.Dock = DockStyle.Left;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(54, 23);
            label6.TabIndex = 1;
            label6.Text = "X";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Left;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(63, 23);
            label2.TabIndex = 0;
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TransformControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(35, 35, 35);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(panel6);
            Controls.Add(panel2);
            Controls.Add(panel1);
            ForeColor = Color.White;
            Name = "TransformControl";
            Size = new Size(225, 154);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel14.ResumeLayout(false);
            panel14.PerformLayout();
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            panel16.ResumeLayout(false);
            panel16.PerformLayout();
            panel4.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel11.PerformLayout();
            panel12.ResumeLayout(false);
            panel12.PerformLayout();
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            panel3.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel6.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel5;
        private Panel panel4;
        private Panel panel6;
        private Label label2;
        private Label label5;
        private Label label4;
        private Label label3;
        private Panel panel7;
        private Label label7;
        private Label label8;
        private Label label6;
        private Panel panel8;
        private Panel panel14;
        private Panel panel15;
        private Panel panel16;
        private Panel panel11;
        private Panel panel12;
        private Panel panel13;
        private Panel panel9;
        private Panel panel10;
        private TextBox tbScale_Y;
        private TextBox tbScale_Z;
        private TextBox tbScale_X;
        private TextBox tbRotation_Y;
        private TextBox tbRotation_Z;
        private TextBox tbRotation_X;
        private TextBox tbPosition_Y;
        private TextBox tbPosition_Z;
        private TextBox tbPosition_X;
    }
}
