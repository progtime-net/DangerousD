using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace DangerousD.GameCore
{
    public enum ScopeState { Up, Middle, Down }
    public enum ControlsState { Gamepad, Keyboard, Mouse }
    class InputManager
    {
        public delegate void Delegat();
        public event Delegat MovEventJump;
        public event Delegat MovEventDown;
        public event Delegat ShootEvent;

        Vector2 vectorMovementDirection;     
        ScopeState scopeState;        // Положение оружия. Up, Middle, Down.
        ControlsState controlsState;

        private bool isJumpDown;      // Блокирует физическое нажатие прыжка и спуска
        private bool isShoot;

        public Vector2 VectorMovementDirection { get => vectorMovementDirection; }
        public ScopeState ScopeState { get => scopeState; }
        public string currentControlsState = ""; 

        public InputManager()
        {
            this.isJumpDown = false;
            this.isShoot = false;
            scopeState = ScopeState.Middle;
            controlsState= ControlsState.Mouse;
            vectorMovementDirection = new Vector2(0, 0);
        }
        public void SetState(ControlsState controlsStates)
        {
            currentControlsState = controlsStates.ToString();
        }
        public void Update()
        {
            // Работа с GamePad
            if (GamePad.GetState(0).IsConnected)
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
                else if (vectorMovementDirection.Y <= -0.7 && !isJumpDown)
                {
                    scopeState = ScopeState.Down;
                }
                else
                {
                    scopeState = ScopeState.Middle;
                }

                // Обработка нажатия выстрела. Вызывает событие ShootEvent
                if (gamePadState.Buttons.X == ButtonState.Pressed && !isJumpDown && !isShoot)
                {
                    isShoot = true;
                    ShootEvent?.Invoke();
                    Debug.WriteLine("Выстрел");
                }
                else if (gamePadState.Buttons.X == ButtonState.Released && !isJumpDown)
                {
                    isShoot = false;
                }
                SetState(ControlsState.Gamepad);
            }

            // Работа с KeyBoard
            else
            {
                KeyboardState keyBoardState = Keyboard.GetState();  // Состояние клавиатуры

                // Обработка движения вправо-влево. Меняет у вектора vectorMovementDirection значение X на -1/0/1.
                if (keyBoardState.IsKeyDown(Keys.Left))
                {
                    vectorMovementDirection.X = -1;
                }
                else if (keyBoardState.IsKeyDown(Keys.Right))
                {
                    vectorMovementDirection.X = 1;
                }
                else
                {
                    vectorMovementDirection.X = 0;
                }

                // Обработка прыжка и спуска. Вызываются события MovEvent.
                if (keyBoardState.IsKeyDown(Keys.LeftShift) && !isJumpDown && keyBoardState.IsKeyDown(Keys.Down))
                {
                    isJumpDown = true;
                    MovEventDown?.Invoke();
                    Debug.WriteLine("Спуск");
                }
                else if (keyBoardState.IsKeyDown(Keys.LeftShift) && !isJumpDown)
                {
                    isJumpDown = true;
                    MovEventJump?.Invoke();
                    Debug.WriteLine("Прыжок");
                }
                else if (keyBoardState.IsKeyUp(Keys.LeftShift))
                {
                    isJumpDown = false;
                }

                // Обработка положения оружия. Задает значение полю scopeState.
                if (keyBoardState.IsKeyDown(Keys.Up))
                {
                    scopeState = ScopeState.Up;
                }
                else if (keyBoardState.IsKeyDown(Keys.Down) && !isJumpDown)
                {
                    scopeState = ScopeState.Down;
                }
                else
                {
                    scopeState = ScopeState.Middle;
                }

                // Обработка нажатия выстрела. Вызывает событие ShootEvent
                if (keyBoardState.IsKeyDown(Keys.X) && !isJumpDown && !isShoot)
                {
                    isShoot = true;
                    ShootEvent?.Invoke();
                    Debug.WriteLine("Выстрел");
                }
                else if (keyBoardState.IsKeyUp(Keys.X) && !isJumpDown)
                {
                    isShoot = false;
                }
            }
        }
    }
}
