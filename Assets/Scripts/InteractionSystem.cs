using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Lean;
using Lean.Transition.Extras;
using NaughtyAttributes;
using TextAdventure;

public class InteractionSystem : MonoBehaviour
{
    [Header("Selection Settings")]
    [SerializeField] private Camera cam = null;
    [SerializeField] private float selectionDistance = 1.7f;

    [Header("Highlight Settings")]
    [SerializeField] private bool useHighlight = true;
    [SerializeField] private Material highlightMaterial = null;
    [SerializeField] private Material defaultMaterial = null;
    
    [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactionButton = KeyCode.E;
    [Label("Cancel Interaction")]
    [SerializeField] private KeyCode cancelInteractionButton = KeyCode.Escape;
    
    
    [Header("Camera Animation")]
    [Label("Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera vcam = null;
    [SerializeField] private LeanAnimation resizeIn;
    [SerializeField] private LeanAnimation resizeOut;
    
    [Header("// Selection Info //")]
    [ReadOnly]
    [Label("Last Selection")]
    [SerializeField] private GameObject selection = null;
    [ReadOnly]
    [SerializeField] private Renderer currentRenderer = null;
    [ReadOnly]
    [SerializeField] private bool isSelected = false;
    [ReadOnly]
    public bool isInteracting = false;
    
    private string selectableTag = "Selectable"; // what can be selected
    private PlayerController player = null;
    public TextInput textInput;

    private void Awake()
    {
        textInput = gameObject.GetComponent<TextInput>();
    }

    private void Start(){ player = FindObjectOfType<PlayerController>(); }

    private void Update(){ Select(); }
    
    private void Select()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, selectionDistance))
        { 
            if (hit.transform.CompareTag(selectableTag)) 
            { 
                selection = hit.collider.gameObject;
             
                if (defaultMaterial == null) 
                    defaultMaterial = selection.GetComponent<Renderer>().material;
             
                currentRenderer = selection.GetComponent<Renderer>();
                if(useHighlight)
                    currentRenderer.material = highlightMaterial;
                    
                isSelected = true;
                Interact();
            }
        }else if (currentRenderer != null) 
            Deselect();
    }
    
    private void Deselect()
    { 
        currentRenderer.material = defaultMaterial;
        isSelected = false;
        currentRenderer = null;
        defaultMaterial = null;
    }

    private void Interact()
    {
        if (Input.GetKeyDown(interactionButton))
        {
            player.canMove = false;
            isInteracting = true;
            vcam.Priority = 8;
            resizeIn.Transitions.Begin();
            textInput.ActivateInput();
        }
        
        DeInteract();
    }

    private void DeInteract()
    {
        if (Input.GetKeyDown(cancelInteractionButton))
        {
            isInteracting = false;
            resizeOut.Transitions.Begin();
            vcam.Priority= 10;
            player.canMove = true;
            textInput.DeactivateInput();
        }
    }
}
