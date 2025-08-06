using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    public float shrinkDuration = 1.0f; // ������܂ł̎��ԁi�b�j
    private Vector3 initialScale;
    private float timer = 0f;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / shrinkDuration;
        if (t >= 1f)
        {
            Destroy(gameObject);
            return;
        }

        // �X���[�Y��0�ɏk��
        transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
    }
}
