/*
 * Bill Nicholson
 * nicholdw@ucmail.uc.edu
 * Door Sensor Array for my Toyota Van
 * 5 doors. Each door is open/closed
 * 
 * Refer to https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/ for thread-safe data structures
 * Refer to https://stackoverflow.com/questions/7511199/system-servicemodel-dll-missing-in-references-visual-studio-2010 for the .dll
 *   for  System.Collections.Generic.SynchronizedCollection<VanDoor<>
 */
using System;
using System.Collections.Generic;

namespace VanDoorNamespace {
    /// <summary>
    /// Model the state and type of door.
    /// All doors can be 'open' or 'closed'
    /// The side doors and the rear door can also be 'opening' or 'closing'
    /// </summary>
    public class VanDoor {
        /// <summary>
        /// All the different doors in the van
        /// </summary>
        public enum DoorType {DriverSide, PassengerSide, DriverSideSlider, PassengerSideSlider, RearHatch};
        /// <summary>
        /// All the possible status codes for the doors. Not all codes are valid for all door types!
        /// </summary>
        public enum DoorStatus {Unknown, Closed, Open, Opening, Closing};
        private DoorType mDoorType;
        private DoorStatus mDoorStatus;
        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="doorType">Driver Side, Passenger Side, etc. </param>
        /// <param name="doorStatus">Open, Closed, etc.</param>
        public VanDoor(DoorType doorType, DoorStatus doorStatus) {
            this.mDoorType = doorType;
            this.mDoorStatus = doorStatus;
        }
        public DoorType doorType {
            get { return mDoorType; }
            set { value = mDoorType; }
        }
        public DoorStatus doorStatus {
            get { return mDoorStatus; }
            set { mDoorStatus = value; }
        }
        /// <summary>
        /// Build a list of the 5 van doors and initialize the status of each to unknown.
        /// </summary>
        /// <returns>The list of doors</returns>
        public static System.Collections.Generic.SynchronizedCollection<VanDoor> buildDoorList() {
//          List<VanDoor> vanDoors = new List<VanDoor>();
            SynchronizedCollection<VanDoor> vanDoors = new SynchronizedCollection<VanDoor>();
            vanDoors.Add(new VanDoor(DoorType.DriverSide, DoorStatus.Unknown));
            vanDoors.Add(new VanDoor(DoorType.PassengerSide, DoorStatus.Unknown));
            vanDoors.Add(new VanDoor(DoorType.DriverSideSlider, DoorStatus.Unknown));
            vanDoors.Add(new VanDoor(DoorType.PassengerSideSlider, DoorStatus.Unknown));
            vanDoors.Add(new VanDoor(DoorType.RearHatch, DoorStatus.Unknown));
            return vanDoors;
        }
    }
}
