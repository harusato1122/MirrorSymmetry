using UnityEngine;

public class Stage_02 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private bool isDragging = false;
    private Vector3 initialMousePos;
    private Vector3 initialPos;
    private float baseSpriteWidth;
    private float initialSize;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (spriteRenderer == null || col == null)
        {
            Debug.LogError("SpriteRendererかCollider2Dがアタッチされていません");
            enabled = false;
            return;
        }

        baseSpriteWidth = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (col == Physics2D.OverlapPoint(mouseWorldPos))
            {
                isDragging = true;
                initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                initialPos = transform.position;
                initialSize = transform.localScale.x;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isDragging = false;

                float currentSize = transform.localScale.x;
                if (Mathf.Abs(currentSize - 2f) < 0.2f)
                {
                    Debug.Log("クリア！");
                    // TODO: ここにクリア演出やシーン遷移など追加
                    GameClear();
                }
            }
        }

        if (isDragging)
        {
            Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float deltaX = currentMousePos.x - initialMousePos.x;

            float sizeChange = -deltaX * 2f;
            float newSize = Mathf.Clamp(initialSize + sizeChange, 1f, 5f);

            if (Mathf.Abs(newSize - 2f) < 0.2f)
            {
                newSize = 2f;
            }

            float newScaleX = newSize;
            float newWidth = baseSpriteWidth * newScaleX;
            float initialWidth = baseSpriteWidth * initialSize;
            float offsetX = (newWidth - initialWidth) * 0.5f;

            transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
            transform.position = initialPos - new Vector3(offsetX, 0f, 0f);
        }
    }

    void GameClear()
    {

    }
}