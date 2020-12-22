using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;

public class HorizontalPlaneManager : MonoBehaviour
{
    bool isFloorCheck;

    // Start is called before the first frame update
    void Start()
    {
        isFloorCheck = false;
        //CreatePlanes();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFloorCheck == false && GameObject.Find("Floor Plane").transform.GetChild(0).gameObject.activeSelf == true)
        {
            GameObject.Find("ARPanel").transform.Find("Horizontal Plane").gameObject.SetActive(false);
            GameObject.Find("DisplayText").GetComponent<Text>().text = "공간인식 완료. 가구를 배치할 공간을 터치 후 배치버튼을 눌러주세요";
            isFloorCheck = true;
        }
    }

    /*
    void CreatePlanes()
    {
        //복사
        GameObject originPlane = GameObject.Find("ARPanel").transform.Find("Horizontal Plane Origin").gameObject;
        GameObject originFloor = GameObject.Find("ARPanel").transform.Find("Floor Plane Origin").gameObject;

        //붙이기
        GameObject horizontalP = Instantiate(originPlane, transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
        GameObject floorP = Instantiate(originFloor, transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;

        //
        horizontalP.name = "Horizontal Plane";
        floorP.name = "Floor Plane";

        horizontalP.transform.parent = GameObject.Find("ARPanel").transform;
        floorP.transform.parent = GameObject.Find("ARPanel").transform;

        horizontalP.gameObject.SetActive(true);
        floorP.gameObject.SetActive(true);

        isFloorCheck = false;
    }
    
    void DeletePlanes()
    {
        GameObject plane = GameObject.Find("Horizontal Plane").gameObject;
        GameObject floor = GameObject.Find("Floor Plane").gameObject;

        Destroy(plane);
        Destroy(floor);
    }

    void ResetPlanes()
    {
        DeletePlanes();
        CreatePlanes();
    }
    */
}
