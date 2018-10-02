using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class sprite
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 offSet = Vector2.Zero;

        Texture2D texture;

        public sprite()
        {

        }

        public void Load(ContentManager content, string assist)
        {
            texture = content.Load<Texture2D>(assist);


        }
        public void Update(float deltaTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position = offSet, Color.White);
        }
    }
}
