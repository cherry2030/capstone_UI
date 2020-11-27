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

public class Furniture
{
    public string furnitureName;

    public Furniture()
    { }

    public Furniture(string furnitureName)
    {
        this.furnitureName = furnitureName;
    }
}
public class ARUIManagerScript : MonoBehaviour
{
    Camera _mainCam = null;

    /// <summary>
	/// 마우스의 상태
	/// </summary>
	private bool _mouseState;

    /// <summary>
    /// 마우스가 다운된 오브젝트
    /// </summary>
    private GameObject target;
    /// <summary>
    /// 마우스 좌표
    /// </summary>
    private Vector3 MousePos;

    [SerializeField]
    public float moveSpeed = 1.0f; //이동속도
    public float moveRot = 0.5f;

    private float gravity = -9.81f;
    private Transform tr = null;
    private Vector3 moveDirection; //이동방향

    public Text UIText;
    float distance = 10;
    public GameObject MenuPanel;
    public GameObject ARPanel;

    public Transform parent;
    public GameObject Furniture;

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    DatabaseReference mDatabaseRef;
    public Text FurnitureText_1;
    public bool isMenuOpen = false;


    public void CategoryPush()
    {
        FirebaseDatabase.DefaultInstance.GetReference("furniture").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("failed");
            }
            else if (task.IsCompleted)
            {
                Firebase.Database.DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Value.ToString());
                Debug.Log(FurnitureText_1.text);
                FurnitureText_1.text = snapshot.Child("furniture_1").Child("furniture_name").Value.ToString();
                Debug.Log(FurnitureText_1.text);
                /*
                foreach (var childSnapshot in snapshot.Children)
                {
                    Debug.Log(FurnitureText_1.text);
                    
                    FurnitureText_1.text = childSnapshot.Key.ToString() + "\n color : " +
                        childSnapshot.Child("color").Value.ToString() +
                        " texture : " +
                        childSnapshot.Child("texture").Value.ToString() +
                        " type : " +
                        childSnapshot.Child("type").Value.ToString();
                    
                    //FurnitureText_1.GetComponent<Image>.sor
                    FurnitureText_1.text = childSnapshot.Child("furniture_name").Value.ToString();
                    

                    Debug.Log(FurnitureText_1.text);


                }*/
                
            }

        });

        ARPanel.SetActive(false);
        MenuPanel.SetActive(true);
        isMenuOpen = true;
        
    }

    public void CategoryReturn()
    {
        MenuPanel.SetActive(false);
        ARPanel.SetActive(true);
        isMenuOpen = false;
    }


    public void CameraShotPush()
    {
        //사진촬영
    }

    public void ReturnButtonPush()
    {
        SceneManager.LoadScene("IntroScene");
    }

    void Awake()
    {
        _mainCam = Camera.main;
    }

    public void SelectFurniture()
    {
        isMenuOpen = false;
        Furniture.SetActive(true);
        MenuPanel.SetActive(false);
        ARPanel.SetActive(true);
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z - Furniture.transform.position.z);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Furniture.transform.position = objPosition;
    }


    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    protected virtual void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        // NOTE: You'll need to replace this url with your Firebase App's database
        // path in order for the database connection to work correctly in editor.
        app.SetEditorDatabaseUrl("https://capstonevivid.firebaseio.com/");
        if (app.Options.DatabaseUrl != null)
            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);

        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }


    // Update is called once per frame
    void Update()
    {
        FurnitureText_1.gameObject.SetActive(false);
        FurnitureText_1.gameObject.SetActive(true);
        if (!isMenuOpen)
        {
            //마우스가 내려갔는지?
            if (true == Input.GetMouseButtonDown(0))
            {
                Debug.Log("드래구중");
                //내려갔다.

                //타겟을 받아온다.
                target = GetClickedObject();

                //타겟이 있나?
                if (target != null)
                {
                    //있으면 마우스 정보를 바꾼다.
                    _mouseState = true;
                }

            }
            else if (true == Input.GetMouseButtonUp(0))
            {
                UIText.text = "가구를 이동하거나 배치해보세요!"; Debug.Log("드래그완료");
                //마우스가 올라 갔다.
                //마우스 정보를 바꾼다.
                _mouseState = false;
            }

            //마우스가 눌렸나?
            if (true == _mouseState)
            {
                //눌렸다!
                UIText.text = "가구를 원하는 곳에 배치해보세요!";


                //마우스 좌표를 받아온다.
                // MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, target.transform.position.z));
                // 위에껀 한번에 해서글너가 안됏는데 밑에는 된다 신기하다
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, target.transform.position.z);
                Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                target.transform.position = objPosition;

                //타겟의 위치 변경
                //target.transform.position = new Vector3(MousePos.x, MousePos.y, target.transform.position.z);
            }

        }
    }

    private GameObject GetClickedObject()
    {
        //충돌이 감지된 영역
        RaycastHit hit;
        //찾은 오브젝트
        GameObject target = null;

        //마우스 포이트 근처 좌표를 만든다.
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);

        //마우스 근처에 오브젝트가 있는지 확인
        if (true == (Physics.Raycast(ray.origin, ray.direction * 15, out hit)))
        {
            //있다!

            //있으면 오브젝트를 저장한다.
            target = hit.collider.gameObject;
            Debug.Log(target.name);
        }

        return target;
    }
}
