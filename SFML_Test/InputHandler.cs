using SFML.Window;
using System;

namespace SFML_Test
{
    public class InputHandler
    {
        Logger logging = new Logger();

        public void Input(Window window, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                logging.Log(LogLevel.INFORMATION, "PRESSED ESCAPE KEY");
                CloseWindow(window);
            }
        }

        public void CloseWindow(Window window)
        {
            window.Close();
            logging.Log(LogLevel.CRITICAL, "WINDOW CLOSED");
        }
    }
}
