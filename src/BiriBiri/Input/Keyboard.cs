using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using Bridge.Html5;

namespace BiriBiri.Input
{
    public enum KeyStates
    {
        Up,
        Down
    }

    public enum Key
    {
        ArrowDown = 0x28,
        ArrowLeft = 0x25,
        ArrowRight = 0x27,
        ArrowUp = 0x26,
    }

    public class Keyboard : IUpdatable
    {
        private readonly KeyStates[] _a = new KeyStates[256];
        private readonly KeyStates[] _b = new KeyStates[256];

        private bool _flip;

        public void Update(double delta)
        {
            _flip = !_flip;
        }

        public void Clear()
        {
            var current = GetCurrent();

            for (int i = 0; i < 256; i++)
            {
                current[i] = KeyStates.Up;
            }
        }

        public bool IsUp(Key key)
        {
            var keyCode = (int) key;

            return GetCurrent()[keyCode] == KeyStates.Up;
        }

        public bool IsDown(Key key)
        {
            var keyCode = (int)key;

            return GetCurrent()[keyCode] == KeyStates.Down;
        }

        public bool WasUp(Key key)
        {
            var keyCode = (int)key;

            return GetPrevious()[keyCode] == KeyStates.Up;
        }

        public bool WasDown(Key key)
        {
            var keyCode = (int)key;

            return GetPrevious()[keyCode] == KeyStates.Down;
        }

        public bool WasPressed(Key key)
        {
            return WasUp(key) && IsDown(key);
        }

        public bool WasReleased(Key key)
        {
            return WasDown(key) && IsUp(key);
        }

        public void HandleKeyUp(KeyboardEvent evt)
        {
            if (evt.KeyCode < 1 || 255 < evt.KeyCode) return;

            GetCurrent()[evt.KeyCode] = KeyStates.Up;
        }

        public void HandleKeyDown(KeyboardEvent evt)
        {
            if (evt.KeyCode < 1 || 255 < evt.KeyCode) return;

            GetCurrent()[evt.KeyCode] = KeyStates.Down;
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
