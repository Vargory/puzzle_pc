using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TextAdventure
{
    public class GameController : MonoBehaviour
    {
        public TMP_Text displayText;
        public InputAction[] inputActions;
    
        [HideInInspector] public RoomNavigation roomNavigation;
        [HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string>();
        List<string> actionLog = new List<string>();

        void Awake()
        {
            roomNavigation = gameObject.GetComponent<RoomNavigation>();
        }

        void Start()
        {
            DisplayRoomText();
            DisplayLoggedText();
        }

        public void DisplayLoggedText()
        {
            string logAsText = string.Join("\n", actionLog.ToArray());
            displayText.text = logAsText;
        }
    
        public void DisplayRoomText()
        {
            clearCollectionsForNewRoom();
            UnpackRoom();

            string joinedInteractionDescriptions = string.Join("\n", interactionDescriptionsInRoom.ToArray());
            string combinedText = roomNavigation.currentRoom.description + "\n" + joinedInteractionDescriptions;
            LogStringWithReturn(combinedText);
        }

        private void UnpackRoom()
        {
            roomNavigation.UnpackExitsInRoom();
        }

        private void clearCollectionsForNewRoom()
        {
            interactionDescriptionsInRoom.Clear();
            roomNavigation.ClearExits();
        }

        public void LogStringWithReturn(string stringToAdd)
        {
            actionLog.Add(stringToAdd + "\n");
        }
    
        void Update()
        {
        
        }
    }
}
