using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePositionInit : MonoBehaviour
{
    //public bool isFurniturePositioning;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject.Find("FurniturePosition").transform.position = new Vector3(hit.point.x, hit.point.y+0.01f, hit.point.z);
            }
        }
        
#elif UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                GameObject.Find("FurniturePosition").transform.position = new Vector3(hit.point.x, hit.point.y+0.01f, hit.point.z);

            }
        }

#endif
    }
}
