using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Firebase.Storage;
using System.IO;

public class FurnitureController : MonoBehaviour
{
    public Button putButton;
    public Button deleteButton;
    public Button RotateAndMoveButton;
    public string furnitureIndex = "furniture_1";
    public float width = 10;
    public bool isAddFurnitureMode = true;
    public bool isRotateOrMove = false;
    public bool isDeleting = false;
    public string savePath;
    Object prefab;

    public bool modelDownStart = false;
    bool isBundleDownloaded;
    bool isDownloadError;
    
    Vector3 vecBetweenGoAndPlayer;

    List<GameObject> furnitureObjects;

    public GameObject selectedFurniture;

    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.persistentDataPath;
        furnitureObjects = new List<GameObject>();
        isBundleDownloaded = false;
        isDownloadError = false;
        
    }


    // Update is called once per frame
    void Update()
    {
        if(isDownloadError == true)
        {
            Debug.Log("모델로딩에 실패했습니다.");
            isDownloadError = false;
        }
        if (modelDownStart == true)
        {
            Debug.Log("여긴들어옴");
            isBundleDownloaded = true;
            //DownloadBundle();
            modelDownStart = false;
        }

        if(isBundleDownloaded == true && System.IO.File.Exists(savePath + "/" + furnitureIndex))
        {
            AssetBundle furnitureBundle = AssetBundle.LoadFromFile(savePath + "/" + furnitureIndex);
            prefab = furnitureBundle.LoadAsset(furnitureIndex);
            furnitureBundle.Unload(false);
            isBundleDownloaded = false;
        }

        if (isDeleting == false)
        {
            ChangeSelectedFurniture();
        }
        else
        {
            DeleteFurniture();
        }

    }

    public void PutCollider(GameObject go)
    {
        if (go.transform.childCount == 0)
        {
            go.AddComponent<BoxCollider>();
        }
        else
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform tf = go.transform.GetChild(i);
                PutCollider(tf.gameObject);
            }

        }
    }

    //가구모델생성
    public void InstantiateFurniture()
    {
        string address = savePath + "/" + furnitureIndex;

        if(furnitureIndex == null)
        {
            Debug.Log("인덱스없음");
            return;
        }
        if(prefab == null)
        {
            GameObject.Find("DisplayText").GetComponent<Text>().text = "배치할 가구를 고르지 않으셨거나 모델이 다운로드중입니다.";
            return;
        }
        GameObject.Find("DisplayText").GetComponent<Text>().text = "이동모드. 가구를 터치해서 움직여주세요";

        GameObject go = Instantiate(prefab, transform.position, Quaternion.Euler(270f, 0f, 0f)) as GameObject;

        //스케일 추가
        go.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);


        go.transform.parent = GameObject.Find("Floor Plane").transform;
        GameObject obj1 = GameObject.Find("skp_camera_Last_Saved_SketchUp_View");
        GameObject obj2 = GameObject.Find("skp_camera_Scena_1");
        GameObject obj3 = GameObject.Find("skp_camera_Scena_2");
        Destroy(obj1);
        Destroy(obj2);
        Destroy(obj3);

        PutCollider(go);
        
        //다운받은 가구 에셋번들 가져와서 자식게임오브젝트로 두는데 다운에 문제가 좀 생겨서 일단 박스로 대체.
        
        /*
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.parent = GameObject.Find("ARPanel").transform.Find("Floor Plane").transform;
        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        */

        go.transform.position = GameObject.Find("ARPanel").transform.Find("Floor Plane").transform.Find("FurniturePosition").transform.position;
        go.AddComponent<DragGameObject>();
        furnitureObjects.Add(go);
        selectedFurniture = go;

        isAddFurnitureMode = false;
        GameObject.Find("Floor Plane").transform.Find("FurniturePosition").gameObject.SetActive(false);
        isAddFurnitureMode = false;
        isDeleting = false;
        isRotateOrMove = true;
        
    }



    //선택중인 가구 변경
    public void ChangeSelectedFurniture()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //실제 쓸 것. 터치(돌아감)
        
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                GameObject go = FindParent(hit.transform.gameObject);
                if (furnitureObjects.Contains(go))
                { 
                    selectedFurniture = go;
                }
                
            }
        }  
#elif UNITY_EDITOR
        //마우스
        if (Input.GetMouseButton(0))
        {
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject go = FindParent(hit.transform.gameObject);
                    if (furnitureObjects.Contains(go))
                    {
                        selectedFurniture = go;
                    }

                }
            }
        }
#endif


    }

    //터치시 가구 삭제
    public void DeleteFurniture()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject go = FindParent(hit.transform.gameObject);
                if (furnitureObjects.Contains(go))
                {
                    selectedFurniture = go;
                    furnitureObjects.Remove(selectedFurniture);
                    Destroy(selectedFurniture);
                }

            }
        }
        
#elif UNITY_EDITOR
        //마우스
        if (Input.GetMouseButton(0))
        {
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject go = FindParent(hit.transform.gameObject);
                    if (furnitureObjects.Contains(go))
                    {
                        selectedFurniture = go;
                        furnitureObjects.Remove(selectedFurniture);
                        Destroy(selectedFurniture);
                        if (furnitureObjects == null)
                        {
                            furnitureObjects = new List<GameObject>();
                        }
                    }

                }
            }
        }
#endif


    }

    //삭제모드로변경
    public void ToDeleteMode()
    {
        if (isDeleting == false)
        {
            isDeleting = true;
            GameObject.Find("DisplayText").GetComponent<Text>().text = "삭제모드. 삭제할 가구를 터치해주세요";

        }
        else
        {
            isDeleting = false;
            
        }
    }

    //가구 이동모드로 전환
    public void ToMoveMode()
    {
        isRotateOrMove = true;
        isDeleting = false;
        isAddFurnitureMode = false;
        GameObject.Find("DisplayText").GetComponent<Text>().text = "이동모드. 가구를 터치해서 움직여주세요";
        GameObject.Find("Floor Plane").transform.Find("FurniturePosition").gameObject.SetActive(false);
        Debug.Log(isRotateOrMove);
    }
    
    //가구 회전모드로 전환
    public void ToRotateMode()
    {
        isRotateOrMove = false;
        isDeleting = false;
        isAddFurnitureMode = false;
        GameObject.Find("DisplayText").GetComponent<Text>().text = "회전모드. 가구를 터치해서 회전시켜주세요";
        GameObject.Find("Floor Plane").transform.Find("FurniturePosition").gameObject.SetActive(false);
        Debug.Log(isRotateOrMove);
    }

    //터치된 콜라이더의 부모(가구obj)찾기
    GameObject FindParent(GameObject go)
    {
        if (go.transform == GameObject.Find("ARPanel").transform)
        {
            return null;
        }
        if (furnitureObjects.Contains(go))
        {
            return go;
        }
        else
        {
            GameObject re = FindParent(go.transform.parent.gameObject);
            return re;
        }
    }

    //번들 다운로드
    public void DownloadBundle()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string path = "gs://capstonevivid.appspot.com/furniture-assetbundle/" + furnitureIndex;
        Firebase.Storage.StorageReference reference = storage.GetReferenceFromUrl(path);

        const long maxAllowedSize = 3 * 1024 * 1024;
        reference.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("bundle down fail");
                isDownloadError = true;
            }
            else
            {
                Debug.Log("bundle down done");
                //여기에 번들받기.
                if (Directory.Exists(savePath) == false)
                {
                    Directory.CreateDirectory(savePath);
                }
                File.WriteAllBytes(savePath +"/" + furnitureIndex, task.Result); 
                isBundleDownloaded = true;
            }
        });
    }

    public void tmpModelLoadButton()
    {
        Debug.Log("여기도들어옴");
        modelDownStart = true;
    }

    public void toAddFurnitureMode()
    {
        if(isAddFurnitureMode == false)
        {
            isAddFurnitureMode = true;
            isDeleting = false;
            GameObject.Find("DisplayText").GetComponent<Text>().text = "가구를 배치할 공간을 터치 후 배치버튼을 눌러주세요";
            GameObject.Find("Floor Plane").transform.Find("FurniturePosition").gameObject.SetActive(true);
        }
        else
        {
            InstantiateFurniture();
            
        }
    }
}
