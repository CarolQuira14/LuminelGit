using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableObject : MonoBehaviour
{
    RaycastHit2D hit;
    private Vector3 target;
    private Vector3 mousePos;
    private Transform dragObj;
    bool isDragging;
    void Start()
    {
        isDragging = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Draggable"))
            {
                dragObj = hit.transform;
                hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                isDragging = true;
            }
        }else if (Input.GetMouseButtonUp(0) && isDragging)
            {
                hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                isDragging = false;
                //hit.collider.gameObject = null;
            }
            else if (isDragging)
            {
                mousePos = Input.mousePosition;
                mousePos.z = -Camera.main.transform.position.z; // Set a distance from the camera
                target = Camera.main.ScreenToWorldPoint(mousePos);

                dragObj.position = new Vector3(target.x, target.y, dragObj.position.z);
            }
    }
}