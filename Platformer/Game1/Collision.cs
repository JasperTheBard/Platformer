using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Collision
    {
        public Game1 game;

        public bool IsColliding(sprite hero, sprite otherSprite)
        {
            //compare the position of each rectangle edges against the other
            //it compares opposite edges of each rectangle
            if (hero.rightEdge < otherSprite.leftEdge ||
                hero.leftEdge > otherSprite.rightEdge ||
                hero.bottomEdge < otherSprite.topEdge ||
                hero.topEdge > otherSprite.bottomEdge)
            {
                //rect is colliding
                return false;
            }
            return true;
            //not colliding
        }

        bool CheckForTile(Vector2 coordinates) // Checks if there is a tile at the specified coordinates
        {
            int column = (int)coordinates.X;
            int row = (int)coordinates.Y;

            if (column < 0 || column > game.levelTileWidth - 1)
            {
                return false;
            }
            if (row < 0 || row > game.levelTileHeight - 1)
            {
                return true;
            }

            sprite tileFound = game.levelGrid[column, row];

            if (tileFound != null)
            {
                return true;
            }

            return false;
        }
        sprite CollideLeft(sprite hero, Vector2 tileIndex, sprite playerPrediction)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPrediction, tile) == true && hero.velocity.X < 0)
            {
                hero.position.X = tile.rightEdge + hero.offSet.X;
                hero.velocity.X = 0;
            }

            return hero;
        }

        sprite CollideRight(sprite hero, Vector2 tileIndex, sprite playerPrediction)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPrediction, tile) == true && hero.velocity.X > 0)
            {
                hero.position.X = tile.leftEdge - hero.width + hero.offSet.X;
                hero.velocity.X = 0;
            }

            return hero;
        }

        sprite CollideAbove(sprite hero, Vector2 tileIndex, sprite playerPrediction)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPrediction, tile) == true && hero.velocity.Y < 0)
            {
                hero.position.Y = tile.bottomEdge + hero.offSet.Y;
                hero.velocity.Y = 0;
            }

            return hero;
        }

        sprite CollideBelow(sprite hero, Vector2 tileIndex, sprite playerPrediction)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPrediction, tile) == true && hero.velocity.Y > 0)
            {
                hero.position.Y = tile.topEdge - hero.height + hero.offSet.Y;
                hero.canJump = true;
            }

            return hero;
        }

        sprite CollideBottomDiagonals(sprite hero, Vector2 tileIndex, sprite playerPrediction)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int rightEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int topEdgeDistance = Math.Abs(tile.topEdge - playerPrediction.bottomEdge);
            int bottomEdgeDistance = Math.Abs(tile.bottomEdge - playerPrediction.topEdge);

            if (IsColliding(playerPrediction, tile) == true)
            {
                if (topEdgeDistance < rightEdgeDistance && topEdgeDistance < leftEdgeDistance)
                {
                    
                    hero.position.Y = tile.topEdge - hero.height + hero.offSet.Y;
                    hero.canJump = true;
                    hero.velocity.Y = 0;

                }
                else if (rightEdgeDistance < leftEdgeDistance)
                {
                    
                    hero.position.X = tile.rightEdge + hero.offSet.X;
                    //hero.velocity.X = 0;
                }
                else
                {
                   
                    hero.position.X = tile.leftEdge - hero.width + hero.offSet.X;
                    //hero.velocity.X = 0;
                }
            }

            return hero;
        }

        sprite CollideAboveDiagonals(sprite hero, Vector2 tileIndex, sprite playerPrediction)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int rightEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int topEdgeDistance = Math.Abs(tile.topEdge - playerPrediction.bottomEdge);
            int bottomEdgeDistance = Math.Abs(tile.bottomEdge - playerPrediction.topEdge);

            if (IsColliding(playerPrediction, tile) == true)
            {
                if (bottomEdgeDistance < leftEdgeDistance && bottomEdgeDistance < rightEdgeDistance)
                {
                    hero.position.Y = tile.bottomEdge + hero.offSet.Y;
                    //hero.velocity.Y = 0;
                }
                else if (leftEdgeDistance < rightEdgeDistance)
                {
                    hero.position.X = tile.rightEdge + hero.offSet.X;
                    //hero.velocity.X = 0;
                }
                else
                {
                    hero.position.X = tile.leftEdge - hero.width + hero.offSet.X;
                    //hero.velocity.X = 0;
                }
            }
               

            return hero;
        }

        public sprite CollideWithPlatforms(sprite hero, float deltaTime)
        {
            //Create a copy of the hero that will move to where the hero will be in the naxt frame
            sprite playerPrediction = new sprite();
            playerPrediction.position = hero.position;
            playerPrediction.width = hero.width;
            playerPrediction.height = hero.height;
            playerPrediction.offSet = hero.offSet;
            playerPrediction.UpdateHitBox();

            playerPrediction.position += hero.velocity * deltaTime;

            int playerColumn = (int)Math.Round(playerPrediction.position.X / game.tileHeight);
            int playerRow = (int)Math.Round(playerPrediction.position.Y / game.tileHeight);

            Vector2 playerTile = new Vector2(playerColumn, playerRow);

            Vector2 leftTile = new Vector2(playerTile.X - 1, playerTile.Y);
            Vector2 rightTile = new Vector2(playerTile.X + 1, playerTile.Y);
            Vector2 topTile = new Vector2(playerTile.X, playerTile.Y - 1);
            Vector2 bottomTile = new Vector2(playerTile.X, playerTile.Y + 1);

            Vector2 bottomLeftTile = new Vector2(playerTile.X - 1, playerTile.Y + 1);
            Vector2 bottomRightTile = new Vector2(playerTile.X + 1, playerTile.Y + 1);
            Vector2 topLeftTile = new Vector2(playerTile.X - 1, playerTile.Y - 1);
            Vector2 topRightTile = new Vector2(playerTile.X + 1, playerTile.Y - 1);
            // ....This allows us to predict if the hero will be overlapping an obstacle
            bool leftCheck = CheckForTile(leftTile);
            bool rightCheck = CheckForTile(rightTile);
            bool topCheck = CheckForTile(topTile);
            bool bottomCheck = CheckForTile(bottomTile);

            bool bottomLeftCheck = CheckForTile(bottomLeftTile);
            bool bottomRightCheck = CheckForTile(bottomRightTile);
            bool topLeftCheck = CheckForTile(topLeftTile);
            bool topRightCheck = CheckForTile(topRightTile);
            if (leftCheck == true) // check for collisions with the tiles left of the player
            {
                hero = CollideLeft(hero, leftTile, playerPrediction);
            }

            if (rightCheck == true)
            {
                hero = CollideRight(hero, rightTile, playerPrediction);
            }
            if (bottomCheck == true)
            {
                hero = CollideBelow(hero, bottomTile, playerPrediction);
            }
            if (topCheck == true)
            {
                hero = CollideAbove(hero, topTile, playerPrediction);
            }
            if (leftCheck == false && bottomCheck == false && bottomLeftCheck == true)
            {
                hero = CollideBottomDiagonals(hero, bottomLeftTile, playerPrediction);
            }
            if (rightCheck == false && bottomCheck == false && bottomRightCheck == true)
            {
                hero = CollideBottomDiagonals(hero, bottomRightTile, playerPrediction);
            }
            if(leftCheck == false && topCheck == false && topLeftCheck == true)
            {
                hero = CollideAboveDiagonals(hero, topLeftTile, playerPrediction);
            }
            if(rightCheck == false && topCheck == false && topRightCheck == true)
            {
                hero = CollideAboveDiagonals(hero, topRightTile, playerPrediction);
            }
            return hero;
        }

        public sprite CollideWithMonster(Player hero, Enemie monster, float deltaTime, Game1 theGame)
        {
            sprite playerPrediction = new sprite();
            playerPrediction.position = hero.playerSprite.position;
            playerPrediction.width = hero.playerSprite.width;
            playerPrediction.height = hero.playerSprite.height;
            playerPrediction.offSet = hero.playerSprite.offSet;
            playerPrediction.UpdateHitBox();

            playerPrediction.position += hero.playerSprite.velocity * deltaTime;
            if (IsColliding(hero.playerSprite, monster.enemieSprite))
            {
                int leftEdgeDistance = Math.Abs(monster.enemieSprite.leftEdge - playerPrediction.rightEdge);
                int rightEdgeDistance = Math.Abs(monster.enemieSprite.rightEdge - playerPrediction.leftEdge);
                int topEdgeDistance = Math.Abs(monster.enemieSprite.topEdge - playerPrediction.bottomEdge);
                int bottomEdgeDistance = Math.Abs(monster.enemieSprite.bottomEdge - playerPrediction.topEdge);

                if (topEdgeDistance < leftEdgeDistance && topEdgeDistance < rightEdgeDistance && topEdgeDistance < bottomEdgeDistance)
                {
                    //kill enemy
                    theGame.enemies.Remove(monster);
                    hero.playerSprite.velocity.Y -= hero.jumpStrength * deltaTime;
                    hero.playerSprite.canJump = false;
                }
                else
                {
                    //kill player
                      theGame.Exit();
                }
            }
            
            return hero.playerSprite;
        }

    }
}

