using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EndersEditor
{
    delegate void MouseClickHandler(Vector2 position);
    delegate void MouseMoveHandler(Vector2 position, Vector2 movement);

    static class MouseInput
    {
        static MouseState lastMouseState;
        static Vector2 movement;
        static bool dragging;

        public static event MouseClickHandler MouseDown = delegate(Vector2 position) { };
        public static event MouseClickHandler MouseUp = delegate(Vector2 position) { };
        public static event MouseClickHandler StartDrag = delegate(Vector2 position) { };
        public static event MouseClickHandler EndDrag = delegate(Vector2 position) { };
        public static event MouseMoveHandler MouseMove = delegate(Vector2 position, Vector2 movement) { };

        public static bool IsLeftButtonDown { get { return lastMouseState.LeftButton == ButtonState.Pressed; } }

        public static void Update()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 currentPosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 lastPosition = new Vector2(lastMouseState.X, lastMouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                MouseDown(currentPosition);
            }
            if (mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
            {
                if (dragging)
                {
                    dragging = false;
                    EndDrag(currentPosition);
                }
                else
                {
                    MouseUp(currentPosition);
                }
            }

            movement = currentPosition - lastPosition;
            if (movement.Length() > 0f)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && !dragging)
                {
                    dragging = true;
                    StartDrag(currentPosition);
                }
                MouseMove(currentPosition, movement);
            }
            lastMouseState = mouseState;
        }
    }
}
