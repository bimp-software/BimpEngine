using BimpEngine.Engine.Math;
using BimpEngine.Engine.Scripting.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project
{
    public class ObjectData
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "GameObject";
        public string PrimitiveType { get; set; } = "Cube";
        public bool Is2D { get; set; } = false;
        public bool Enabled { get; set; } = true;
        public string Tag { get; set; } = "Untagged";
        public int Layer { get; set; } = 0;
        public bool BlueprintMode { get; set; } = false;
        public Vec3Data Position { get; set; } = new();
        public Vec3Data Rotation { get; set; } = new();
        public Vec3Data Scale { get; set; } = new(1, 1, 1);
        public Guid? ParentId { get; set; } = null;
        public List<Guid> ChildrenIds { get; set; } = new();

        public bool IsCamera { get; set; } = false;
        public double FieldOfView { get; set; } = 60;
        public double NearClip { get; set; } = 0.1;
        public double FarClip { get; set; } = 1000;

        public List<ScriptData> Scripts { get; set; } = new();
    }
}
