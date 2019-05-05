using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : MonoBehaviour {

    [SerializeField] float decayTime;

    private void Update()
    {
        if (decayTime > 0)
            decayTime -= Time.deltaTime;

        if (decayTime <= 0)
            Destroy(gameObject);
    }
}
