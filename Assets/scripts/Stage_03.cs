using UnityEngine;

public class Stage_03 : MonoBehaviour
{
    public GameObject object1; // Z軸回転
    public GameObject object2; // Y軸回転

    private Camera mainCamera;
    private GameObject selectedObject = null;
    private Vector3 lastMousePos;

    private bool object1Snapped = false;
    private bool object2Snapped = false;
    private bool isClear = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isClear) return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverObject(object1))
            {
                selectedObject = object1;
            }
            else if (IsMouseOverObject(object2))
            {
                selectedObject = object2;
            }

            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedObject = null;
        }

        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float rotationSpeed = 0.3f;

            if (selectedObject == object1)
            {
                selectedObject.transform.Rotate(0f, 0f, -delta.x * rotationSpeed);
                float zAngle = NormalizeAngle(selectedObject.transform.eulerAngles.z);

                if (Mathf.Abs(zAngle - 90f) < 5f)
                {
                    selectedObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    object1Snapped = true;
                }
                else if (Mathf.Abs(zAngle - 270f) < 5f)
                {
                    selectedObject.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                    object1Snapped = true;
                }
                else
                {
                    object1Snapped = false;
                }
            }
            else if (selectedObject == object2)
            {
                selectedObject.transform.Rotate(0f, delta.x * rotationSpeed, 0f);
                float yAngle = NormalizeAngle(selectedObject.transform.eulerAngles.y);

                if (Mathf.Abs(yAngle - 180f) < 5f)
                {
                    selectedObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    object2Snapped = true;
                }
                else
                {
                    object2Snapped = false;
                }
            }

            lastMousePos = Input.mousePosition;
        }

        // ✅ 両方はまってたらクリア
        if (object1Snapped && object2Snapped && !isClear)
        {
            isClear = true;
            Debug.Log("🎉 Clear!");
            // TODO: エフェクト・演出・シーン遷移などここに追加
            GameClear();
        }
    }

    bool IsMouseOverObject(GameObject obj)
    {
        Collider2D col2D = obj.GetComponent<Collider2D>();
        Collider col3D = obj.GetComponent<Collider>();

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouse2D = new Vector2(mouseWorld.x, mouseWorld.y);

        if (col2D != null)
        {
            return col2D.OverlapPoint(mouse2D);
        }
        else if (col3D != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            return col3D.Raycast(ray, out RaycastHit hit, 100f);
        }

        return false;
    }

    float NormalizeAngle(float angle)
    {
        return (angle + 360f) % 360f;
    }

    void GameClear()
    {

    }
}