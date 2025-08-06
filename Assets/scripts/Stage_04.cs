using UnityEngine;

public class Stage_04 : MonoBehaviour
{
    public Vector3 snapTargetPosition = new Vector3(2f, 1f, 0f); // �s�^�b�Ƃ������ڕW���W
    public float snapThreshold = 0.5f; // �z���̋��e����

    private Vector3 offset;
    private bool isDragging = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOver())
            {
                isDragging = true;
                offset = transform.position - mouseWorld;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // �w��ʒu�ɋ߂���΃X�i�b�v
            if (Vector3.Distance(transform.position, snapTargetPosition) < snapThreshold)
            {
                transform.position = snapTargetPosition;
                Debug.Log("�s�^�b�Ƌz���I");
            }
        }

        if (isDragging)
        {
            transform.position = mouseWorld + offset;
        }
    }

    // �}�E�X���I�u�W�F�N�g��ɂ��邩����
    bool IsMouseOver()
    {
        Collider2D col2D = GetComponent<Collider2D>();
        Collider col3D = GetComponent<Collider>();

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
}