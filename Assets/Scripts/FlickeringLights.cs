using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlickeringLights : MonoBehaviour
{
    public Light light;
    public float timeDelay = 1f;
    public int flickering = 1;
    
    private void Update()
    {
        light = gameObject.GetComponent<Light>();

        StartCoroutine(Flick());
    }

    private IEnumerator Flick()
    {
        switch (flickering)
        {
            case 0:
                light.enabled = true;
                flickering = 1;
                yield return new WaitForSeconds(timeDelay * Time.deltaTime);
                break;

            case 1:
                light.enabled = false;
                yield return new WaitForSeconds(timeDelay * Time.deltaTime);
                flickering = 0;
                break;
            
            default:
                Debug.Log(("Eita"));
                break;
        }
    }
    
}
