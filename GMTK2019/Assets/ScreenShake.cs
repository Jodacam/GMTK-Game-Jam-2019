using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    float strength;
    Coroutine shakeCoroutine;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    public void StartShake(float duration, float strength)
    {

        if (strength > this.strength)
        {
            if(shakeCoroutine != null){
                StopCoroutine(shakeCoroutine);
            }
            this.strength = strength;
            shakeCoroutine = StartCoroutine(Shake(duration, strength));
        }
    }

    IEnumerator Shake(float duration, float strength)
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;

            transform.localPosition = initialPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.localPosition = initialPosition;
    }
}
