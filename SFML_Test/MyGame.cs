using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_Test
{
    public class MyGame
    {
        RenderWindow mainSFMLWindow;
        InputHandler mainSFMLInputHandler;
        Logger logging = new Logger();

        public MyGame(uint width, uint height, string title)
        {
            mainSFMLInputHandler = new InputHandler();
            logging.Log(LogLevel.INFORMATION, "ASSIGNED INPUTHANDLER");
            var mode = new VideoMode(width, height);
            logging.Log(LogLevel.INFORMATION, "INIT VIDEOMODE");
            mainSFMLWindow = new RenderWindow(mode, title);
            logging.Log(LogLevel.INFORMATION, "CREATE WINDOW");
            mainSFMLWindow.KeyPressed += WindowKeyPressed;

            GameMainLoop();
            
        }

        private void WindowKeyPressed(object sender, KeyEventArgs e)
        {
            mainSFMLInputHandler.Input((Window)sender, e);
        }

        public void GameMainLoop()
        {
            logging.Log(LogLevel.INFORMATION, "STARTED GAME LOOP");
            while (mainSFMLWindow.IsOpen)
            {
                mainSFMLWindow.DispatchEvents();

                // game code here

                mainSFMLWindow.Display();
            }
        }


    }
}
