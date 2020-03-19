using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);

    [Range(0,1)][SerializeField] float movementFactor;

    [SerializeField] float period = 2f;

    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   // set movement factor auto
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period; // grows continually form 0 

        const float tau = Mathf.PI * 2; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = Mathf.Abs(rawSinWave) ;

        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
