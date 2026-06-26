using BimpEngine.Engine.Entities;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Core
{
    public class Scene
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; } = "Scene";
        public bool IsLoaded { get; set; } = true;
        public List<Objetos> Objetos { get; } = new List<Objetos>();
        public CameraObject MainCamera { get; set; } = new CameraObject();
        public void Add(Objetos objeto)
        {
            if (objeto == null)
                return;

            Objetos.Add(objeto);

            if (objeto is CameraObject camera && MainCamera == null)
                MainCamera = camera;
        }

        public void Remove(Objetos objeto)
        {
            if (objeto == null)
                return;

            Objetos.Remove(objeto);

            if (MainCamera == objeto)
                MainCamera = null;
        }

        public void Clear()
        {
            Objetos.Clear();
        }

        public Objetos FindById(Guid id)
        {
            return Objetos.FirstOrDefault(x => x.Id == id);
        }

        public Objetos FindByName(string name)
        {
            return Objetos.FirstOrDefault(x => x.Name == name);
        }

        public List<Objetos> FindByTag(string tag)
        {
            return Objetos
                .Where(x => x.Tag == tag)
                .ToList();
        }

        public void Update()
        {
            foreach (var objeto in Objetos)
            {
                if (objeto.Enabled)
                    objeto.Update();
            }
        }
    }
}
