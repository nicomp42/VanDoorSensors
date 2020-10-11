/*
 * Bill Nicholson
 * nicholdw@ucmail.uc.edu
 * Door Sensor Array for my Toyota Van
 * 5 doors. Each door is open/closed
 * The side doors and the read door can also be opening or closing.
 * 
 * We are using SynchronizedCollection instead of the ubiquitous List data structure. 
 * Refer to https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/ for thread-safe data structures.
 * See also https://stackoverflow.com/questions/7511199/system-servicemodel-dll-missing-in-references-visual-studio-2010 for
 *   adding the proper reference to the project.
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
        private Action<SynchronizedCollection<VanDoor>> CallMe;
        private TimeSpan timeSpan;
        private SynchronizedCollection<VanDoor> vanDoors;
        /// <summary>
        /// Initialize the door sensor array and start it running
        /// </summary>
        /// <param name="sensorName">The friendly name of the sensor</param>
        /// <param name="CallMe"> The method to call when a door status changes. 
        /// The method will be passed the list of all doors 
        /// or leaving (negative number) the sensor in real time.</param>
        /// <params name="seconds"> How long the sensor should run, in seconds. 
        ///  Use 0 for Test Mode: All doors set to Unknown for 5 seconds, then 30-second duration, All door toggle between open and closed every 5 seconds for 30 seconds.</params>
        /// <returns></returns>
        public Thread StartSensor(String sensorName, Action<SynchronizedCollection<VanDoor>> CallMe, int seconds) {
            this.CallMe = CallMe;
            this.timeSpan = new TimeSpan(0, 0, seconds);
            this.sensorName = sensorName;
            vanDoors = VanDoor.buildDoorList();     // Create a set of van doors.
            thread = new Thread(this.ThreadStartCallMe);
            thread.Start();
            return thread;
        }
        /// <summary>
        /// What happens in the thread.
        /// Demo mode = 10 minutes duration, drivers side door cycles between open for 10 seconds, closed for 10 seconds.
        /// </summary>
        private void ThreadStartCallMe() {
            DateTime start = new DateTime();
            start = DateTime.Now;
            Random random = new Random();
            if (timeSpan != new TimeSpan(0)) {
                while (true) {
                    Thread.Sleep(250 * random.Next(1, 5));
                    // Get a random door
                    int randomDoor = random.Next(0, vanDoors.Count - 1);
                    VanDoor.DoorStatus randomDoorStatus;
                    Array values = Enum.GetValues(typeof(VanDoor.DoorStatus));
                    while (true) {  // Get a valid random door status for the random door.
                        randomDoorStatus = (VanDoor.DoorStatus)values.GetValue(random.Next(values.Length));
                        if ((vanDoors[randomDoor].doorStatus == VanDoor.DoorStatus.Closing ||
                             vanDoors[randomDoor].doorStatus == VanDoor.DoorStatus.Opening)) {
                            if (vanDoors[randomDoor].doorType == VanDoor.DoorType.DriverSideSlider ||
                                vanDoors[randomDoor].doorType == VanDoor.DoorType.PassengerSideSlider) {

                                break;
                            }
                        } else { break; }
                    }
                    vanDoors[randomDoor].doorStatus = randomDoorStatus;
                    CallMe(vanDoors);
                    TimeSpan elapsed;
                    elapsed = DateTime.Now - start;
                    if (elapsed >= timeSpan) { break; }
                }
            } else {
                // The Demo Mode. 
                VanDoor.DoorStatus myDoorStatus = VanDoor.DoorStatus.Open;
                for (int i = 0; i < vanDoors.Count; i++) { vanDoors[i].doorStatus = VanDoor.DoorStatus.Unknown; }
                CallMe(vanDoors);
                Thread.Sleep(5000);
                timeSpan = new TimeSpan(0, 10, 0);    // Default to 10 minutes
                while (true) {
                    Thread.Sleep(10000);             // Default to 10 second pause
                    for (int i = 0; i < vanDoors.Count; i++) {
                        vanDoors[i].doorStatus = myDoorStatus;
                        // Toggle the status netween open and closed
                        if (myDoorStatus == VanDoor.DoorStatus.Open) {
                            myDoorStatus = VanDoor.DoorStatus.Closed;
                        } else {
                            myDoorStatus = VanDoor.DoorStatus.Open;
                        }
                    }
                    CallMe(vanDoors);               // Default to something
                    TimeSpan elapsed;
                    elapsed = DateTime.Now - start;
                    //                  Console.WriteLine(elapsed);
                    if (elapsed >= timeSpan) { break; }
                }
            }
        }
    }
}
