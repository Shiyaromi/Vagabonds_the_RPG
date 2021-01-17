using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera; // Cinemachine camera package

    private float orthographicSize;
    private float targetOrthographicSize;

    private NPC currentTarget;

    private void Awake()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    void Update()
    {
        ClickTarget();

        HandleZoom();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null)
            {
                if (currentTarget != null) 
                    currentTarget.DeSelect();

                currentTarget = hit.collider.GetComponent<NPC>();

                player.MyTarget = currentTarget.Select();

                UIManager.Instance.ShowTargetFrame(currentTarget);
            }
            else // Deselect the target
            {
                UIManager.Instance.HideTargetFrame();

                if (currentTarget != null) currentTarget.DeSelect();

                currentTarget = null;
                player.MyTarget = null;
            }
        }
    }

    private void HandleZoom()
    {
        // With this you can zoom with ingame camera
        // TODO: The player can zoom farther based on skills
        if (Input.GetKey(KeyCode.LeftControl))
        {
            float zoomAmount = 2f;
            targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount; // TODO: You can invert this in option menu

            float minOrthoraphicSize = 7f;
            float maxOrthoraphicSize = 13f;
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthoraphicSize, maxOrthoraphicSize);

            float zoomSpeed = 5f;
            orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

            cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
        }
    }
}
