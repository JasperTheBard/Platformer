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

        bool CheckForTile(Vector2 coordinates) //checks if there is a tile at the specified coordinate
        {
            int column = (int)coordinates.X;
            int row = (int)coordinates.Y;

            if (column < 0 || column < game.levelTileWidth -1)
            {
                return false;
            }
            if (row < 0 || row < game.levelTileHeight - 1)
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

        sprite CollideLeft(sprite hero, Vector2 tileIndex, sprite playerPredictions)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPredictions, tile) == true && hero.velocity.X < 0)
            {
                hero.position.X = tile.rightEdge + hero.offSet.X;
                hero.velocity.X = 0;
            }
            return hero;
        }

        sprite CollideRight(sprite hero, Vector2 tileIndex, sprite playerPredictions)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPredictions, tile) == true && hero.velocity.X > 0)
            {
                hero.position.X = tile.leftEdge - hero.width + hero.offSet.X;
                hero.velocity.X = 0;
            }
            return hero;
        }

        sprite CollideBelowDiagonals(sprite hero, Vector2 tileIndex, sprite playerPredictions)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.leftEdge - playerPredictions.rightEdge);
            int rightEdgeDistance = Math.Abs(tile.rightEdge - playerPredictions.leftEdge);
            int topEdgeDistance = Math.Abs(tile.topEdge - playerPredictions.bottomEdge);
            if (IsColliding(playerPredictions, tile) == true)
            {
                if (topEdgeDistance < rightEdgeDistance && topEdgeDistance < leftEdgeDistance)
                {
                    hero.position.Y = tile.topEdge - hero.height + hero.offSet.Y;
                    hero.velocity.Y = 0;
                }
                else if (rightEdgeDistance < leftEdgeDistance)
                {
                    hero.position.X = tile.rightEdge + hero.offSet.X;
                    hero.velocity.X = 0;
                }
                else
                {
                    hero.position.X = tile.leftEdge - hero.width + hero.offSet.X;
                    hero.position.X = 0;
                }
            }

            return hero;
        }

        sprite CollideAboveDiagonals(sprite hero, Vector2 tileIndex, sprite playerPredictions)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.rightEdge - playerPredictions.leftEdge);
            int rightEdgeDistance = Math.Abs(tile.leftEdge - playerPredictions.rightEdge);
            int bottomEdgeDistance = Math.Abs(tile.topEdge - playerPredictions.bottomEdge);
            if (IsColliding(playerPredictions, tile) == true)
            {
                if (bottomEdgeDistance < rightEdgeDistance && bottomEdgeDistance < leftEdgeDistance)
                {
                    hero.position.Y = tile.bottomEdge + hero.offSet.Y;
                    hero.velocity.Y = 0;
                }
                else if (rightEdgeDistance < leftEdgeDistance)
                {
                    hero.position.X = tile.rightEdge + hero.offSet.X;
                    hero.velocity.X = 0;
                }
                else
                {
                    hero.position.X = tile.leftEdge + hero.offSet.X;
                    hero.position.X = 0;
                }
            }

            return hero;
        }

        sprite CollideAbove(sprite hero, Vector2 tileIndex, sprite playerPredictions)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPredictions, tile) == true && hero.velocity.Y < 0)
            {
                hero.position.Y = tile.topEdge + hero.offSet.Y;
                hero.velocity.Y = 0;
            }
            return hero;
        }

        sprite CollideBelow(sprite hero, Vector2 tileIndex, sprite playerPredictions)
        {
            sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPredictions, tile) == true && hero.velocity.Y > 0)
            {
                hero.position.Y = tile.bottomEdge - hero.height + hero.offSet.Y;
                hero.velocity.Y = 0;
            }
            return hero;
        }

        public sprite CollideWithPlatform(sprite hero, float deltaTime)
        {
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
            Vector2 bottomRightTile = new Vector2(playerTile.X + 1, playerTile.Y - 1);
            Vector2 topLeftTile = new Vector2(playerTile.X + 1, playerTile.Y - 1);
            Vector2 topRightTile = new Vector2(playerTile.X - 1, playerTile.Y + 1);
            //this allows us to predict if te hero will be overlaping an obsticle in the next frame

            bool LeftCheck = CheckForTile(leftTile);
            bool RightCheck = CheckForTile(rightTile);
            bool topCheck = CheckForTile(topTile);
            bool bottomCheck = CheckForTile(bottomTile);

            bool bottomLeftCheck = CheckForTile(bottomLeftTile);
            bool bottomRightCheck = CheckForTile(bottomRightTile);
            bool topLeftCheck = CheckForTile(topLeftTile);
            bool topRightCheck = CheckForTile(topRightTile);

            if(LeftCheck == true) //checks all collisions on the left side of the player
            {
                hero = CollideLeft(hero, leftTile, playerPrediction);
            }

            if (RightCheck == true) //checks all collisions on the right side of the player
            {
                hero = CollideRight(hero, rightTile, playerPrediction);
            }

            if (topCheck == true) //checks all collisions above the player
            {
                hero = CollideAbove(hero, topTile, playerPrediction);
            }

            if (bottomCheck == true) //checks all collisions below the player
            {
                hero = CollideBelow(hero, bottomTile, playerPrediction);
            }

            return hero;
        }

    }
}
