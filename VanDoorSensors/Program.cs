/*
 * Bill Nicholson
 * nicholdw@ucmail.uc.edu
 */
using System;
using System.Collections.Generic;
using System.Threading;
using VanDoorNamespace;
using VanDoorSensorNamespace;

namespace VanDoorSensorDemoNamespace {
    class Program {
        static void Main(string[] args) {
            
            VanDoorSensor vanDoorSensor = new VanDoorSensor();
            Thread vanDoorSensorThread = vanDoorSensor.StartSensor("My Toyota", ProcessSensorOutput, 0);    // Demo mode
            // Do whatever you want here. Your sensor will call you when something happens. 
            // ...
            for (int i = 0; i < 10; i++) { Console.Write("Working "); Thread.Sleep(1000); }

            // If you get to the end of the main and you don't want the app to end yet, join the Van Door Sensor thread.
            vanDoorSensorThread.Join();
            Console.WriteLine("Done");
        }
        /// <summary>
        /// This method will be called by the sensor when a door status changes. 
        /// It happens in real time. The main can go about whatever business it wants to do.
        /// </summary>
        /// <param name="vanDoorList"></param>
        private static void ProcessSensorOutput(List<VanDoor> vanDoors) {
            Console.WriteLine("Door status changed");
            for (int i = 0; i < vanDoors.Count; i++) {
                Console.WriteLine("  " + vanDoors[i].doorType.ToString() + ": " + vanDoors[i].doorStatus.ToString());
            }
        }
    }
}
