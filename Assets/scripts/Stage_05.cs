using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class Stage_05 : MonoBehaviour
{
    public GameObject[] targetObjects; // 4つ入れておく
    [SerializeField] GameObject symmetryLine;

    private Camera mainCamera;
    private GameObject selectedObject = null;
    private Vector3 lastMousePos;

    private float snapTolerance = 5f; // ±5度でスナップ

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
                Debug.Log($"ピタッと {targetAngle} 度にスナップ！");
                break;
            }
        }

        CheckGameClear();
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

    void CheckGameClear()
    {
        if (targetObjects.Length < 4) return;

        bool IsUpper(GameObject obj)
        {
            TMP_Text tmp = obj.GetComponentInChildren<TMP_Text>();
            return tmp != null && tmp.text.Length > 0 && char.IsUpper(tmp.text[0]);
        }

        bool IsLower(GameObject obj)
        {
            TMP_Text tmp = obj.GetComponentInChildren<TMP_Text>();
            return tmp != null && tmp.text.Length > 0 && char.IsLower(tmp.text[0]);
        }

        bool IsAngleNear(GameObject obj, float target)
        {
            float z = NormalizeAngle(obj.transform.eulerAngles.z);
            return Mathf.Abs(z - target) < snapTolerance;
        }

        GameObject obj1 = targetObjects[0];
        GameObject obj2 = targetObjects[1];
        GameObject obj3 = targetObjects[2];
        GameObject obj4 = targetObjects[3];

        bool cond1 = IsUpper(obj1);
        bool cond2 = IsUpper(obj2) || (IsLower(obj2) && (IsAngleNear(obj2, 90f) || IsAngleNear(obj2, 270f)));
        bool cond3 = IsUpper(obj3) && (IsAngleNear(obj3, 0f) || IsAngleNear(obj3, 180f));
        bool cond4 = IsUpper(obj4) && (IsAngleNear(obj4, 90f) || IsAngleNear(obj4, 270f));

        if (cond1 && cond2 && cond3 && cond4)
        {
            Debug.Log("🎉 クリア判定成功！");
            GameClear();
        }
    }

    void GameClear()
    {
        ShowSymmetryLine();
    }

    void ShowSymmetryLine()
    {
        symmetryLine.SetActive(true);
        StartCoroutine(WaitFiveSeconds());
    }

    IEnumerator WaitFiveSeconds()
    {
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("Stage_06");
    }
}