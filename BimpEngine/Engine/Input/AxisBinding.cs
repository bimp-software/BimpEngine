namespace BimpEngine.Engine.Input
{
    public class AxisBinding
    {
        public string Nombre { get; set; }

        // IDs de acciones (InputBinding.Id) que mueven el eje en cada dirección
        public string AccionPositiva { get; set; }
        public string AccionNegativa { get; set; }

        // Alternativas opcionales (ej: flechas además de WASD)
        public string AccionPositivaAlt { get; set; }
        public string AccionNegativaAlt { get; set; }

        // Qué tan rápido (unidades/seg) se acerca el valor a -1/1 mientras se mantiene la tecla
        public float Sensibilidad { get; set; } = 3f;

        // Qué tan rápido (unidades/seg) vuelve el valor a 0 al soltar la tecla
        public float Gravedad { get; set; } = 3f;

        // Si es true, GetAxis devuelve el valor suavizado; si es false, se comporta igual que GetAxisRaw
        public bool Suavizado { get; set; } = true;

        public AxisBinding() { }

        public AxisBinding(string nombre, string accionPositiva, string accionNegativa,
            string accionPositivaAlt = "", string accionNegativaAlt = "",
            float sensibilidad = 3f, float gravedad = 3f, bool suavizado = true)
        {
            Nombre = nombre;
            AccionPositiva = accionPositiva;
            AccionNegativa = accionNegativa;
            AccionPositivaAlt = accionPositivaAlt;
            AccionNegativaAlt = accionNegativaAlt;
            Sensibilidad = sensibilidad;
            Gravedad = gravedad;
            Suavizado = suavizado;
        }
    }
}