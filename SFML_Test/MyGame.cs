using SFML.Graphics;
using SFML.System;
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

        RectangleShape leftPlayer;
        RectangleShape rightPlayer;

        Font baseFont;

        public static int PLAYER_LEFT_SCORE = 0;
        public static int PLAYER_RIGHT_SCORE = 0;

        public MyGame(uint width, uint height, string title)
        {
            LoadFont();
            mainSFMLInputHandler = new InputHandler();
            var mode = new VideoMode(width, height);
            mainSFMLWindow = new RenderWindow(mode, title);
            mainSFMLWindow.KeyPressed += WindowKeyPressed;
            mainSFMLWindow.Closed += WindowClosed;

            mainSFMLWindow.SetVerticalSyncEnabled(true);
            GameMainLoop();
        }

        private void LoadFont()
        {
            baseFont = new Font("data/fonts/Fipps-Regular.otf");
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            mainSFMLInputHandler.ExitButton((Window)sender, e);
        }

        private void WindowKeyPressed(object sender, KeyEventArgs e)
        {
            mainSFMLInputHandler.Input((Window)sender, e);
        }

        public void GameMainLoop()
        {
            leftPlayer = new RectangleShape(new Vector2f(16, 64));
            leftPlayer.Position = new Vector2f(32, 300);

            rightPlayer = new RectangleShape(new Vector2f(16, 64));
            rightPlayer.Position = new Vector2f(752, 300);

            while (mainSFMLWindow.IsOpen)
            {
                mainSFMLWindow.DispatchEvents();

                DrawPlayer();
                DrawMiddleLine();
                DrawScore();

                mainSFMLWindow.Display();
                mainSFMLWindow.Clear();
            }
        }

        private void DrawScore()
        {
            Text scorePlayerLeft = new Text(PLAYER_LEFT_SCORE.ToString(), baseFont);
            scorePlayerLeft.CharacterSize = 24;
            scorePlayerLeft.FillColor = Color.White;
            scorePlayerLeft.Position = new Vector2f(200, 96);
            scorePlayerLeft.Draw(mainSFMLWindow, RenderStates.Default);

            Text scorePlayerRight = new Text(PLAYER_RIGHT_SCORE.ToString(), baseFont);
            scorePlayerRight.CharacterSize = 24;
            scorePlayerRight.FillColor = Color.White;
            scorePlayerRight.Position = new Vector2f(600, 96);
            scorePlayerRight.Draw(mainSFMLWindow, RenderStates.Default);
        }

        private void DrawMiddleLine()
        {
            for (int i = -64; i < 600; i += 32)
            {
                RectangleShape middleLineSegment = new RectangleShape(new Vector2f(16, 16));
                middleLineSegment.Position = new Vector2f(400, i);
                middleLineSegment.Draw(mainSFMLWindow, RenderStates.Default);
            }
        }

        private void DrawPlayer()
        {
            leftPlayer.Draw(mainSFMLWindow, RenderStates.Default);
            rightPlayer.Draw(mainSFMLWindow, RenderStates.Default);
        }
    }
}
