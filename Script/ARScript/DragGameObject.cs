using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragGameObject : MonoBehaviour
{

    Vector3 vecBetweenGoAndPlayer;

    float vecZ;
    bool isDeleting;
    bool isRotateOrMove;
    GameObject selectedFurniture;

    Vector2 currentPosition;
    Vector2 lastPosition;
    Touch touch;

    void Start()
    {
        currentPosition = new Vector2(0, 0);
        lastPosition = new Vector2(0, 0);
    }
    
    void Update()
    {   
        FurnitureController FC = GameObject.Find("Furniture Manager").GetComponent<FurnitureController>();
        isDeleting = FC.isDeleting;
        isRotateOrMove = FC.isRotateOrMove;

        if (isDeleting == false && FC != null) {

            selectedFurniture = FC.selectedFurniture;

            if (selectedFurniture != null)
            {
                if (isRotateOrMove == true)
                {
                    float speed = 0.5f;

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (Input.touchCount > 0)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            vecZ = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                            vecBetweenGoAndPlayer = selectedFurniture.transform.position - GetMousePos();
                        }


                        if (Input.GetTouch(0).phase == TouchPhase.Moved)
                        {
                            selectedFurniture.transform.position = new Vector3(GetMousePos().x + vecBetweenGoAndPlayer.x, transform.position.y, GetMousePos().z + vecBetweenGoAndPlayer.z);
                        }
                    }
                    
#elif UNITY_EDITOR
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        vecZ = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                        vecBetweenGoAndPlayer = selectedFurniture.transform.position - GetMousePos();
                    }

                    if (Input.GetMouseButton(0))
                    {
                        selectedFurniture.transform.position = new Vector3(GetMousePos().x + vecBetweenGoAndPlayer.x, selectedFurniture.transform.position.y, GetMousePos().z + vecBetweenGoAndPlayer.z);
                    }
#endif

                }

                else
                {
                    float speed = 0.3f;

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (Input.touchCount > 0)
                    {
                        touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Moved)
                        {
                            selectedFurniture.transform.Rotate(0f, 0f,  - touch.deltaPosition.x * speed);
                        }
                    }
                    

#elif UNITY_EDITOR
                    if (Input.GetMouseButtonDown(0))
                    {
                        currentPosition = Input.mousePosition;
                        lastPosition = Input.mousePosition;
                    }

                    if (Input.GetMouseButton(0))
                    {
                        currentPosition = Input.mousePosition;
                        selectedFurniture.transform.Rotate(0f, 0f, (lastPosition.x - currentPosition.x) * speed);
                        lastPosition = currentPosition;
                    }
#endif
                }
            }
        }
    }
    Vector3 GetMousePos()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Vector3 touchPoint = Input.GetTouch(0).position;  //터치
#elif UNITY_EDITOR
        Vector3 touchPoint = Input.mousePosition; //마우스
#endif     

        touchPoint.z = vecZ;

        return Camera.main.ScreenToWorldPoint(touchPoint);
    }
}
