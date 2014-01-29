using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Asteroid_Belt_Assault // pg 158, adding sounds baby!
{
    public static class SoundManager
    {
        private static List<SoundEffect> explosions = new
            List<SoundEffect>();
        private static int explosionCount = 4;

        private static SoundEffect playerShot;
        private static SoundEffect enemyShot;

        //player movement sounds
        private static SoundEffect rightSound;
        private static SoundEffect leftSound;

        private static SoundEffectInstance leftsoundInstance;
        private static SoundEffectInstance rightsoundInstance;

        private static Random rand = new Random();

        public static void Initialize(ContentManager content)
        {
            try
            {
                playerShot = content.Load<SoundEffect>(@"Sounds\Shot1");
                enemyShot = content.Load<SoundEffect>(@"Sounds\Shot2");

                rightSound = content.Load<SoundEffect>(@"Sounds\RightSound");
                leftSound = content.Load<SoundEffect>(@"Sounds\LeftSound");

                leftsoundInstance = leftSound.CreateInstance();
                rightsoundInstance = rightSound.CreateInstance();



                for (int i = 1; i <= explosionCount; i++)
                {
                    explosions.Add(
                        content.Load<SoundEffect>(@"Sounds\Explosion" + i.ToString()));
                }
            }
            catch
            {
                Debug.Write("SoundManager Initialization Failed");
            }

        }


        public static void PlayExplosion(float pan)
        {
            try
            {


                explosions[rand.Next(0, explosionCount)].Play(1.0f, 0.0f, pan);


            }
            catch
            {
                Debug.Write("PlayExplosion Failed");
            }
        }

        public static void PlayPlayerShot(float pan)
        {
            try
            {
                playerShot.Play(1.0f, 0.0f, pan);
            }
            catch
            {
                Debug.Write("PlayPlayerShot Failed");
            }
        }

        public static void PlayEnemyShot(float pan)
        {
            try
            {
                enemyShot.Play(1.0f, 0.0f, pan);
            }
            catch
            {
                Debug.Write("PlayEnemyShot Failed");
            }
        }//end pg 159


        public static void PlayRightSound(float pan, bool canPlay)
        {
            rightsoundInstance.Pan = pan;
            rightsoundInstance.Volume = 0.2f; 

            if (canPlay == true)
            {
                try
                {
                    rightsoundInstance.Play();
                }
                catch
                {
                    Debug.Write("PlayPlayerShot Failed");
                }
            }
            else if (canPlay == false)
                rightsoundInstance.Stop();
        }

        public static void PlayLeftSound(float pan, bool canPlay)
        {
            leftsoundInstance.Pan = pan;
            leftsoundInstance.Volume = 0.2f;

            if (canPlay == true)
                try
                {
                    leftsoundInstance.Play();
                }
                catch
                {
                    Debug.Write("PlayPlayerShot Failed");
                }

            else if (canPlay == false)
                
                leftsoundInstance.Stop();
     
        }


        //pg 160, using the SoundManager class




    }
}
