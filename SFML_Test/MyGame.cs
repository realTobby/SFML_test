using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyEventArgs = SFML.Window.KeyEventArgs;

namespace SFML_Test
{
    public enum Scenes
    {
        Game,
        LobbyList,
        Menu,
        Single,
        Lobby
    }

    public class MyGame
    {
        public static Font baseFont;

        public static bool gotLobbyList = false;

        public static SocketConnection connection = new SocketConnection();
        public static Scenes CurrentScene = Scenes.Menu;
        RenderWindow mainSFMLWindow;
        InputHandler mainSFMLInputHandler;

        PlayerPaddle leftPlayer;
        PlayerPaddle rightPlayer;

        public ColorSlider COLOR_SLIDER_RED;
        public ColorSlider COLOR_SLIDER_GREEN;
        public ColorSlider COLOR_SLIDER_BLUE;

        int PLAYER_COLOR_RED = 0;
        int PLAYER_COLOR_GREEN = 0;
        int PLAYER_COLOR_BLUE = 0;

        public static bool VSYNC = true;
        public List<string> LOBBIES = new List<string>();

        public static int PLAYER_LEFT_SCORE = 0;
        public static int PLAYER_RIGHT_SCORE = 0;

        public MyGame(uint width, uint height, string title)
        {
            leftPlayer = new PlayerPaddle();
            rightPlayer = new PlayerPaddle();


            LoadFont();
            mainSFMLInputHandler = new InputHandler();
            var mode = new VideoMode(width, height);
            mainSFMLWindow = new RenderWindow(mode, title,Styles.None);
            mainSFMLWindow.KeyPressed += WindowKeyPressed;
            mainSFMLWindow.MouseButtonReleased += WindowMouseButtonReleased;
            mainSFMLWindow.MouseMoved += WindowMouseMoved;

            mainSFMLWindow.SetVerticalSyncEnabled(VSYNC);
            GameMainLoop();
        }

        private void WindowMouseMoved(object sender, MouseMoveEventArgs e)
        {
            
        }

        private void WindowMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (CurrentScene == Scenes.LobbyList)
            {
                // lobby button input
                if (e.Button == Mouse.Button.Left)
                {
                    int mouse_pos_x = Mouse.GetPosition(mainSFMLWindow).X;
                    int mouse_pos_y = Mouse.GetPosition(mainSFMLWindow).Y;
                    Console.WriteLine("[DEBUG] LOBBYLIST: " + mouse_pos_x + " / " + mouse_pos_y);

                }
            }
            if (CurrentScene == Scenes.Menu)
            {
                // menu button input
                if (e.Button == Mouse.Button.Left)
                {
                    int mouse_pos_x = Mouse.GetPosition(mainSFMLWindow).X;
                    int mouse_pos_y = Mouse.GetPosition(mainSFMLWindow).Y;

                    Console.WriteLine("[DEBUG] MENU: " + mouse_pos_x + " / " + mouse_pos_y);
                    // Multi Button
                    if (mouse_pos_x > 125 && mouse_pos_y > 151 && mouse_pos_x < 319 && mouse_pos_y < 207)
                    {
                        CurrentScene = Scenes.LobbyList;
                    }

                    // Single Button
                    if (mouse_pos_x > 126 && mouse_pos_y > 250 && mouse_pos_x < 322 && mouse_pos_y < 310)
                    {
                        CurrentScene = Scenes.Single;
                    }

                    // Exit Button
                    if (mouse_pos_x > 126 && mouse_pos_y > 350 && mouse_pos_x < 322 && mouse_pos_y < 411)
                    {
                        connection.CloseConnection();
                        mainSFMLWindow.Close();
                    }
                }
            }
        }

        private void LoadFont()
        {
            baseFont = new Font("data/fonts/Fipps-Regular.otf");
        }

        private void WindowKeyPressed(object sender, KeyEventArgs e)
        {
            mainSFMLInputHandler.Input((Window)sender, e, ref CurrentScene);
        }

        public void GameMainLoop()
        {
            COLOR_SLIDER_RED = new ColorSlider(mainSFMLWindow);
            COLOR_SLIDER_RED.SetPostion(64, 100);
            COLOR_SLIDER_RED.Init(255,0,0);

            COLOR_SLIDER_GREEN = new ColorSlider(mainSFMLWindow);
            COLOR_SLIDER_GREEN.SetPostion(64, 150);
            COLOR_SLIDER_GREEN.Init(0,255,0);

            COLOR_SLIDER_BLUE = new ColorSlider(mainSFMLWindow);
            COLOR_SLIDER_BLUE.SetPostion(64, 200);
            COLOR_SLIDER_BLUE.Init(0,0,255);






            leftPlayer.SetShape(new RectangleShape(new Vector2f(16, 64)));
            leftPlayer.POS_X = 32;
            leftPlayer.POS_Y = 300;

            rightPlayer.SetShape(new RectangleShape(new Vector2f(16, 64)));
            rightPlayer.POS_X = 752;
            rightPlayer.POS_Y = 300;

            while (mainSFMLWindow.IsOpen)
            {
                mainSFMLWindow.DispatchEvents();
                mainSFMLWindow.Clear();
                if (CurrentScene == Scenes.Menu)
                {

                    DrawMenuTitle();
                    DrawMiddleLine();
                    DrawSinglePlayerButton();
                    DrawMultiPlayerButton();
                    DrawExitButton();
                }

                if (CurrentScene == Scenes.Game)
                {
                    HandleMousePostion();

                    GetPlayerRightPosition();
                    GetPlayerRightScore();

                    DrawPlayer();
                    DrawMiddleLine();
                    DrawScore();
                }

                if(CurrentScene == Scenes.Single)
                {
                    HandleMousePostion();
                    DrawPlayer();
                    DrawMiddleLine();
                    DrawScore();
                }

                if (CurrentScene == Scenes.LobbyList)
                {
                    if(gotLobbyList == false)
                        LOBBIES = connection.GetLobbyList();

                    DrawLobbyTitle();
                    DrawLobbyList();
                    DrawLobbyUserSettings();
                }
                mainSFMLWindow.Display();
            }
        }

        private void DrawLobbyUserSettings()
        {
            COLOR_SLIDER_RED.Draw();
            COLOR_SLIDER_GREEN.Draw();
            COLOR_SLIDER_BLUE.Draw();
           
            RectangleShape playerNameInput = new RectangleShape(new Vector2f(350, 32));
            playerNameInput.OutlineColor = Color.Magenta;
            playerNameInput.OutlineThickness = 1;
            playerNameInput.FillColor = Color.White;
            playerNameInput.Position = new Vector2f(64, 100);
            playerNameInput.Draw(mainSFMLWindow, RenderStates.Default);
        }

        private void DrawLobbyList()
        {
            if(LOBBIES == null)
            {
                CurrentScene = Scenes.Menu;
                MessageBox.Show("Der Server ist offline.");
            }
            else
            {
                if (LOBBIES.Count != 0)
                {
                    int y = 200;
                    foreach (var lobby in LOBBIES)
                    {
                        Text lobbyName = new Text(lobby, baseFont);
                        lobbyName.FillColor = Color.White;
                        lobbyName.CharacterSize = 25;
                        lobbyName.Position = new Vector2f(260, y);
                        lobbyName.Draw(mainSFMLWindow, RenderStates.Default);
                        y += 31;
                    }
                }
            }
            
        }

        private void DrawLobbyTitle()
        {
            Text exitButtonText = new Text("Pong Lobbys", baseFont);
            exitButtonText.FillColor = Color.White;
            exitButtonText.CharacterSize = 36;
            exitButtonText.Position = new Vector2f(64, 32);
            exitButtonText.Draw(mainSFMLWindow, RenderStates.Default);
        }

        private void HandleMousePostion()
        {
            int mouse_pos_y = Mouse.GetPosition(mainSFMLWindow).Y;
            leftPlayer.POS_Y = mouse_pos_y - 32;
        }

        private void DrawSinglePlayerButton()
        {
            RectangleShape singleButton = new RectangleShape(new Vector2f(200, 64));
            singleButton.OutlineColor = Color.White;
            singleButton.OutlineThickness = 1;
            singleButton.FillColor = Color.Black;
            singleButton.Position = new Vector2f(125, 250);
            singleButton.Draw(mainSFMLWindow, RenderStates.Default);

            Text singleButtonText = new Text("Single", baseFont);
            singleButtonText.FillColor = Color.White;
            singleButtonText.CharacterSize = 24;
            singleButtonText.Position = new Vector2f(175, 275);
            singleButtonText.Draw(mainSFMLWindow, RenderStates.Default);
        }

        private void DrawExitButton()
        {
            RectangleShape exitButton = new RectangleShape(new Vector2f(200, 64));
            exitButton.OutlineColor = Color.White;
            exitButton.OutlineThickness = 1;
            exitButton.FillColor = Color.Black;
            exitButton.Position = new Vector2f(125, 350);
            exitButton.Draw(mainSFMLWindow, RenderStates.Default);

            Text exitButtonText = new Text("Exit", baseFont);
            exitButtonText.FillColor = Color.White;
            exitButtonText.CharacterSize = 24;
            exitButtonText.Position = new Vector2f(175, 375);
            exitButtonText.Draw(mainSFMLWindow, RenderStates.Default);
        }

        private void DrawMultiPlayerButton()
        {
            RectangleShape multiButton = new RectangleShape(new Vector2f(200, 64));
            multiButton.OutlineColor = Color.White;
            multiButton.OutlineThickness = 1;
            multiButton.FillColor = Color.Black;
            multiButton.Position = new Vector2f(125, 150);
            multiButton.Draw(mainSFMLWindow,RenderStates.Default);

            Text multiButtonText = new Text("Multi", baseFont);
            multiButtonText.FillColor = Color.White;
            multiButtonText.CharacterSize = 24;
            multiButtonText.Position = new Vector2f(175, 175);
            multiButtonText.Draw(mainSFMLWindow, RenderStates.Default);

        }

        private void DrawMenuTitle()
        {
            Text menuTitle = new Text("PONG", baseFont);
            menuTitle.CharacterSize = 36;
            menuTitle.FillColor = Color.White;
            menuTitle.Position = new Vector2f(150, 50);
            menuTitle.Draw(mainSFMLWindow, RenderStates.Default);
        }

        private void GetPlayerRightScore()
        {
            // service code goes here
        }

        private void GetPlayerRightPosition()
        {
            // service code goes here
        }

        private void GetBallPosition()
        {

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
            for (int i = -64; i < 600; i += 24)
            {
                RectangleShape middleLineSegment = new RectangleShape(new Vector2f(8, 8));
                middleLineSegment.Position = new Vector2f(400, i);
                middleLineSegment.Draw(mainSFMLWindow, RenderStates.Default);
            }
        }

        private void DrawPlayer()
        {
            leftPlayer.GetShape().Draw(mainSFMLWindow, RenderStates.Default);
            rightPlayer.GetShape().Draw(mainSFMLWindow, RenderStates.Default);
        }
    }
}
