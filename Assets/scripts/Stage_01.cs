using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Stage_01 : MonoBehaviour
{
    public Transform center;              // 円の中心
    public float radius = 1f;             // 円の半径
    public float snapThreshold = 10f;     // スナップ角度の閾値（度）
    public float[] snapAngles = { 90f, 270f }; // スナップする角度（度）
    [SerializeField] GameObject symmetryLine;

    private bool isDragging = false;

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.WorldToScreenPoint(center.position).z);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3 direction = (worldMousePos - center.position).normalized;
            float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 最も近いスナップ角度を計算
            float closestSnap = snapAngles
                .OrderBy(a => Mathf.Abs(Mathf.DeltaAngle(currentAngle, a)))
                .First();

            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, closestSnap));

            // 吸着する角度を決定
            float finalAngle = currentAngle;
            if (angleDifference < snapThreshold)
            {
                finalAngle = closestSnap;
            }

            // 角度 → ベクトル変換
            float finalRad = finalAngle * Mathf.Deg2Rad;
            Vector3 snappedDir = new Vector3(Mathf.Cos(finalRad), Mathf.Sin(finalRad), 0f);

            // 位置・角度を更新
            transform.position = center.position + snappedDir * radius;
            transform.rotation = Quaternion.Euler(0f, 0f, finalAngle);  // ←ここが追加ポイント
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            float angle = transform.eulerAngles.z;
            if (Mathf.Abs(angle - 90f) < 10f || Mathf.Abs(angle - 270f) < 10f)
            {
                Debug.Log("クリア判定！");
                // ここにクリア処理を書く
                ShowSymmetryLine();
            }
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void ShowSymmetryLine()
    {
        symmetryLine.SetActive(true);
        StartCoroutine(WaitFiveSeconds());
    }

    IEnumerator WaitFiveSeconds()
    {
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("Stage_02");
    }
}