using UnityEngine;

namespace TextAdventure
{
 [CreateAssetMenu(menuName = "TextAdventure/Room")]
 public class Room : ScriptableObject
 {
 
  [TextArea]
  public string description;
  public string roomName;
  public Exit[] exits;

 }
}
