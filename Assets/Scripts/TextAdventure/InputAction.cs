using System.Collections;
using System.Collections.Generic;
using TextAdventure;
using UnityEngine;
using UnityEngine.UI;

public abstract class InputAction : ScriptableObject
{
    public string keyWord;

    public abstract void RespondInput(GameController controller, string[] separatedInputWords);    
}
