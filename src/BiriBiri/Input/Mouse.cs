using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;

namespace BiriBiri.Input
{
    public enum MouseButton
    {
        Left = 0,
        Right = 1,
        WheelUp = 2,
        WheelDown = 3
    }

    public class Mouse : IUpdatable
    {
        private readonly KeyStates[] _a = new KeyStates[4];
        private readonly KeyStates[] _b = new KeyStates[4];

        private bool _flip;

        public double X;
        public double Y;

        public bool IsInside;

        public void Claim(Element element)
        {
            element.OnMouseMove = evt =>
            {
                var rect = element.GetBoundingClientRect();

                var x = evt.ClientX - rect.Left;
                var y = evt.ClientY - rect.Top;

                X = MathHelper.Clamp(x, 0, element.ClientWidth);
                Y = MathHelper.Clamp(y, 0, element.ClientHeight);

                IsInside = evt.ClientX >= rect.Left && rect.Right >= evt.ClientX && evt.ClientY >= rect.Top &&
                           rect.Bottom >= evt.ClientY;
            };

            element.OnMouseUp = evt =>
            {
                switch (evt.Button)
                {
                    case 0:
                        GetCurrent()[(int)MouseButton.Left] = KeyStates.Up;
                        break;
                    case 2:
                        GetCurrent()[(int)MouseButton.Right] = KeyStates.Up;
                        break;
                }
            };

            element.OnMouseDown = evt =>
            {
                switch (evt.Button)
                {
                    case 0:
                        GetCurrent()[(int)MouseButton.Left] = KeyStates.Down;
                        break;
                    case 2:
                        GetCurrent()[(int)MouseButton.Right] = KeyStates.Down;
                        break;
                }
            };

            element.OnMouseEnter = evt =>
            {
                IsInside = true;
            };

            element.OnMouseLeave = evt =>
            {
                IsInside = false;
            };
        }

        public void HandleMove(KeyboardEvent evt)
        {
            if (evt.KeyCode < 1 || 255 < evt.KeyCode) return;
        }

        public void Update(double delta)
        {
            _flip = !_flip;
        }

        private KeyStates[] GetCurrent()
        {
            return _flip ? _b : _a;
        }

        private KeyStates[] GetPrevious()
        {
            return _flip ? _a : _b;
        }
    }
}
