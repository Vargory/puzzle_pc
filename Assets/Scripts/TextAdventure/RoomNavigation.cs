using System.Collections.Generic;
using UnityEngine;

namespace TextAdventure
{
    public class RoomNavigation : MonoBehaviour
    {
        public Room currentRoom;
        
        Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();

        private GameController controller;

        private void Awake()
        {
            controller = gameObject.GetComponent<GameController>();
        }

        public void UnpackExitsInRoom()
        {
            
            for (int i = 0; i < currentRoom.exits.Length; i++)
            {
                exitDictionary.Add(currentRoom.exits[i].keyString, currentRoom.exits[i].valueRoom);
                controller.interactionDescriptionsInRoom.Add(currentRoom.exits[i].exitDescription);
            }
        }

        public void attemptToChangeRooms(string directionNoun)
        {
            if (exitDictionary.ContainsKey(directionNoun))
            {
                currentRoom = exitDictionary[directionNoun];
                controller.LogStringWithReturn("Você vai em direção ao " + directionNoun);
                controller.DisplayRoomText();
            }
            else
            {
                controller.LogStringWithReturn("Não há nada ao " + directionNoun);
            }
        }

        public void ClearExits()
        {
            exitDictionary.Clear();
        }
    }
}
