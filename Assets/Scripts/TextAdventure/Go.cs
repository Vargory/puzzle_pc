using System.Collections;
using System.Collections.Generic;
using TextAdventure;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/InputActions/Go")]
public class Go : InputAction
{
    public override void RespondInput(GameController controller, string[] separatedInputWords)
    {
        controller.roomNavigation.attemptToChangeRooms(separatedInputWords[1]);
    }
}
