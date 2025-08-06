using UnityEngine;
using TMPro;

public class Stage_05 : MonoBehaviour
{
    public GameObject[] targetObjects;

    private Camera mainCamera;
    private GameObject selectedObject = null;
    private Vector3 lastMousePos;

    private float snapTolerance = 5f; // �}5�x�ŃX�i�b�v

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;

            GameObject clicked = GetClickedObject();
            if (clicked != null && IsTarget(clicked))
            {
                selectedObject = clicked;

                // �N���b�N�ŕ����؂�ւ�
                ToggleTextCase(clicked);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            SnapRotationIfNeeded();
            selectedObject = null;
        }

        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float rotationSpeed = 0.3f;

            selectedObject.transform.Rotate(0f, 0f, -delta.x * rotationSpeed);

            lastMousePos = Input.mousePosition;
        }
    }

    void SnapRotationIfNeeded()
    {
        if (selectedObject == null) return;

        float zAngle = NormalizeAngle(selectedObject.transform.eulerAngles.z);

        float[] snapAngles = { 0f, 90f, 180f, 270f };
        foreach (float targetAngle in snapAngles)
        {
            if (Mathf.Abs(zAngle - targetAngle) < snapTolerance)
            {
                selectedObject.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
                Debug.Log($"�s�^�b�� {targetAngle} �x�ɃX�i�b�v�I");
                break;
            }
        }
    }

    GameObject GetClickedObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        RaycastHit hit3D;

        if (hit2D.collider != null)
        {
            return hit2D.collider.gameObject;
        }
        else if (Physics.Raycast(ray, out hit3D, 100f))
        {
            return hit3D.collider.gameObject;
        }

        return null;
    }

    bool IsTarget(GameObject obj)
    {
        foreach (GameObject target in targetObjects)
        {
            if (obj == target) return true;
        }
        return false;
    }

    void ToggleTextCase(GameObject obj)
    {
        TMP_Text tmp = obj.GetComponentInChildren<TMP_Text>();
        if (tmp == null) return;

        string original = tmp.text;
        string toggled = "";

        foreach (char c in original)
        {
            if (char.IsUpper(c))
                toggled += char.ToLower(c);
            else if (char.IsLower(c))
                toggled += char.ToUpper(c);
            else
                toggled += c;
        }

        tmp.text = toggled;
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f) angle += 360f;
        return angle;
    }
}