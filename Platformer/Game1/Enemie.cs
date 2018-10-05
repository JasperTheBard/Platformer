using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class Enemie
    {
        float walkSpeed = 7500;
        public sprite enemieSprite = new sprite();
        Collision collision = new Collision();
        Game1 game = null;

        public void Load(ContentManager content, Game1 game)
        {
            this.game = game;

            AnimatedTexture animation = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animation.Load(content, "zombie", 4, 5);

            enemieSprite.AddAnimation(animation, 16, 0);
            enemieSprite.width = 64;
            enemieSprite.height = 64;
            enemieSprite.offSet = new Vector2(8, 8);
        }

        public void Update(float deltaTime)
        {
            //move the enemy
            enemieSprite.velocity = new Vector2(walkSpeed, 0) * deltaTime;
            enemieSprite.position += enemieSprite.velocity * deltaTime;
            collision.game = game;
            enemieSprite = collision.CollideWithPlatforms(enemieSprite, deltaTime);
            if (enemieSprite.velocity.X == 0)
            {
                walkSpeed *= -1;
            }
            enemieSprite.UpdateHitBox();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            enemieSprite.Draw(spriteBatch);
        }

    }
}
