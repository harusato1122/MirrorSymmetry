using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Stage_01 : MonoBehaviour
{
    public Transform center;              // �~�̒��S
    public float radius = 1f;             // �~�̔��a
    public float snapThreshold = 10f;     // �X�i�b�v�p�x��臒l�i�x�j
    public float[] snapAngles = { 90f, 270f }; // �X�i�b�v����p�x�i�x�j
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

            // �ł��߂��X�i�b�v�p�x���v�Z
            float closestSnap = snapAngles
                .OrderBy(a => Mathf.Abs(Mathf.DeltaAngle(currentAngle, a)))
                .First();

            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, closestSnap));

            // �z������p�x������
            float finalAngle = currentAngle;
            if (angleDifference < snapThreshold)
            {
                finalAngle = closestSnap;
            }

            // �p�x �� �x�N�g���ϊ�
            float finalRad = finalAngle * Mathf.Deg2Rad;
            Vector3 snappedDir = new Vector3(Mathf.Cos(finalRad), Mathf.Sin(finalRad), 0f);

            // �ʒu�E�p�x���X�V
            transform.position = center.position + snappedDir * radius;
            transform.rotation = Quaternion.Euler(0f, 0f, finalAngle);  // ���������ǉ��|�C���g
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            float angle = transform.eulerAngles.z;
            if (Mathf.Abs(angle - 90f) < 10f || Mathf.Abs(angle - 270f) < 10f)
            {
                Debug.Log("�N���A����I");
                // �����ɃN���A����������
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