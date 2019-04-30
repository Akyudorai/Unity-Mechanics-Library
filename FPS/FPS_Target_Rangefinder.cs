using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Target_Rangefinder : MonoBehaviour {

	private float distance = 0.5f;
    
    // between 1 and 0
    public void SetDistance(float position)
    {
        distance = position - 0.5f;
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(distance, transform.localPosition.y, transform.localPosition.z), Time.deltaTime);
    }
}
