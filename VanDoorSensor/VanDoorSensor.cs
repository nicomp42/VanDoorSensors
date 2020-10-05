/*
 * Bill Nicholson
 * nicholdw@ucmail.uc.edu
 * Door Sensor Array for my Toyota Van
 * 5 doors. Each door is open/closed
 * The side doors and the read door can also be opening or closing
 */
using System;
using System.Collections.Generic;
using System.Threading;
using VanDoorNamespace;

namespace VanDoorSensorNamespace
{
    public class VanDoorSensor
    {
        // private non-static class members. We will not be sharing them between threads
        private String sensorName;
        private Thread thread;
        private Action<List<VanDoor>> CallMe;
        private TimeSpan timeSpan;
        private List<VanDoor> vanDoors;
        /// <summary>
        /// Initialize the door sensor array and start it running
        /// </summary>
        /// <param name="sensorName">The friendly name of the sensor</param>
        /// <param name="CallMe"> The method to call when a door status changes. 
        /// The method will be passed the list of all doors 
        /// or leaving (negative number) the sensor in real time.</param>
        /// <params name="seconds"> How long the sensor should run, in seconds. Use 0 for Test Mode: 30-second duration, 1 vehicle entering per second.</params>
        /// <returns></returns>
        public Thread StartSensor(String sensorName, Action<List<VanDoor>> CallMe, int seconds) {
            this.CallMe = CallMe;
            this.timeSpan = new TimeSpan(0, 0, seconds);
            this.sensorName = sensorName;
            vanDoors = VanDoor.buildDoorList();     // Create a set of van doors.
            thread = new Thread(this.ThreadStartCallMe);
            thread.Start();
            return thread;
        }
        /// <summary>
        /// What happens in the thread
        /// </summary>
        private void ThreadStartCallMe() {
            DateTime start = new DateTime();
            start = DateTime.Now;
            Random random = new Random();
            if (timeSpan != new TimeSpan(0)) {
                while (true) {
                    Thread.Sleep(250 * random.Next(1, 5));
                    // Get a random door

                    CallMe(vanDoors);
                    TimeSpan elapsed;
                    elapsed = DateTime.Now - start;
                    if (elapsed >= timeSpan) { break; }
                }
            } else {
                // The Demo Mode. 
                timeSpan = new TimeSpan(0, 0, 30);    // Default to 30 seconds
                while (true) {
                    Thread.Sleep(1000);             // Default to 1 second pause
                    CallMe(vanDoors);                      // Default to one vehicle entering the sensor
                    TimeSpan elapsed;
                    elapsed = DateTime.Now - start;
                    //                  Console.WriteLine(elapsed);
                    if (elapsed >= timeSpan) { break; }
                }
            }
        }
    }
}
