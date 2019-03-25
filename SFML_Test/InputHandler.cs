using SFML.Window;
using System;

namespace SFML_Test
{
    public class InputHandler
    {
        public void Input(Window window, KeyEventArgs e,ref Scenes currentScene)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                switch(currentScene)
                {
                    case Scenes.Game:
                        MyGame.gotLobbyList = false;
                        currentScene = Scenes.Menu;
                        break;
                    case Scenes.Lobby:
                        MyGame.gotLobbyList = false;
                        currentScene = Scenes.LobbyList;
                        break;
                    case Scenes.LobbyList:
                        MyGame.gotLobbyList = false;
                        currentScene = Scenes.Menu;
                        break;
                    case Scenes.Single:
                        MyGame.gotLobbyList = false;
                        currentScene = Scenes.Menu;
                        break;
                    case Scenes.Menu:
                        MyGame.connection.CloseConnection();
                        window.Close();
                        break;
                }
                
            }
        }
    }
}
