using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage_06 : MonoBehaviour
{
    public GameObject object1; // 上に動く
    public GameObject object2; // 下に動く
    [SerializeField] GameObject symmetryLine;

    private Camera mainCamera;
    private bool isDragging = false;
    private GameObject activeTarget = null;
    private Vector3 lastMousePos;

    private float snapThreshold = 0.1f; // Y=0に近づいたらスナップ

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // マウス押下時の処理
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == object1 || hit.collider.gameObject == object2)
                {
                    isDragging = true;
                    activeTarget = hit.collider.gameObject;
                }
            }
        }

        // マウス離したときの処理
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            activeTarget = null;

            TrySnapBothToZero();

            if (IsAtZero(object1) && IsAtZero(object2))
            {
                Debug.Log("🎉 クリア！！両方 Y=0 にスナップされました！");
                // 必要に応じて他の処理を追加（例：シーン遷移など）
                GameClear();
            }
        }

        // ドラッグ中の処理
        if (isDragging && activeTarget != null)
        {
            Vector3 currentMousePos = Input.mousePosition;
            float deltaY = currentMousePos.y - lastMousePos.y;
            float moveAmount = deltaY * 0.01f;

            // ドラッグによる移動処理（両方同時）
            object1.transform.position += new Vector3(0f, moveAmount, 0f);
            object2.transform.position -= new Vector3(0f, moveAmount, 0f);

            lastMousePos = currentMousePos;
        }
    }

    // Y=0付近ならピタッとスナップ（両方同時）
    void TrySnapBothToZero()
    {
        if (IsNearZero(object1) && IsNearZero(object2))
        {
            Vector3 pos1 = object1.transform.position;
            Vector3 pos2 = object2.transform.position;

            object1.transform.position = new Vector3(pos1.x, 0f, pos1.z);
            object2.transform.position = new Vector3(pos2.x, 0f, pos2.z);

            Debug.Log("🔒 両方ピタッと Y=0 にスナップ！");
        }
    }

    // Yが0付近か判定
    bool IsNearZero(GameObject obj)
    {
        return Mathf.Abs(obj.transform.position.y) < snapThreshold;
    }

    // Yが完全に0か判定（スナップ後の判定用）
    bool IsAtZero(GameObject obj)
    {
        return Mathf.Approximately(obj.transform.position.y, 0f);
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
        SceneManager.LoadScene("Stage_07");
    }
}
