using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnitation : MonoBehaviour
{
    public float stretchTime = 150.0f;      // �e���̊g�厞��
    public float targetYScale = 22.0f;     // �ŏI�I��Y�̃X�P�[��
    public float targetXScale = 22.0f;     // �ŏI�I��X�̃X�P�[��

    void Start()
    {
        StartCoroutine(StretchInOrder());
    }

    IEnumerator StretchInOrder()
    {
        // Y�����ɐL�΂�
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

        // �����ҋ@�i�I�v�V�����j
        yield return new WaitForSeconds(0.1f);

        // X�����ɐL�΂�
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
