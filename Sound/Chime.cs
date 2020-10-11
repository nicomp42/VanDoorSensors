/* 
 *  https://freesound.org/
 */
using System;
using System.Threading;

namespace SoundNamespace {
    public static class Chime {
        private static CachedSound chime;
        private static Thread chimeThread;
        private static Boolean keepChiming = false;
        static Chime() {
            chimeThread = null;
            
            chime = new CachedSound("511397__pjhedman__se2-ding.wav");
            //chime = new CachedSound("484344__inspectorj__bike-bell-ding-single-01-01.wav");
            //chime = new CachedSound("17904__terminal__chime-16.wav");
        }
        public static void PlayChime() {
            if (chimeThread == null) {
                chimeThread = new Thread(playChime);
                chimeThread.Start();
            }
        }
        private static void playChime() {
            keepChiming = true;
            Console.WriteLine("\n******* Chime starting **********");
            while (keepChiming) {
                AudioPlaybackEngine.Instance.PlaySound(chime);
                Thread.Sleep(2000); // This is arbitary. We need to wait for the sound to finish
            }
            Console.WriteLine("\n******* Chime closing **********");
        }
        public static void StopChime() {
            if (chimeThread != null) {
                chimeThread.Abort();        // Doesn't seem to work
                keepChiming = false;
            }
            chimeThread = null;
        }
    }
}
