/*
 * Bill Nicholson
 * nicholdw@ucmail.uc.edu
 * Refer to C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1 for 
 *   the SynchronizedCollection data structure.
 * Refer to https://stackoverflow.com/questions/7511199/system-servicemodel-dll-missing-in-references-visual-studio-2010 also.
 */
using SoundNamespace;
using System;
using System.Collections.Generic;
using System.Threading;
using VanDoorNamespace;
using VanDoorSensorNamespace;

namespace VanDoorSensorDemoNamespace {
    class Program {
        private static bool isChiming;
        static void Main(string[] args) {
            isChiming = false;
//          Chime.PlayChime(); Thread.Sleep(5000);
            VanDoorSensor vanDoorSensor = new VanDoorSensor();
            Thread vanDoorSensorThread = vanDoorSensor.StartSensor("My Toyota", ProcessSensorOutput, 0);    // Demo mode
            // Do whatever you want here. Your sensor will call you when something happens. 
            // ...
            for (int i = 0; i < 10; i++) { Console.WriteLine("Working "); Thread.Sleep(1000); }

            // If you get to the end of the main and you don't want the app to end yet, join the Van Door Sensor thread.
            vanDoorSensorThread.Join();
            Console.WriteLine("Done");
            //AudioPlaybackEngine.Instance.Dispose();

        }
        /// <summary>
        /// This method will be called by the sensor when a door status changes. 
        /// It happens in real time. The main can go about whatever business it wants to do.
        /// </summary>
        /// <param name="vanDoors">The states of the doors</param>
        private static void ProcessSensorOutput(SynchronizedCollection<VanDoor> vanDoors) {
            Console.WriteLine("Door status changed");
            for (int i = 0; i < vanDoors.Count; i++) {
                Console.WriteLine("  " + vanDoors[i].doorType.ToString() + ": " + vanDoors[i].doorStatus.ToString());
                if (vanDoors[i].doorType.ToString() == "DriverSide" && vanDoors[i].doorStatus == VanDoor.DoorStatus.Open) {
                    if (!isChiming) {
                        Chime.PlayChime();
                    }
                    isChiming = true;
                }
                if (vanDoors[i].doorType.ToString() == "DriverSide" && vanDoors[i].doorStatus == VanDoor.DoorStatus.Closed) {
                    Chime.StopChime();
                    isChiming = false;
                }
            }
        }
    }
}
