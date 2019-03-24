using SFML.Window;
using System;

namespace SFML_Test
{
    public class InputHandler
    {
        public void Input(Window window, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }

        public void ExitButton(Window window, EventArgs e)
        {
            window.Close();
        }
    }
}
