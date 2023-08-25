using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace DangerousD.GameCore
{
    public enum ScopeState { Up, Middle, Down }
    public enum ControlsState { Gamepad, Keyboard }
    public class InputManager
    {
        public delegate void Delegat();
        public event Delegat MovEventJump;
        public event Delegat MovEventDown;
        public event Delegat ShootEvent;

        Vector2 vectorMovementDirection;
        ScopeState scopeState;        // Положение оружия. Up, Middle, Down.
        ControlsState controlsState;
        private bool _overrideControls = false;
        private bool _cheatsEnabled = false;
        public bool InvincibilityCheat { get; private set; } = false;
        public bool CollisionsCheat { get; private set; } = false;
        public bool InfiniteAmmoCheat { get; private set; } = false;

        private bool isJumpDown;      // Блокирует физическое нажатие прыжка и спуска
        private bool isShoot;

        private KeyboardState lastKeyboardState;
        private GamePadState lastGamePadState;


        public Vector2 VectorMovementDirection { get => vectorMovementDirection; }
        public ScopeState ScopeState { get => scopeState; }
        public string currentControlsState;

        public InputManager()
        {
            this.isJumpDown = false;
            this.isShoot = false;
            scopeState = ScopeState.Middle;
            controlsState = ControlsState.Keyboard;
            vectorMovementDirection = new Vector2(0, 0);
        }
        public void Update()
        {
            if (_cheatsEnabled)
            {
                AppManager.Instance.DebugHUD.Set("cheats", _cheatsEnabled.ToString());
                AppManager.Instance.DebugHUD.Set("invincible", InvincibilityCheat.ToString());
                AppManager.Instance.DebugHUD.Set("infinite ammo", InfiniteAmmoCheat.ToString());
            }

            #region Работа с GamePad
            if (_overrideControls ? controlsState == ControlsState.Gamepad : GamePad.GetState(0).IsConnected)
            {
                controlsState = ControlsState.Gamepad;

                #region Обработка гейм-пада. Задает Vector2 vectorMovementDirection являющийся вектором отклонения левого стика.
                GamePadState gamePadState = GamePad.GetState(0);
                vectorMovementDirection = gamePadState.ThumbSticks.Left;
                #endregion

                #region читы 
                if (gamePadState.Triggers.Left >= 0.9 && gamePadState.Triggers.Right >= 0.9)
                    _cheatsEnabled = true;
                if (_cheatsEnabled)
                {
                    if (gamePadState.Buttons.Y == ButtonState.Pressed && lastGamePadState.Buttons.Y == ButtonState.Released)
                        InvincibilityCheat = !InvincibilityCheat;
                    if (gamePadState.Buttons.B == ButtonState.Pressed && lastGamePadState.Buttons.B == ButtonState.Released)
                        CollisionsCheat = !CollisionsCheat;
                    //TODO: infinite ammo cheat by gamepad
                }
                #endregion // Cheats
                 
                #region Обработка нажатия прыжка и спуска. Вызывает события MovEvent.
                if (vectorMovementDirection.Y < -0.2 && gamePadState.Buttons.A == ButtonState.Pressed && !isJumpDown)
                {
                    isJumpDown = true;
                    MovEventDown?.Invoke();
                    Debug.WriteLine("Спуск");
                }
                else if (gamePadState.Buttons.A == ButtonState.Pressed && lastGamePadState.Buttons.A == ButtonState.Released)
                {
                    MovEventJump?.Invoke();
                    Debug.WriteLine("Прыжок");
                }
                #endregion

                #region Обработка положения оружия. Задает значение полю scopeState.
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
                #endregion

                #region Обработка нажатия выстрела. Вызывает событие ShootEvent
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
                #endregion

                lastGamePadState = gamePadState;
            }
            #endregion
            #region Работа с KeyBoard
            else
            {
                controlsState = ControlsState.Keyboard;

                #region Состояние клавиатуры
                KeyboardState keyBoardState = Keyboard.GetState();  // Состояние клавиатуры
                #endregion

                #region читы 
                if (keyBoardState.IsKeyDown(Keys.LeftShift) && keyBoardState.IsKeyDown(Keys.RightShift))
                    _cheatsEnabled = true;
                if (_cheatsEnabled)
                {
                    if (keyBoardState.IsKeyDown(Keys.I) && lastKeyboardState.IsKeyUp(Keys.I))
                        InvincibilityCheat = !InvincibilityCheat;
                    if (keyBoardState.IsKeyDown(Keys.C) && lastKeyboardState.IsKeyUp(Keys.C))
                        CollisionsCheat = !CollisionsCheat;
                    if (keyBoardState.IsKeyDown(Keys.A) && lastKeyboardState.IsKeyUp(Keys.A))
                        InfiniteAmmoCheat = !InfiniteAmmoCheat;
                }
                #endregion // Cheats

                #region Обработка движения вправо-влево. Меняет у вектора vectorMovementDirection значение X на -1/0/1.
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
                #endregion

                #region Обработка прыжка и спуска. Вызываются события MovEvent.
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
                #endregion

                #region Обработка положения оружия. Задает значение полю scopeState.
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
                #endregion

                #region Обработка нажатия выстрела. Вызывает событие ShootEvent
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
                #endregion

                SetState(ControlsState.Keyboard);
                lastKeyboardState = keyBoardState;
            }
            #endregion 
        }
        public void SetState(ControlsState controlsState)
        {
            currentControlsState = controlsState.ToString();
        }
    }
}
