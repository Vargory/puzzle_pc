using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace TextAdventure
{
    public class TextInput : MonoBehaviour
    {
        [ReadOnly][SerializeField]
        private GameController controller;
        public TMP_InputField inputField;
        public InteractionSystem interactionSystem;

        private void Awake()
        {
            interactionSystem = gameObject.GetComponent<InteractionSystem>();
            controller = gameObject.GetComponent<GameController>();
            inputField.onEndEdit.AddListener(AcceptStringInput);
        }

        private void AcceptStringInput(string userInput)
        {
            if(!interactionSystem.isInteracting)
                inputField.onEndEdit.RemoveListener(AcceptStringInput);
            
            userInput = userInput.ToLower();
            controller.LogStringWithReturn(userInput);
            char[] delimiterCharacters = {' '};
            string[] separatedInputWords = userInput.Split(delimiterCharacters);

            for (int i = 0; i < controller.inputActions.Length; i++)
            {
                InputAction inputAction = controller.inputActions[i];
                if (inputAction.keyWord == separatedInputWords[0])
                {
                    inputAction.RespondInput(controller, separatedInputWords);
                }
            }
            InputComplete();
            }
        private void InputComplete()
        {
            controller.DisplayLoggedText();
            
                inputField.ActivateInputField();

                inputField.text = null;
        }

        public void ActivateInput()
        {
            inputField.interactable = true;
            inputField.ActivateInputField();
        }

        public void DeactivateInput()
        {
            inputField.interactable = false;
        }
    }
}
