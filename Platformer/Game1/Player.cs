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
    class Player
    {
        public sprite playerSprite = new sprite();

        Game1 game = null;
        public Player()
        {

        }

        public void Load(ContentManager content, Game1 TheGame)
        {
            playerSprite.Load(content, "hero");
            game = TheGame; 

        }
        public void Update(float deltaTime)
        {
            playerSprite.Update(deltaTime);



        }
        public void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.Draw(spriteBatch);
        }
    }
}
