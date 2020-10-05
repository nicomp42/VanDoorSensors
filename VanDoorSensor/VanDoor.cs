/*
 * Bill Nicholson
 * nicholdw@ucmail.uc.edu
 * Door Sensor Array for my Toyota Van
 * 5 doors. Each door is open/closed
 * The side doors and the read door can also be opening or closing
 */
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanDoorNamespace {
    public class VanDoor {
        public enum DoorType {DriverSide, PassengerSide, DriverSideSlider, PassengerSideSlider, RearHatch};
        public enum DoorStatus {Unknown, Closed, Open, Opening, Closing};
        private DoorType doorType;
        private DoorStatus doorStatus;
        public VanDoor(DoorType doorType, DoorStatus doorStatus) {
            this.doorType = doorType;
            this.doorStatus = doorStatus;
        }
        /// <summary>
        /// Build a list of the 5 van doors and initialize the status of each to unknown.
        /// </summary>
        /// <returns>The list of doors</returns>
        public static List<VanDoor> buildDoorList() {
            List<VanDoor> vanDoors = new List<VanDoor>();
            vanDoors.Add(new VanDoor(DoorType.DriverSide, DoorStatus.Unknown));
            vanDoors.Add(new VanDoor(DoorType.PassengerSide, DoorStatus.Unknown));
            vanDoors.Add(new VanDoor(DoorType.DriverSideSlider, DoorStatus.Unknown));
            vanDoors.Add(new VanDoor(DoorType.PassengerSideSlider, DoorStatus.Unknown));
            vanDoors.Add(new VanDoor(DoorType.RearHatch, DoorStatus.Unknown));
            return vanDoors;
        }
    }
}
