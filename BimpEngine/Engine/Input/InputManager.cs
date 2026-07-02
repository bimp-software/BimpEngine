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

        public static void Clear()
        {
            _keysDown.Clear();
        }
    }
}
