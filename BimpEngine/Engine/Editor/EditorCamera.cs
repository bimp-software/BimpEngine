using SharpGL;
using System;

namespace BimpEngine.Engine.Editor
{
    public class EditorCamera
    {
        public double EyeX { get; private set; }
        public double EyeY { get; private set; }
        public double EyeZ { get; private set; }

        public double TargetX { get; set; } = 0;
        public double TargetY { get; set; } = 0;
        public double TargetZ { get; set; } = 0;

        public double Distance { get; set; } = 15;

        public double Yaw { get; set; } = 45;
        public double Pitch { get; set; } = 25;

        public double MoveSpeed { get; set; } = 0.3;
        public double RotateSpeed { get; set; } = 0.4;
        public double ZoomSpeed { get; set; } = 0.8;
        public double PanSpeed { get; set; } = 0.03;

        public void Apply(OpenGL gl)
        {
            double yawRad = Yaw * System.Math.PI / 180.0;
            double pitchRad = Pitch * System.Math.PI / 180.0;

            EyeX = TargetX + Distance * System.Math.Cos(pitchRad) * System.Math.Sin(yawRad);
            EyeY = TargetY + Distance * System.Math.Sin(pitchRad);
            EyeZ = TargetZ + Distance * System.Math.Cos(pitchRad) * System.Math.Cos(yawRad);

            gl.LookAt(
                EyeX, EyeY, EyeZ,
                TargetX, TargetY, TargetZ,
                0, 1, 0
            );
        }

        public void Rotate(int dx, int dy)
        {
            Yaw += dx * RotateSpeed;
            Pitch += dy * RotateSpeed;

            if (Pitch > 89) Pitch = 89;
            if (Pitch < -89) Pitch = -89;
        }

        public void Zoom(int delta)
        {
            double direction = System.Math.Sign(delta);

            Distance -= direction * Distance * 0.1;

            if (Distance < 0.01)
                Distance = 0.01;
        }

        public void Pan(int dx, int dy)
        {
            double yawRad = Yaw * System.Math.PI / 180.0;

            double rightX = System.Math.Cos(yawRad);
            double rightZ = -System.Math.Sin(yawRad);

            TargetX -= dx * PanSpeed * rightX;
            TargetZ -= dx * PanSpeed * rightZ;

            TargetY += dy * PanSpeed;
        }

        public void Move(Keys key)
        {
            double yawRad = Yaw * System.Math.PI / 180.0;

            double forwardX = System.Math.Sin(yawRad);
            double forwardZ = System.Math.Cos(yawRad);

            double rightX = System.Math.Cos(yawRad);
            double rightZ = -System.Math.Sin(yawRad);

            if (key == Keys.W)
            {
                TargetX += forwardX * MoveSpeed;
                TargetZ += forwardZ * MoveSpeed;
            }

            if (key == Keys.S)
            {
                TargetX -= forwardX * MoveSpeed;
                TargetZ -= forwardZ * MoveSpeed;
            }

            if (key == Keys.A)
            {
                TargetX -= rightX * MoveSpeed;
                TargetZ -= rightZ * MoveSpeed;
            }

            if (key == Keys.D)
            {
                TargetX += rightX * MoveSpeed;
                TargetZ += rightZ * MoveSpeed;
            }

            if (key == Keys.Q)
                TargetY -= MoveSpeed;

            if (key == Keys.E)
                TargetY += MoveSpeed;
        }
    }
}
