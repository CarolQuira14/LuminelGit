using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    private Vector2 mousePos;
    [SerializeField] private GameObject cursor;
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        mousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        target = Camera.main.ScreenToWorldPoint(mousePos);
        cursor.transform.position = new Vector3(target.x, target.y, cursor.transform.position.z);
        //cursor.transform.position = Vector2.MoveTowards(cursor.transform.position, mousePos, 10f * Time.deltaTime);
    }
}
