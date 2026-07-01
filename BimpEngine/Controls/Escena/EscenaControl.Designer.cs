namespace BimpEngine.Controls.Escena
{
    partial class EscenaControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EscenaControl));
            panel1 = new Panel();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            panel3 = new Panel();
            glControl = new SharpGL.OpenGLControl();
            panel4 = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)glControl).BeginInit();
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
            panel1.Size = new Size(1089, 30);
            panel1.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(70, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(992, 30);
            panel2.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(1062, 0);
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
            label1.Text = "Escena";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            panel3.Controls.Add(glControl);
            panel3.Controls.Add(panel4);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 30);
            panel3.Name = "panel3";
            panel3.Size = new Size(1089, 614);
            panel3.TabIndex = 3;
            // 
            // glControl
            // 
            glControl.AllowDrop = true;
            glControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            glControl.AutoSize = true;
            glControl.DrawFPS = true;
            glControl.ForeColor = Color.White;
            glControl.Location = new Point(4, 32);
            glControl.Margin = new Padding(4, 3, 4, 3);
            glControl.Name = "glControl";
            glControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            glControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            glControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            glControl.Size = new Size(1081, 579);
            glControl.TabIndex = 1;
            glControl.OpenGLInitialized += glControl_OpenGLInitialized;
            glControl.OpenGLDraw += glControl_OpenGLDraw;
            glControl.Resized += glControl_Resized;
            glControl.DragDrop += glControl_DragDrop;
            glControl.DragEnter += glControl_DragEnter;
            glControl.KeyDown += glControl_KeyDown;
            glControl.MouseDown += glControl_MouseDown;
            glControl.MouseMove += glControl_MouseMove;
            glControl.MouseUp += glControl_MouseUp;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(40, 40, 40);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(1089, 34);
            panel4.TabIndex = 0;
            // 
            // EscenaControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(48, 48, 48);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(panel3);
            Controls.Add(panel1);
            ForeColor = Color.White;
            Name = "EscenaControl";
            Size = new Size(1089, 644);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)glControl).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Label label1;
        private Panel panel3;
        private Panel panel4;
        private SharpGL.OpenGLControl glControl;
    }
}
