using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Asteroid_Belt_Assault
{
    class CollisionManager //pg 151
    {
        private AsteroidManager asteroidManager;
        private PlayerManager playerManager;
        private EnemyManager enemyManager;
        private ExplosionManager explosionManager;
        private Vector2 offScreen = new Vector2(-500, -500); // used as shortcut for destroying some objects. It pushes them offscreen so program can delete them
        private Vector2 shotToAsteroidImpact = new Vector2(0, -20); //how much velocity each shot wil ltransfer to an asteroid
        private int enemyPointValue = 100; //when a shot hits an enemy, will add score

        //public constructor
        public CollisionManager(
            AsteroidManager asteroidManager,
            PlayerManager playerManager,
            EnemyManager enemyManager,
            ExplosionManager explosionManager)
        {
            this.asteroidManager = asteroidManager;
            this.playerManager = playerManager;
            this.enemyManager = enemyManager;
            this.explosionManager = explosionManager;
        }// end pg 151 code

        //this is the CollisionManager, this will have helper methods for the other 5 collsion codes
        //pg 153

        private void checkShotToEnemyCollisions()
        {
            foreach (Sprite shot in playerManager.PlayerShotManager.Shots)
            {
                foreach (Enemy enemy in enemyManager.Enemies)
                {
                    if (shot.IsCircleColliding(
                        enemy.EnemySprite.Center,
                        enemy.EnemySprite.CollisionRadius))
                    {
                        shot.Location = offScreen;
                        enemy.Destroyed = true;
                        playerManager.PlayerScore += enemyPointValue;
                        explosionManager.AddExplosion(
                            enemy.EnemySprite.Center,
                            enemy.EnemySprite.Velocity / 10);
                    }
                }
            }
        }

        private void checkShotToAsteroidCollisions()
        {
            foreach (Sprite shot in playerManager.PlayerShotManager.Shots)
            {
                foreach (Sprite asteroid in asteroidManager.Asteroids)
                {
                    if (shot.IsCircleColliding(
                        asteroid.Center,
                        asteroid.CollisionRadius))
                    {
                        shot.Location = offScreen;
                        asteroid.Velocity += shotToAsteroidImpact;
                    }
                }
            }
        }// end pg 153

        //pg 154, player collisions
        private void checkShotToPlayerCollisions()
        {
            foreach (Sprite shot in enemyManager.EnemyShotManager.Shots)
            {
                if (shot.IsCircleColliding(
                    playerManager.playerSprite.Center,
                    playerManager.playerSprite.CollisionRadius))
                {
                    shot.Location = offScreen;
                    playerManager.Destroyed = true;
                    explosionManager.AddExplosion(
                        playerManager.playerSprite.Center,
                        Vector2.Zero);
                }
            }
        }

        private void checkEnemyToPlayerCollisions()
        {
            foreach (Enemy enemy in enemyManager.Enemies)
            {
                if (enemy.EnemySprite.IsCircleColliding(
                    playerManager.playerSprite.Center,
                    playerManager.playerSprite.CollisionRadius))
                {
                    enemy.Destroyed = true;
                    explosionManager.AddExplosion(
                        enemy.EnemySprite.Center,
                        enemy.EnemySprite.Velocity / 10);

                    playerManager.Destroyed = true;

                    explosionManager.AddExplosion(
                        playerManager.playerSprite.Center,
                        Vector2.Zero);
                }
            }
        }

        private void checkAsteroidToPlayerCollisions()
        {
            foreach (Sprite asteroid in asteroidManager.Asteroids)
            {
                if (asteroid.IsCircleColliding(
                    playerManager.playerSprite.Center,
                    playerManager.playerSprite.CollisionRadius))
                {
                    explosionManager.AddExplosion(
                        asteroid.Center,
                        asteroid.Velocity/10);

                    asteroid.Location = offScreen;

                    playerManager.Destroyed = true;
                    explosionManager.AddExplosion(
                        playerManager.playerSprite.Center,
                        Vector2.Zero);
                }
            }
        } //end pg 155 code

        //pg 156, using the collision manager class

        public void CheckCollisions()
        {
            checkShotToEnemyCollisions();
            checkShotToAsteroidCollisions();
            if (!playerManager.Destroyed)
            {
                checkShotToPlayerCollisions();
                checkEnemyToPlayerCollisions();
                checkAsteroidToPlayerCollisions();
            }
        }






    }
}
