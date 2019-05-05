using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEmmission : MonoBehaviour {

	ParticleSystem system;

    int numParticles = 0;

    private void Start()
    {
        system = GetComponent<ParticleSystem>();

        var emitter = system.emission;
        emitter.rateOverTime = numParticles;
        numParticles = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up");
            numParticles++;

            var emitter = system.emission;
            emitter.rateOverTime = numParticles;            
        }
    }

}
