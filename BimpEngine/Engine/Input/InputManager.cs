using BimpEngine.Engine.Core;

namespace BimpEngine.Engine.Input
{
    public static class InputManager
    {
        private static readonly HashSet<Keys> _keysDown = new();

        public static void KeyDown(Keys key)
        {
            if (!_keysDown.Contains(key))
                _keysDown.Add(key);
        }

        public static void KeyUp(Keys key)
        {
            if (_keysDown.Contains(key))
                _keysDown.Remove(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return _keysDown.Contains(key);
        }

        public static bool GetAction(string actionId)
        {
            string keyName = EngineSettings.Current.GetKey(actionId);

            if (Enum.TryParse(keyName, out Keys key))
                return IsKeyDown(key);

            return false;
        }

        private static readonly Dictionary<string, float> _axisValues = new();

        /// <summary>
        /// Valor instantáneo del eje: -1, 0 o 1 según las teclas presionadas (equivalente a Input.GetAxisRaw de Unity).
        /// </summary>
        public static float GetAxisRaw(string nombre)
        {
            var eje = EngineSettings.Current.GetAxisBinding(nombre);
            if (eje == null) return 0f;

            bool positivo = EsAccionActiva(eje.AccionPositiva) || EsAccionActiva(eje.AccionPositivaAlt);
            bool negativo = EsAccionActiva(eje.AccionNegativa) || EsAccionActiva(eje.AccionNegativaAlt);

            if (positivo && !negativo) return 1f;
            if (negativo && !positivo) return -1f;
            return 0f;
        }

        /// <summary>
        /// Valor suavizado del eje entre -1 y 1, interpolado con Sensibilidad/Gravedad
        /// (equivalente a Input.GetAxis de Unity). Requiere que UpdateAxes(deltaTime) se llame cada frame.
        /// </summary>
        public static float GetAxis(string nombre)
        {
            var eje = EngineSettings.Current.GetAxisBinding(nombre);
            if (eje == null) return 0f;

            if (!eje.Suavizado)
                return GetAxisRaw(nombre);

            return _axisValues.TryGetValue(nombre, out var valor) ? valor : 0f;
        }

        /// <summary>
        /// Debe llamarse una vez por frame (con Time.DeltaTime) mientras el juego corre,
        /// para actualizar el valor suavizado de todos los ejes registrados.
        /// </summary>
        public static void UpdateAxes(float deltaTime)
        {
            foreach (var eje in EngineSettings.Current.AxisBindings)
            {
                float objetivo = GetAxisRaw(eje.Nombre);
                float actual = _axisValues.TryGetValue(eje.Nombre, out var v) ? v : 0f;

                float paso = (System.Math.Abs(objetivo) > 0.0001f ? eje.Sensibilidad : eje.Gravedad) * deltaTime;

                if (actual < objetivo)
                    actual = System.Math.Min(actual + paso, objetivo);
                else if (actual > objetivo)
                    actual = System.Math.Max(actual - paso, objetivo);

                _axisValues[eje.Nombre] = actual;
            }
        }

        private static bool EsAccionActiva(string accionId)
        {
            if (string.IsNullOrEmpty(accionId)) return false;
            return GetAction(accionId);
        }

        public static void Clear()
        {
            _keysDown.Clear();
            _axisValues.Clear();
        }
    }
}