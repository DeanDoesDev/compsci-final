using UnityEngine;

public class GhostFade : MonoBehaviour
{
    public float fadeTime = 0.3f;
    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        float timer = fadeTime;
        while (timer > 0)
        {
            float alpha = timer / fadeTime;
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            timer -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
