using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DangerousD.GameCore
{
    public enum ScopeState { Up, Middle, Down }
    class InputManager
    {
        public delegate void Delegat();
        public event Delegat MovEventJump;
        public event Delegat MovEventDown;

        Vector2 vectorMovementDirection;     
        ScopeState scopeState;        // Положение оружия. Up, Middle, Down.

        private bool isJumpDown;      // Блокирует физическое нажатие прыжка и спуска

        public Vector2 VectorMovementDirection { get => vectorMovementDirection; }
        public ScopeState ScopeState { get => scopeState; }

        public InputManager()
        {
            this.isJumpDown = false;
            scopeState = ScopeState.Middle;
        }

        public void Update()
        {
            // Обработка гейм-пада. Задает Vector2 vectorMovementDirection являющийся вектором отклонения левого стика.
            GamePadState gamePadState = GamePad.GetState(0);
            vectorMovementDirection = gamePadState.ThumbSticks.Left;

            // Обработка нажатия прыжка и спуска. Вызывает события MovEvent.
            if (vectorMovementDirection.Y < -0.2 && gamePadState.Buttons.A == ButtonState.Pressed && !isJumpDown)
            {
                isJumpDown = true;
                MovEventDown?.Invoke();
                Debug.WriteLine("Спуск");
            }
            else if (gamePadState.Buttons.A == ButtonState.Pressed && !isJumpDown)
            {
                isJumpDown = true;            
                MovEventJump?.Invoke();
                Debug.WriteLine("Прыжок");
            }
            else if (gamePadState.Buttons.A == ButtonState.Released)
            {
                isJumpDown = false;
            }

            // Обработка положения оружия. Задает значение полю scopeState.
            if (vectorMovementDirection.Y >= 0.7)
            {
                scopeState = ScopeState.Up;
            }
            else if (vectorMovementDirection.Y <= -0.7)
            {
                scopeState = ScopeState.Down;
            }
            else
            {
                scopeState = ScopeState.Middle;
            }
        }
    }
}
