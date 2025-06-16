using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    [Header("Shake Settings")]
    public float shakeDuration = 0f;      
    public float shakeAmount = 0.1f;      
    public float decreaseFactor = 1f;     

    private Vector3 originalPos;          
    private bool isShaking = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            if (!isShaking)
            {
                originalPos = transform.position;
                isShaking = true;
            }

            transform.position = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;

            if (shakeDuration <= 0)
            {
                shakeDuration = 0;
                isShaking = false;
                transform.position = originalPos; 
            }
        }
    }

    public void TriggerShake(float duration, float amount, float decayFactor)
    {
        shakeDuration = duration;
        shakeAmount = amount;
        decreaseFactor = decayFactor;
    }
}
