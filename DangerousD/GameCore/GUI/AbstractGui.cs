using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;

namespace DangerousD.GameCore.GUI;

public abstract class AbstractGui : IDrawableObject
{
    protected UIManager Manager = new();
    protected List<DrawableUIElement> Elements = new();
    private List<DrawableUIElement> ActiveElements;
    protected DrawableUIElement SelectedElement;
    private bool isStartedPrint = false;
    private bool isPressed = false;

    public AbstractGui()
    {
    }

    protected abstract void CreateUI();
    private GraphicsDevice graphicsDevice;
    public virtual void Initialize()
    {
        Manager.Initialize(AppManager.Instance.GraphicsDevice);
        CreateUI();
        ActiveElements = new List<DrawableUIElement>();
        foreach (var element in Elements)
        {
            if (CheckOnBadElements(element))
            {
                ActiveElements.Add(element);
            }
        }
        if (ActiveElements.Count > 0) { SelectedElement = ActiveElements.First(); }
        
    }

    public virtual void LoadContent()
    {
        Manager.LoadContent(AppManager.Instance.Content, "Font2");
    }

    public virtual void Update(GameTime gameTime)
    {
        string state = AppManager.Instance.InputManager.currentControlsState;

        

        if (ActiveElements.Count != 0)
        {
            if (state == "Gamepad")
            {
                GamePadState gamePadState = GamePad.GetState(0);
                GamepadInput(gamePadState);
            }
            else if (state == "Keyboard" || state == "Mouse")
            {
                KeyboardState keyBoardState = Keyboard.GetState();
                KeyBoardInput(keyBoardState);
            }
            
        }

        Manager.Update(gameTime);

        if (SelectedElement is not null)
        {
            if (SelectedElement is Button)
            {
                (SelectedElement as Button).hoverState = MonogameLibrary.UI.Enums.HoverState.Hovering;
            }
            if (SelectedElement is ButtonText)
            {
                (SelectedElement as ButtonText).hoverState = MonogameLibrary.UI.Enums.HoverState.Hovering;
            }
            if (SelectedElement is TextBox)
            {
                (SelectedElement as TextBox).hoverState = MonogameLibrary.UI.Enums.HoverState.Hovering;
            }
        }
    }
        
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Manager.Draw(spriteBatch);
    }
    protected virtual void GamepadInput(GamePadState gamePadState)
    {
        if (gamePadState.DPad.Up == ButtonState.Pressed && !isPressed)
        {
            isPressed = true;
            ChangeSelectedElement(-1);
            Debug.WriteLine("switch");
        }
        else if (gamePadState.DPad.Down == ButtonState.Pressed && !isPressed)
        {
            isPressed = true;
            ChangeSelectedElement(1);
        }
        else if (gamePadState.Buttons.A == ButtonState.Pressed && !isPressed)
        {
            isPressed = true;
            if (SelectedElement is Button)
            {
                Button button = SelectedElement as Button;
                button.CallLeftBtnEvent();
            }
        }
        else if (isPressed && (gamePadState.Buttons.A == ButtonState.Released && 
                        gamePadState.DPad.Down == ButtonState.Released && 
                        gamePadState.DPad.Up == ButtonState.Released))
        {
            isPressed = false;
        }
    }
    protected virtual void KeyBoardInput(KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.Up) && !isPressed)
        {
            isPressed = true;
            ChangeSelectedElement(-1);
        }
        else if (keyboardState.IsKeyDown(Keys.Down) && !isPressed)
        {
            isPressed = true;
            ChangeSelectedElement(1);
        }
        else if (keyboardState.IsKeyDown(Keys.Enter) && !isPressed)
        {
            isPressed = true;
            if (SelectedElement is Button)
            {
                Button button = SelectedElement as Button;
                button.CallLeftBtnEvent();
            }
        }
        else if (isPressed && (keyboardState.IsKeyUp(Keys.Enter) &&
                            keyboardState.IsKeyUp(Keys.Down) &&
                            keyboardState.IsKeyUp(Keys.Up)))
        {
            isPressed = false;
        }
    }
    private void ChangeSelectedElement(int x) // Меняет выбранный элемент
    {
        for (int i = 0; i < ActiveElements.Count; i++)
        {
            if (ActiveElements[i] == SelectedElement)
            {
                if (i + x >= ActiveElements.Count)
                {
                    SelectedElement = ActiveElements.First();
                    return;
                }
                else
                {
                    if (i + x < 0)
                    {
                        SelectedElement = ActiveElements.Last();
                        return;
                    }
                    else
                    {
                        SelectedElement = ActiveElements[i + x];
                        return;
                    }
                }
            }
        }
    }
    private bool CheckOnBadElements(DrawableUIElement element)
    {
        if (element is Button)
        {
            return true;
        }
        else if (element is ButtonText)
        {
            return true;
        }
        else if (element is TextBox)
        {
            return true;
        }
        else return false;
    }
}