using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Stage_07 : MonoBehaviour
{
    public GameObject stretchXObject; // X方向に伸ばす
    public GameObject stretchYObject; // Y方向に伸ばす
    [SerializeField] GameObject symmetryLine;

    private Vector3 lastMousePos;
    private bool isDragging = false;
    private float snapThreshold = 5f;

    // 初期状態記録用
    private float initialZRotation = 290f;

    private Vector3 stretchXStartScale = new Vector3(2f, 0.8f, 1f);
    private Vector3 stretchXEndScale = new Vector3(3.5f, 0.8f, 1f);
    private Vector3 stretchXStartPos = new Vector3(-1f, 0.5f, 0f);
    private Vector3 stretchXEndPos = new Vector3(0f, 0.5f, 0f);

    private Vector3 stretchYStartScale = new Vector3(0.8f, 3f, 1f);
    private Vector3 stretchYEndScale = new Vector3(0.8f, 4f, 1f);
    private Vector3 stretchYStartPos = new Vector3(-1.8f, -0.5f, 0f);
    private Vector3 stretchYEndPos = new Vector3(-1.8f, 0f, 0f);
    private bool isSnapped = false;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 180f, initialZRotation);
        if (stretchXObject != null)
        {
            stretchXObject.transform.localScale = stretchXStartScale;
            stretchXObject.transform.localPosition = stretchXStartPos;
        }
        if (stretchYObject != null)
        {
            stretchYObject.transform.localScale = stretchYStartScale;
            stretchYObject.transform.localPosition = stretchYStartPos;
        }
    }

    void Update()
    {
        // マウス押下
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                isDragging = true;
                lastMousePos = Input.mousePosition;
            }
        }

        // マウス離す
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            if (NormalizeAngle(transform.eulerAngles.z) == 0) GameClear();
        }

        // ドラッグ中の回転
        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float rotationSpeed = 0.3f;

            // 時計回りのみ許可（delta.x > 0）
            if (delta.x > 0f)
            {
                transform.Rotate(0f, 0f, -delta.x * rotationSpeed);  // 時計回りはZマイナス方向
                UpdateStretchObjects();  // 補間の更新もここで実行
            }

            lastMousePos = Input.mousePosition;
        }

        // スナップ処理
        float currentZ = NormalizeAngle(transform.eulerAngles.z);
        float z = NormalizeAngle(transform.eulerAngles.z);
        if (!isSnapped && Mathf.Abs(z - 360f) <= 5f)
        {
            Vector3 currentEuler = transform.eulerAngles;
            transform.rotation = Quaternion.Euler(currentEuler.x, currentEuler.y, 0f); // Z=0 にスナップ
            ApplyFinalTransforms();  // ピタッと見た目補正
            isSnapped = true;        // 一度だけスナップ
        }
        else isSnapped = false;
    }

    // ストレッチオブジェクトを補間して更新
    void UpdateStretchObjects()
    {
        // Z角度を取得して、290°〜0°を t=0〜1 にマッピング
        float currentZ = NormalizeAngle(transform.eulerAngles.z);
        float t = Mathf.InverseLerp(initialZRotation, 0f, currentZ);

        // ★ t を使ってスケーリング
        if (stretchXObject != null)
        {
            stretchXObject.transform.localScale = Vector3.Lerp(stretchXStartScale, stretchXEndScale, t);
            stretchXObject.transform.localPosition = Vector3.Lerp(stretchXStartPos, stretchXEndPos, t);
        }

        if (stretchYObject != null)
        {
            stretchYObject.transform.localScale = Vector3.Lerp(stretchYStartScale, stretchYEndScale, t);
            stretchYObject.transform.localPosition = Vector3.Lerp(stretchYStartPos, stretchYEndPos, t);
        }

        // アタッチされたオブジェクト自身のスケールも回転に合わせて大きくする
        //float scaleValue = Mathf.Lerp(1f, 1.5f, t);
        //transform.localScale = new Vector3(scaleValue, scaleValue, 1f);
    }

    // スナップしたときに最終的な形にする
    void ApplyFinalTransforms()
    {
        if (stretchXObject != null)
        {
            stretchXObject.transform.localScale = stretchXEndScale;
            stretchXObject.transform.localPosition = stretchXEndPos;
        }

        if (stretchYObject != null)
        {
            stretchYObject.transform.localScale = stretchYEndScale;
            stretchYObject.transform.localPosition = stretchYEndPos;
        }
    }

    // 角度正規化
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        return (angle < 0) ? angle + 360f : angle;
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
        SceneManager.LoadScene("Stage_08");
    }
}