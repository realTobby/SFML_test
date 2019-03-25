using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace SFML_Test
{
    public class ColorSlider
    {
        private RenderWindow mainSFMLWindow;
        private int POS_X { get; set; }
        private int POS_Y { get; set; }

        Text colorNumber;
        ConvexShape leftButton;
        ConvexShape rightButton;

        int number { get; set; } = 0;

        int r = 0;
        int g = 0;
        int b = 0;


        void SetTextNumber(int input)
        {
            number = input;
        }

        public void SetRGB(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public void SetPostion(int x, int y)
        {
            POS_X = x;
            POS_Y = y;
        }

        public ColorSlider(RenderWindow mainSFMLWindow)
        {
            this.mainSFMLWindow = mainSFMLWindow;
            
        }

        private void InitText()
        {
            colorNumber = new Text(number.ToString(), MyGame.baseFont);
            colorNumber.FillColor = Color.White;
            colorNumber.CharacterSize = 12;
            colorNumber.Position = new Vector2f(POS_X, POS_Y);
        }

        private void InitRightButton()
        {
            int rightPosX = POS_X + 32;
            int rightPosY = POS_Y;

            rightButton = new ConvexShape(3);
            rightButton.FillColor = GetColor(); 
            rightButton.Position = new Vector2f(rightPosX, rightPosY);
            rightButton.SetPoint(0, new Vector2f(rightPosX + 16, rightPosY));
            rightButton.SetPoint(1, new Vector2f(rightPosX - 16, rightPosY - 16));
            rightButton.SetPoint(2, new Vector2f(rightPosX - 16, rightPosY + 16));
        }

        private Color GetColor()
        {
            return new Color(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b), 255);
        }

        private void InitLeftButton()
        {
            int leftPosX = POS_X - 32;
            int leftPosY = POS_Y;
            

            leftButton = new ConvexShape(3);
            leftButton.FillColor = GetColor();
            leftButton.Position = new Vector2f(leftPosX, leftPosY);
            leftButton.SetPoint(0, new Vector2f(leftPosX - 16, leftPosY));
            leftButton.SetPoint(1, new Vector2f(leftPosX + 16, leftPosY - 16));
            leftButton.SetPoint(2, new Vector2f(leftPosX + 16, leftPosY + 16));
        }

        public void Draw()
        {
            colorNumber.Draw(mainSFMLWindow, RenderStates.Default);
            leftButton.Draw(mainSFMLWindow, RenderStates.Default);
            rightButton.Draw(mainSFMLWindow, RenderStates.Default);
        }

        public void Init(int r, int g, int b)
        {
            SetRGB(r, g, b);
            InitLeftButton();
            InitRightButton();
            InitText();
        }



    }
}
