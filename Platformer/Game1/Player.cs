﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace Game1
{
    public class Player
    {
        public sprite playerSprite = new sprite();

        Game1 game = null;

        float runSpeed = 250;
        float maxRunSpeed = 500;
        float friction = 500;
        float terminalVelocity = 500;
        public float jumpStrength = 50000;

        Collision collision = new Collision();

        SoundEffect jumpSound;
        SoundEffectInstance jumpSoundInst;

        public Player()
        {

        }

        public void Load(ContentManager content, Game1 TheGame)
        {
            playerSprite.Load(content, "hero", true);

            AnimatedTexture animation = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animation.Load(content, "walk", 12, 20);
            playerSprite.AddAnimation(animation, 0, -5);
            playerSprite.Pause();

            jumpSound = content.Load<SoundEffect>("JumpNoise");
            jumpSoundInst = jumpSound.CreateInstance();

            playerSprite.offSet = new Vector2(24,24);
            game = TheGame;
            playerSprite.velocity = Vector2.Zero;
            playerSprite.position = new Vector2(128, 4992); //TheGame.GraphicsDevice.Viewport.Width / 2
        }

        private void UpdateInput(float deltaTime)
        {
            bool wasMovingLeft = playerSprite.velocity.X < 0;
            bool wasMovingRight = playerSprite.velocity.X > 0;

            Vector2 localAcceleration = game.gravity;
            if (Keyboard.GetState().IsKeyDown(Keys.Left) == true || Keyboard.GetState().IsKeyDown(Keys.A) == true)
            {
                localAcceleration.X += -runSpeed;
                playerSprite.SetFlipped(true);
                playerSprite.Play();
            }
            else if (wasMovingLeft == true)
            {
                localAcceleration.X += friction;
                playerSprite.Pause();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true || Keyboard.GetState().IsKeyDown(Keys.D) == true)
            {
                localAcceleration.X += runSpeed;
                playerSprite.SetFlipped(false);
                playerSprite.Play();
            }
            else if (wasMovingRight == true)
            {
                localAcceleration.X += -friction;
                playerSprite.Pause();
            }
            //if(Keyboard.GetState().IsKeyDown(Keys.Up) == true || Keyboard.GetState().IsKeyDown(Keys.W) == true)
            //{
            //    localAcceleration.Y = -runSpeed;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.Down) == true || Keyboard.GetState().IsKeyDown(Keys.S) == true)
            //{
            //    localAcceleration.Y = runSpeed;
            //}
            if (Keyboard.GetState().IsKeyUp(Keys.Left) == true && Keyboard.GetState().IsKeyUp(Keys.Right) == true && 
                Keyboard.GetState().IsKeyUp(Keys.A) == true && Keyboard.GetState().IsKeyUp(Keys.D) == true)
            {
                playerSprite.Pause();
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Space) == true && playerSprite.canJump == true)
            {
                playerSprite.canJump = false;
                localAcceleration.Y -= jumpStrength;
                jumpSoundInst.Play();
            }
            //foreach (sprite tile in game.allCollisionTiles)
            //{
            //    if(collision.IsColliding(playerSprite,tile) == true)
            //    {
            //        int testVariable = 0;
            //    }
            //}


            playerSprite.velocity += localAcceleration * deltaTime;

            if (playerSprite.velocity.X > maxRunSpeed)
            {
                playerSprite.velocity.X = maxRunSpeed;
            }
            else if(playerSprite.velocity.X < -maxRunSpeed)
            {
                playerSprite.velocity.X = -maxRunSpeed;
            }

            if(wasMovingLeft && (playerSprite.velocity.X > 0) || wasMovingRight && (playerSprite.velocity.X < 0))
            {
                //clamp at 0 to prevent friction from making the hero slide
                playerSprite.velocity.X = 0;
            }

            if (playerSprite.velocity.Y > terminalVelocity)
            {
                playerSprite.velocity.Y = terminalVelocity;
            }

            playerSprite.position += playerSprite.velocity * deltaTime;

            collision.game = game;
            playerSprite = collision.CollideWithPlatforms(playerSprite, deltaTime);
        }

        public void Update(float deltaTime)
        {
            UpdateInput(deltaTime);
            playerSprite.Update(deltaTime);
            playerSprite.UpdateHitBox();
            if (collision.IsColliding(playerSprite, game.goal.chestSprite))
            {
                game.Exit();
                int temp = 1;
            }
            for (int i = 0; i < game.enemies.Count; i++)
            {
                playerSprite = collision.CollideWithMonster(this, game.enemies[i], deltaTime, game);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.Draw(spriteBatch);
        }
    }
}
