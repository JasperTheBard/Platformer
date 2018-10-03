using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class sprite
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public Vector2 offSet = Vector2.Zero;

        Texture2D texture;

        public int width = 0;
        public int height = 0;

        //the edges of the sprite
        public int leftEdge = 0;
        public int rightEdge = 0;
        public int topEdge = 0;
        public int bottomEdge = 0;


        public sprite()
        {

        }

        public void Load(ContentManager content, string assist, bool useOffset)
        {
            texture = content.Load<Texture2D>(assist);
            width = texture.Bounds.Width;
            height = texture.Bounds.Height;
            if (useOffset == true)
            {
                offSet = new Vector2(leftEdge + width / 2, topEdge + height / 2);
            }

            UpdateHitBox();

        }

        public void UpdateHitBox()
        {
            leftEdge = (int)position.X - (int)offSet.X;
            rightEdge = (int)position.X + width;
            topEdge = (int)position.Y - (int)offSet.Y;
            bottomEdge = topEdge + height;
        }

        public void Update(float deltaTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position - offSet, Color.White);
        }
    }
}
