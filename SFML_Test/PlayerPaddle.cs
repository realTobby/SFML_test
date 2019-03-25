using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_Test
{
    public class PlayerPaddle
    {
        private float pos_x;
        public float POS_X
        {
            get
            {
                return pos_x;
            }
            set
            {
                pos_x = value;
                SetShapePosition(value, POS_Y);
            }
        }

        private float pos_y;
        public float POS_Y
        {
            get
            {
                return pos_y;
            }
            set
            {
                pos_y = value;
                SetShapePosition(POS_X, value);
            }
        }

        private RectangleShape playerForm;

        public void SetShapePosition(float x, float y)
        {
            playerForm.Position = new SFML.System.Vector2f(x, y);
        }

        public RectangleShape GetShape()
        {
            return playerForm;
        }

        public void SetShape(RectangleShape newShape)
        {
            playerForm = newShape;
        }

        public void MoveUp()
        {
            if(POS_Y > 16)
                POS_Y = POS_Y -= 12f;
        }

        public void MoveDown()
        {
            if(POS_Y < 552)
            POS_Y = POS_Y += 12f;
        }
    }
}
