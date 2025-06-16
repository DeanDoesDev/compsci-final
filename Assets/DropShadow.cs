using UnityEngine;

public class DropShadow : MonoBehaviour
{
    [Header("Shadow Settings")]
    public Color shadowColor = new Color(0f, 0f, 0f, 0.5f); 
    public Vector2 shadowOffset = new Vector2(0.2f, 0.2f);
    public int shadowLayer = -10; 
    public GameObject shadowPrefab; 

    private GameObject shadowInstance;

    void Start()
    {
        shadowInstance = Instantiate(shadowPrefab, transform.position + (Vector3)shadowOffset, Quaternion.identity);

        shadowInstance.transform.SetParent(transform);

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            SpriteRenderer shadowRenderer = shadowInstance.GetComponent<SpriteRenderer>();
            if (shadowRenderer != null)
            {
                shadowRenderer.sprite = spriteRenderer.sprite; 
                shadowRenderer.color = shadowColor; 
                shadowRenderer.sortingOrder = shadowLayer; 
            }
        }

        shadowInstance.transform.localScale = transform.localScale;
    }

    void Update()
    {
        if (shadowInstance != null)
        {
            shadowInstance.transform.position = transform.position + (Vector3)shadowOffset;
            shadowInstance.transform.rotation = transform.rotation;
        }
    }

    void OnDestroy()
    {
        if (shadowInstance != null)
        {
            Destroy(shadowInstance);
        }
    }
}
