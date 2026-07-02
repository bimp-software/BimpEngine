using BimpEngine.Engine.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Runtime
{
    public class PlayerInput
    {
        public bool Forward =>
            InputManager.GetAction("player_forward") ||
            InputManager.GetAction("player_arrow_up");

        public bool Back =>
            InputManager.GetAction("player_back") ||
            InputManager.GetAction("player_arrow_down");

        public bool Left =>
            InputManager.GetAction("player_left") ||
            InputManager.GetAction("player_arrow_left");

        public bool Right =>
            InputManager.GetAction("player_right") ||
            InputManager.GetAction("player_arrow_right");

        public bool Jump =>
            InputManager.GetAction("player_jump");

        public bool Run =>
            InputManager.GetAction("player_run");

        public bool Interact =>
            InputManager.GetAction("player_interact");
    }
}
