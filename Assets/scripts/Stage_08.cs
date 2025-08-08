using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Stage_08 : MonoBehaviour
{
    public GameObject targetObject;  // Inspector ‚ÅŽw’è
    [SerializeField] GameObject symmetryLine;

    private bool isCleared = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCleared)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Click");

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Hit");
                if (hit.collider.gameObject == targetObject)
                {
                    targetObject.SetActive(false);
                    isCleared = true;
                    Debug.Log("Clear");
                    GameClear();
                }
            }
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
        SceneManager.LoadScene("END");
    }
}