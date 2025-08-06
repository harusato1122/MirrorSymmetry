using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnitation : MonoBehaviour
{
    public float stretchTime = 150.0f;      // 各軸の拡大時間
    public float targetYScale = 22.0f;     // 最終的なYのスケール
    public float targetXScale = 22.0f;     // 最終的なXのスケール

    void Start()
    {
        StartCoroutine(StretchInOrder());
    }

    IEnumerator StretchInOrder()
    {
        // Y方向に伸ばす
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;
        while (elapsed < stretchTime)
        {
            float t = elapsed / stretchTime;
            float newY = Mathf.Lerp(originalScale.y, targetYScale, t);
            transform.localScale = new Vector3(originalScale.x, newY, originalScale.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(originalScale.x, targetYScale, originalScale.z);

        // 少し待機（オプション）
        yield return new WaitForSeconds(0.1f);

        // X方向に伸ばす
        elapsed = 0f;
        originalScale = transform.localScale;
        while (elapsed < stretchTime)
        {
            float t = elapsed / stretchTime;
            float newX = Mathf.Lerp(originalScale.x, targetXScale, t);
            transform.localScale = new Vector3(newX, originalScale.y, originalScale.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(targetXScale, originalScale.y, originalScale.z);
    }
}
