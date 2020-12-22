using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;

using GooglePlayGames;
using GooglePlayGames.BasicApi;

using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Firebase.Auth;
using System.IO;
//using BackEnd
using System.Globalization;

public class FurnitureData
{
    public string furnitureBrand;
    public string furnitureClicked;
    public string furnitureDepth;
    public string furnitureHeight;
    public string furnitureLiked;
    public string furnitureName;
    public string furnitureWidth;
    public string furnitureImageAddress;
    public string furnitureIndex;
    public string mood;
    public string space;
    public string subtype;
    public string type;
    public List<string> texture;

    public FurnitureData(string _furniture_brand, string _furniture_clicked, string _furniture_depth, string _furniture_height,
        string _furniture_liked, string _furniture_name, string _furniture_width, string _furniture_image,
        string _furniture_index, string _mood, string _space, string _subtype, string _type)
    {
        furnitureBrand = _furniture_brand;
        furnitureClicked = _furniture_clicked;
        furnitureDepth = _furniture_depth;
        furnitureHeight = _furniture_height;
        furnitureLiked = _furniture_liked;
        furnitureName = _furniture_name;
        furnitureWidth = _furniture_width;
        furnitureIndex = _furniture_index;
        mood = _mood;
        space = _space;
        subtype = _subtype;
        type = _type;
        furnitureImageAddress = _furniture_image;

        texture = new List<string>();
    }
}

public class UserData
{
    public string user_name;
    public string user_email;
    public string user_age;
    public string user_sex;
    public string user_housesize;
    public string user_roomtype;
    public string user_index;
    public string user_likemood;

    public UserData()
    {
    }

    public UserData(string _userEmail)
    {
        this.user_email = _userEmail;
    }
    public UserData(string _username, string _useremail, string _userage, string _usersex,
        string _userhousesize, string _userroomtype, string _userindex, string _userlikemood)
    {
        this.user_name = _username;
        this.user_email = _useremail;
        this.user_age = _userage;
        this.user_sex = _usersex;
        this.user_housesize = _userhousesize;
        this.user_roomtype = _userroomtype;
        this.user_index = _userindex;
        this.user_likemood = _userlikemood;
    }
}

public class IntroManager : MonoBehaviour
{
    //public FurnitureData[] _fd = new FurnitureData[7];
    public List<FurnitureData> FurnitureDataList = new List<FurnitureData>();
    public UserData UserDataList;

    IEnumerator ShowLikeResult()
    {
        LoadingImage.SetActive(true);
        //List 초기화
        ResetResult();

        Debug.Log(nowLogin_UserID + " ShowLikeResult 시작");

        FirebaseDatabase.DefaultInstance.GetReference("like").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("failed");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Value.ToString());
                foreach (var childSnapshot in snapshot.Children)
                {
                    Debug.Log("ShowLikeResult foreach 들어왓다" + childSnapshot.Child("user_index").Value.ToString());
                    //Debug.Log(nowLogin_UserID);
                    if ((string)childSnapshot.Child("user_index").Value.ToString() == nowLogin_UserID && ((string)childSnapshot.Child("event").Value.ToString() == "LIKE" || (string)childSnapshot.Child("event").Value.ToString() == "BOTH"))
                    {
                        Debug.Log(childSnapshot.Child("furniture_index").Value.ToString() + "ShowLikeResult like  if문 들어왓다  " + nowLogin_UserID + ", " + childSnapshot.Child("event").Value.ToString());
                        //ResultFurnList_Image.Add(childSnapshot.Child("image").Value.ToString());
                        //Debug.Log(childSnapshot.Child("image").Value.ToString() + childSnapshot.Child("furniture_name").Value.ToString());
                        //Debug.Log("ShowLikeResult foreach foreach  들어왓다  " + childSnapshot.Child("furniture_index").Value.ToString());

                        string furnIndex = childSnapshot.Child("furniture_index").Value.ToString();
                        ResultFurnList_name.Add(furnIndex);
                    }
                }
                //Debug.Log(ResultFurnList_name[0]);
            }
        });
        yield return new WaitForSeconds(5);
        //Debug.Log(ResultFurnList_name);
        //yield return new WaitForSeconds(1);
        FirebaseDatabase.DefaultInstance.GetReference("furniture").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("failed");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());
                foreach (var childSnapshot in snapshot.Children)
                {
                    for (int i = 0; i < ResultFurnList_name.Count; i++)
                    {
                        // Debug.Log(" foreach 들어왓다  " + childSnapshot.Child(TopCategory).Value.ToString());
                        if (childSnapshot.Key.ToString() == ResultFurnList_name[i])
                        {
                            FurnitureDataList.Add(new FurnitureData(
                            childSnapshot.Child("furniture_brand").Value.ToString(),
                            childSnapshot.Child("furniture_clicked").Value.ToString(),
                            childSnapshot.Child("furniture_depth").Value.ToString(),
                            childSnapshot.Child("furniture_height").Value.ToString(),
                            childSnapshot.Child("furniture_liked").Value.ToString(),
                            childSnapshot.Child("furniture_name").Value.ToString(),
                            childSnapshot.Child("furniture_width").Value.ToString(),
                             childSnapshot.Child("image").Value.ToString(),
                            childSnapshot.Key.ToString(),
                            childSnapshot.Child("mood").Value.ToString(),
                            childSnapshot.Child("space").Value.ToString(),
                            childSnapshot.Child("subtype").Value.ToString(),
                            childSnapshot.Child("type").Value.ToString()
                           ));

                            Debug.Log("ShowLikeResult furniture  if문 들어왓다  " + childSnapshot.Child("image").Value.ToString());
                            ResultFurnList_Image.Add(childSnapshot.Child("image").Value.ToString());
                            //Debug.Log(childSnapshot.Child("image").Value.ToString() + childSnapshot.Child("furniture_name").Value.ToString());

                            break;
                        }
                    }
                }
            }
            PushMoreFurnButton();
            LoadingImage.SetActive(false);
        });
    }

    IEnumerator ShowRecommendResult()
    {
        LoadingImage.SetActive(true);
        //List 초기화
        ResetResult();
        Debug.Log(nowLogin_UserID + " showRecommendResult 시작");

        FirebaseDatabase.DefaultInstance.GetReference("recommendation").Child(nowLogin_UserID).GetValueAsync().ContinueWith(task => 
        {

            if (task.IsFaulted)
            {
                Debug.Log("failed");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("showRecommendResult foreach  if문 들어왓다  " + nowLogin_UserID);

                 foreach (var snapshot in task.Result.Children)
                {
                    Debug.Log("showRecommendResult  foreach  들어왓다  " + snapshot.Value.ToString());
                    string furnIndex = (string)snapshot.Value.ToString();
                    //Debug.Log("!" + furnIndex);
                    ResultFurnList_name.Add(furnIndex);
                }
            }

        });

        yield return new WaitForSeconds(4);
        FirebaseDatabase.DefaultInstance.GetReference("furniture").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("failed");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("숫자"+ResultFurnList_name.Count);
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());
                foreach (var childSnapshot in snapshot.Children)
                {
                    //Debug.Log(" foreach 들어왓다  " + childSnapshot.Child(TopCategory).Value.ToString());
                    for (int i = 0; i < ResultFurnList_name.Count; i++)
                    {
                        //Debug.Log(ResultFurnList_name[i] + "<" + childSnapshot.Key.ToString());
                        //Debug.Log(" foreach 들어왓다  " + childSnapshot.Child(TopCategory).Value.ToString());
                        if (childSnapshot.Key.ToString() == ResultFurnList_name[i])
                        {
                            Debug.Log("Recommendation furniture  if문 들어왓다  " + nowLogin_UserID);
                            ResultFurnList_Image.Add(childSnapshot.Child("image").Value.ToString());
                            Debug.Log(childSnapshot.Child("image").Value.ToString() + childSnapshot.Child("furniture_name").Value.ToString());
                            FurnitureDataList.Add(new FurnitureData(
                            childSnapshot.Child("furniture_brand").Value.ToString(),
                            childSnapshot.Child("furniture_clicked").Value.ToString(),
                            childSnapshot.Child("furniture_depth").Value.ToString(),
                            childSnapshot.Child("furniture_height").Value.ToString(),
                            childSnapshot.Child("furniture_liked").Value.ToString(),
                            childSnapshot.Child("furniture_name").Value.ToString(),
                            childSnapshot.Child("furniture_width").Value.ToString(),
                             childSnapshot.Child("image").Value.ToString(),
                            childSnapshot.Key.ToString(),
                            childSnapshot.Child("mood").Value.ToString(),
                            childSnapshot.Child("space").Value.ToString(),
                            childSnapshot.Child("subtype").Value.ToString(),
                            childSnapshot.Child("type").Value.ToString()
                           ));

                            break;
                        }
                    }
                }
            }

            PushMoreFurnButton();
            LoadingImage.SetActive(false);
        });

    }
    // public List<Button> RcmResultFurnList = new List<Button>();

    public string nowLogin_UserID, nowLogin_DeviceID;
    public void LogCheck()
    {
        LoadingImage.SetActive(true);
        string temp_deviceID;//=SystemInfo.deviceUniqueIdentifier;temp_deviceID
        Debug.Log("          LogCheck시작");

        FirebaseDatabase.DefaultInstance.GetReference("login").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("failed");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(" task.IsCompleted 들어왓다  ");
                foreach (var childSnapshot in snapshot.Children)
                {
                    temp_deviceID = childSnapshot.Child("deviceID").Value.ToString();
                    Debug.Log(" foreach 들어왓다  " + temp_deviceID);

                    if (nowLogin_DeviceID == temp_deviceID)
                    {
                        LoadingImage.SetActive(true);
                        Debug.Log("foreach  if문 들어왓다  " + temp_deviceID);
                        IsLogIn = true;
                        nowLogin_UserID = childSnapshot.Child("userID").Value.ToString();
                        nowLogin_DeviceID = temp_deviceID;

                        UserDataList = new UserData(
                                    mDatabaseRef.Child("user").Child("user_1").Child("user_age").ToString()
                                    , mDatabaseRef.Child("user").Child("user_1").Child("user_email").ToString()
                                    , mDatabaseRef.Child("user").Child("user_1").Child("user_age").ToString()
                                    , mDatabaseRef.Child("user").Child("user_1").Child("user_sex").ToString()
                                    , mDatabaseRef.Child("user").Child("user_1").Child("user_housesize").ToString()
                               , mDatabaseRef.Child("user").Child("user_1").Child("user_roomtype").ToString()
                               , userNum.ToString()
                               , mDatabaseRef.Child("user").Child("user_1").Child("user_likemood").ToString()
                               );   

                        break;
                    }
                    else
                        IsLogIn = false;
                }
            }
            

            LoadingImage.SetActive(false);
            LogChange(IsLogIn);
        }
        );
    }
    // Start is called before the first frame update
    private float screenRatio;
    void Start()
    {
        //FurnitureDataList = new List<FurnitureData>();
        /*
        screenRatio = (float)Screen.width / (float)Screen.height;
        if (screenRatio < 0.520f)
        {
            if (gameObject.name == "Camera")
            {
                GetComponent<Camera>().orthographicSize = (screenRatio - 0.5625f) * (-2.189f) + 1f;
            }

            else if (gameObject.name == "Main Camera")
            {
                GetComponent<Camera>().orthographicSize = (screenRatio - 0.5625f) * (-18.905f) + 8.7f;
            }
        }

        else
        {
            if (gameObject.name == "Camera")
            {
                GetComponent<Camera>().orthographicSize = 1f;
            }

            else if (gameObject.name == "Main Camera")
            {
                GetComponent<Camera>().orthographicSize = 8.7f;
            }
        }*/
        /*
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
        */
        InitializeFirebase();

#if UNITY_ANDROID && !UNITY_EDITOR
        //InitializeSDK();        
        nowLogin_DeviceID =SystemInfo.deviceUniqueIdentifier;
        Debug.Log(nowLogin_DeviceID+ "   start   시작");
        LogCheck();
        //LogChange(IsLogIn);
          //로그인 체크후 로그인 처음부터할꺼면 이거하고 아니면 말고
       //OpenSearchPanel();
       //EditorOnLogin();
#elif UNITY_EDITOR
        EditorOnLogin();
        // OpenSearchPanel();
        //LogChange(IsLogIn);
        //OpenLogReadyPanel();
        //OpenUserInfoPanel();
        //OpenUserInfoPanel1();
#endif
    }

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    DatabaseReference mDatabaseRef;

    public void ChangeARScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ChangeSearchScene()
    {
        SceneManager.LoadScene("SearchScene");
    }

    public void QuitButtonPush()
    {
        Application.Quit();
    }

    public GameObject LogInPanel;
    public GameObject NowPanel;
    public GameObject LastPanel;
    public GameObject BeforeBottomPanelChoose;

    public GameObject LogReadyPanel;
    public GameObject UserInfoPanel1;
    public GameObject UserInfoPanel2;
    public GameObject UserInfoPanel3;

    public GameObject BottomBarPanel;
    public GameObject RcmFurnPanel;
    public GameObject SearchPanel;
    public GameObject MyPagePanel;

    public GameObject SearchResultPanel;

    public GameObject MyLikePanel;
    public GameObject MySettingPanel;
    public GameObject MyAboutPanel;

    public GameObject FurnitureInfoPanel;
    public GameObject LoadingImage;

    //user 정보 입력 
    public Text user_name_Text;
    public string user_name_temp;
    public string user_email_temp; //= "tkdlqjzotl@gmail.com"
    public void Update_user_name()
    {
        user_name_temp = user_name_Text.text;
    }

    public Text user_age_Text;

    public string user_age_temp;
    public void Update_user_age()
    {
        user_age_temp = user_age_Text.text;
    }
    public string user_sex_temp;
    public Color SelectButtonColor = new Color(96, 125, 139), DeselectButtonColor = Color.gray;//Color.blue"#607D8B"

    public Button manButton, womanButton;
    public ColorBlock cb_manButton, cb_womanButton;

    public void Update_user_sex(string _sex)
    {
        //cb_manButton = manButton.colors;
        //cb_womanButton = womanButton.colors;

        user_sex_temp = _sex;
        if (_sex == "1")
        {
            Debug.Log(_sex);
            manButton.GetComponent<Image>().sprite = Resources.Load("image/남자버튼", typeof(Sprite)) as Sprite;
            womanButton.GetComponent<Image>().sprite = Resources.Load("image/클릭시여자버튼", typeof(Sprite)) as Sprite;

            //cb_manButton.normalColor = SelectButtonColor;
            //cb_womanButton.normalColor = DeselectButtonColor;
        }
        else if (_sex == "0")
        {
            Debug.Log(_sex);
            manButton.GetComponent<Image>().sprite = Resources.Load("image/클릭시남자버튼", typeof(Sprite)) as Sprite;
            womanButton.GetComponent<Image>().sprite = Resources.Load("image/여자버튼", typeof(Sprite)) as Sprite;

            //cb_manButton.normalColor = DeselectButtonColor;
            //cb_womanButton.normalColor = SelectButtonColor;
        }
        //manButton.colors = cb_manButton;
        //womanButton.colors = cb_womanButton;
    }

    public Button OneRoomButton, TwoRoomButton, ETCRoomButton;
    public ColorBlock cb_OneRoomButton, cb_TwoRoomButton, cb_ETCRoomButton;
    public string user_roomtype_temp;

    public void Update_user_roomtype(string _room)
    {
        user_roomtype_temp = _room;

        if (_room == "0")
        {
            OneRoomButton.GetComponent<Image>().sprite = Resources.Load("image/클릭시원룸", typeof(Sprite)) as Sprite;
            TwoRoomButton.GetComponent<Image>().sprite = Resources.Load("image/투룸", typeof(Sprite)) as Sprite;
            ETCRoomButton.GetComponent<Image>().sprite = Resources.Load("image/기타", typeof(Sprite)) as Sprite;
        }
        else if (_room == "1")
        {
            OneRoomButton.GetComponent<Image>().sprite = Resources.Load("image/원룸", typeof(Sprite)) as Sprite;
            TwoRoomButton.GetComponent<Image>().sprite = Resources.Load("image/클릭시투룸", typeof(Sprite)) as Sprite;
            ETCRoomButton.GetComponent<Image>().sprite = Resources.Load("image/기타", typeof(Sprite)) as Sprite;
        }

        else if (_room == "2")
        {
            OneRoomButton.GetComponent<Image>().sprite = Resources.Load("image/원룸", typeof(Sprite)) as Sprite;
            TwoRoomButton.GetComponent<Image>().sprite = Resources.Load("image/투룸", typeof(Sprite)) as Sprite;
            ETCRoomButton.GetComponent<Image>().sprite = Resources.Load("image/클릭시기타", typeof(Sprite)) as Sprite;
        }
    }
    public Button MySetting_manButton, MySetting_womanButton;
    //public ColorBlock MySetting_cb_manButton, MySetting_cb_womanButton;

    public void MySetting_Update_user_sex(string _sex)
    {
        user_sex_temp = _sex;
        if (_sex == "0")
        {
            MySetting_manButton.GetComponent<Image>().sprite = Resources.Load("image/클릭시setting_남버튼", typeof(Sprite)) as Sprite;
            MySetting_womanButton.GetComponent<Image>().sprite = Resources.Load("image/setting_남버튼", typeof(Sprite)) as Sprite;
        }
        else if (_sex == "1")
        {
            MySetting_manButton.GetComponent<Image>().sprite = Resources.Load("image/setting_여버튼", typeof(Sprite)) as Sprite;
            MySetting_womanButton.GetComponent<Image>().sprite = Resources.Load("image/클릭시setting_여버튼", typeof(Sprite)) as Sprite;
        }
    }

    public Button MySetting_OneRoomButton, MySetting_TwoRoomButton, MySetting_ETCRoomButton;
    //public ColorBlock MySetting_cb_OneRoomButton, MySetting_cb_TwoRoomButton, MySetting_cb_ETCRoomButton;

    public void MySetting_Update_user_roomtype(string _room)
    {
        user_roomtype_temp = _room;

        if (_room == "0")
        {
            MySetting_OneRoomButton.GetComponent<Image>().sprite = Resources.Load("image/클릭시setting_원룸버튼", typeof(Sprite)) as Sprite;
            MySetting_TwoRoomButton.GetComponent<Image>().sprite = Resources.Load("image/setting_투룸버튼", typeof(Sprite)) as Sprite;
            MySetting_ETCRoomButton.GetComponent<Image>().sprite = Resources.Load("image/setting_기타버튼", typeof(Sprite)) as Sprite;
        }
        else if (_room == "1")
        {
            MySetting_OneRoomButton.GetComponent<Image>().sprite = Resources.Load("image/setting_원룸버튼", typeof(Sprite)) as Sprite;
            MySetting_TwoRoomButton.GetComponent<Image>().sprite = Resources.Load("image/클릭시setting_투룸버튼", typeof(Sprite)) as Sprite;
            MySetting_ETCRoomButton.GetComponent<Image>().sprite = Resources.Load("image/setting_기타버튼", typeof(Sprite)) as Sprite;
        }
        else if (_room == "2")
        {
            MySetting_OneRoomButton.GetComponent<Image>().sprite = Resources.Load("image/setting_원룸버튼", typeof(Sprite)) as Sprite;
            MySetting_TwoRoomButton.GetComponent<Image>().sprite = Resources.Load("image/setting_투룸버튼", typeof(Sprite)) as Sprite;
            MySetting_ETCRoomButton.GetComponent<Image>().sprite = Resources.Load("image/클릭시setting_기타버튼", typeof(Sprite)) as Sprite;
        }
    }
    public Dropdown dropdown;
    public void Dropdown_LikeMood()
    {
        user_likemood_temp = dropdown.value.ToString();
    }

    public Text user_housesize_Text;
    public string user_housesize_temp;
    public void Update_user_housesize()
    {
        user_housesize_temp = user_housesize_Text.text;
    }
    public void MySetting_Update_user_housesize()
    {
        user_housesize_temp = MySettingPanel.transform.FindChild("User_HouseSIze_InputField2").GetComponent<InputField>().text;
    }
    public void MySetting_Update_user_name()
    {
        user_name_temp = MySettingPanel.transform.FindChild("User_Name_InputField").GetComponent<InputField>().text;
    }
    public void MySetting_Update_user_age()
    {
        user_age_temp = MySettingPanel.transform.FindChild("User_Age_InputField2").GetComponent<InputField>().text;
    }


    public GameObject[] LikeMoodButton = new GameObject[7];
    public Text[] LikeMoodButtonText = new Text[7];
    private Vector2 LikeMoodButtonRectTransform;
    private Vector3 LikeMoodButtonRectTransform_position;
    private int selectedIndex = 0;

    public GameObject UserInfoReadyButton;
    public GameObject ImageUploadButton;
    public string user_likemood_temp;
    public void LikeMoodToggleValueChange(int _index)
    {
        selectedIndex = _index;
        user_likemood_temp = LikeMoodButtonText[_index].text.ToString();
        Debug.Log("LikeMoodToggleValueChange " + user_likemood_temp);
        OpenAskSelectMood(LikeMoodButton[selectedIndex].transform.FindChild("Background").GetComponent<Image>().sprite, user_likemood_temp);
    }

    public int userNum = 1;
    private void writeNewUser_Test(string _username, string _useremail, string _userage, string _usersex,
        string _userhousesize, string _userroomtype, string _userindex, string _userlikemood)
    {
        Debug.Log("writeNewUser_Test 시작!" + _username +"이름 "
            + _userage + " 살 "
            + _useremail + " email "
            + _usersex + " 성별 "
            + _userhousesize + " 집크기 "
            + _userroomtype + " 집형태 "
            + userNum.ToString() + " 유저인덱스 "
            + _userlikemood + " 유저무드 "
            );
        //User user = new User("user_1");
        //UserDataList.Add();
        UserDataList=   new UserData(_username, _useremail, _userage, _usersex,
           _userhousesize, _userroomtype, userNum.ToString(), _userlikemood);
        string json = JsonUtility.ToJson(user);
        mDatabaseRef.Child("user").Child("user_1").Child("user_age").SetValueAsync(_userage);
        mDatabaseRef.Child("user").Child("user_1").Child("user_email").SetValueAsync(_useremail);
        mDatabaseRef.Child("user").Child("user_1").Child("user_housesize").SetValueAsync(_userhousesize);

        // mDatabaseRef.Child("user").Child("user_1").Child("user_index").SetValueAsync(_userindex);

        mDatabaseRef.Child("user").Child("user_1").Child("user_likemood").SetValueAsync(_userlikemood);

        mDatabaseRef.Child("user").Child("user_1").Child("user_name").SetValueAsync(_username);
        mDatabaseRef.Child("user").Child("user_1").Child("user_roomtype").SetValueAsync(_userroomtype);
        mDatabaseRef.Child("user").Child("user_1").Child("user_sex").SetValueAsync(_usersex);

        nowLogin_UserID = "user_" + userNum.ToString();
        mDatabaseRef.Child("login").Child("login_1").Child("deviceID").SetValueAsync(nowLogin_DeviceID);
        mDatabaseRef.Child("login").Child("login_1").Child("userID").SetValueAsync(nowLogin_UserID);
    }

    public void USerInfoUpdate()  // 빌드시에 설정 필수!! editor에선 풀어주기 
    {
        Debug.Log("USerInfoUpdate 시작!");
#if UNITY_ANDROID && !UNITY_EDITOR
        user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log("사용자불러오기성공!");
            // user_name_temp = user.DisplayName;
            user_email_temp = user.Email;
            //System.Uri photo_url = user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = user.UserId;
        }
        else
        {
            Debug.Log("사용자불러오기실패!");
        }
#endif
        USerInfoEdit();     
        //userNum++;
    }

    public void USerInfoEdit()
    {
        writeNewUser_Test(user_name_temp, user_email_temp, user_age_temp, user_sex_temp,
            user_housesize_temp, user_roomtype_temp, userNum.ToString(), user_likemood_temp);
    }

    // = FirebaseAuth.DefaultInstance;
    FirebaseAuth auth = FirebaseAuth.DefaultInstance;
    public void OpenUserInfoPanel()
    {
        LoadingImage.SetActive(false);
        LastPanel = NowPanel;
        NowPanel.SetActive(false);
        UserInfoPanel1.SetActive(true);
        UserInfoPanel2.SetActive(true);
        UserInfoPanel3.SetActive(true);
    }

    public GameObject WarnNoResultFurniture;
    public GameObject PopupEndRevise;
    public GameObject WarnNoInsertInfo;
    public GameObject WarnNoLink;
    public GameObject WarnNetworkPanel1;
    public GameObject PopupAddLikeFurn;
    public GameObject DeleteFurn;

    public void QuitWarnigPanel()
    {
        WarnNoResultFurniture.SetActive(false);
        PopupEndRevise.SetActive(false);
        WarnNoInsertInfo.SetActive(false);
        WarnNoLink.SetActive(false);
        WarnNetworkPanel1.SetActive(false);
        PopupAddLikeFurn.SetActive(false);
        DeleteFurn.SetActive(false);
    }

    public void OpenUserInfoPanel1()
    {
        UserInfoPanel1.transform.forward = new Vector3(0, 0, 0);
        UserInfoPanel2.transform.forward = new Vector3(500, 0, 0);
        UserInfoPanel3.transform.forward = new Vector3(500, 0, 0);
        LastPanel = NowPanel;
        NowPanel = UserInfoPanel1;
    }
    public void OpenUserInfoPanel2()
    {
        if (user_name_temp == "" || user_age_temp == "" || user_sex_temp == "")
        {
            WarnNoInsertInfo.SetActive(true);
        }
        else
        {
            UserInfoPanel1.transform.forward = new Vector3(500, 0, 0);
            UserInfoPanel2.transform.forward = new Vector3(0, 0, 0);
            UserInfoPanel3.transform.forward = new Vector3(500, 0, 0);
            LastPanel = NowPanel;
            NowPanel = UserInfoPanel2;
        }
    }
    public void OpenUserInfoPanel3()
    {
        if (user_housesize_temp == "" || user_roomtype_temp == "")
        {
            Debug.Log("정보입력 경고 ");
            WarnNoInsertInfo.SetActive(true);
        }

        else
        {
            LastPanel = NowPanel;
            Debug.Log(AskSelectMoodPanel);
            AskSelectMoodPanel.SetActive(false);
            UserInfoPanel1.transform.forward = new Vector3(500, 0, 0);
            UserInfoPanel2.transform.forward = new Vector3(500, 0, 0);
            UserInfoPanel3.transform.forward = new Vector3(0, 0, 0);
            Debug.Log(" 유저인포3 등장해!");
            NowPanel = UserInfoPanel3;
            //user_likemood_temp = "";
        }
    }


    public GameObject AskSelectMoodPanel;
    public void OpenAskSelectMood(Sprite _sprite, string _string)
    {
        //UserInfoPanel3.transform.forward = new Vector3(500, 0, 0);
        AskSelectMoodPanel.transform.FindChild("Image").GetComponent<Image>().sprite = _sprite;
        AskSelectMoodPanel.transform.FindChild("Text").GetComponent<Text>().text = _string + " 스타일을\n선택한 것이 맞습니까 ? ";

        //LastPanel = NowPanel;
        Debug.Log("OpenAskSelectMood");
        //NowPanel.SetActive(false);
        AskSelectMoodPanel.SetActive(true);
        LastPanel = NowPanel;
        NowPanel = AskSelectMoodPanel;
    }

    public void Push_YesButton_AskSelectMood()
    {
        Debug.Log("1" + LastPanel);
        //Debug.Log("Push_YesButton_AskSelectMood"+ LastPanel.name + NowPanel.name);
        if (LastPanel == UserInfoPanel3)
        {
            Debug.Log("Push_YesButton_AskSelectMood if  UserInfoPanel3" + LastPanel.name + NowPanel.name);
            USerInfoUpdate();

            AskSelectMoodPanel.SetActive(false);
            NowPanel.SetActive(false);
            CloseUserInfoPanel();
        }
        else
        {
            Debug.Log("Push_YesButton_AskSelectMood else" + LastPanel.name + NowPanel.name);
            user_likemood_temp = LikeMoodButtonText[1].text.ToString();
            MySettingPanel.transform.FindChild("User_LikeMood_Furniture_Select").transform.FindChild("Text").GetComponent<Text>().text = user_likemood_temp;

            NowPanel.SetActive(false);
            LastPanel.SetActive(true);
            NowPanel = LastPanel;
            LastPanel = MyPagePanel;
        }
    }

    public void Push_NoButton_AskSelectMood()
    {
        Debug.Log("Push_NoButton_AskSelectMood UserInfoPanel3" + LastPanel.name + NowPanel.name);
        if (LastPanel == UserInfoPanel3)
        {
            AskSelectMoodPanel.SetActive(false);
            UserInfoPanel3.transform.forward = new Vector3(0, 0, 0);
            NowPanel = UserInfoPanel3;
            user_likemood_temp = "";
        }
        else
        {
            AskSelectMoodPanel.SetActive(false);
            Debug.Log("Push_NoButton_AskSelectMood else" + LastPanel.name + NowPanel.name);
        }
    }

    public void CloseUserInfoPanel()
    {
        NowPanel.SetActive(false);
        UserInfoPanel1.SetActive(false);
        UserInfoPanel2.SetActive(false);
        UserInfoPanel3.SetActive(false);
        IsLogIn = true;
        LogChange(IsLogIn);
    }

    public Button RcmFurnButton, SearchButton, MyPageButton;
    public ColorBlock cb_RcmFurnButton, cb_SearchButton, cb_MyPageButton;

    public bool IsLogIn = false;
    public void LogChange(bool _IsLogIn)
    {
        IsLogIn = _IsLogIn;
        if (IsLogIn == true)
        {

            OpenLogReadyPanel();
            //BottomBarPanel.SetActive(true);
        }
        else
        {
            OpenLogInPanel();
            //BottomBarPanel.SetActive(false);
        }
    }
    public void OpenLogInPanel()
    {
        IsLogIn = false;
        if (NowPanel != null)
            NowPanel.SetActive(false);
        //LastPanel = NowPanel;
        LogInPanel.SetActive(true);
        NowPanel = LogInPanel;
        //BottomBarPanel.SetActive(false);

    }
    public void OpenLogReadyPanel()
    {
        IsLogIn = true;
        NowPanel.SetActive(false);
        LastPanel = NowPanel;
        LogReadyPanel.SetActive(true);
        NowPanel = LogReadyPanel;
        //BottomBarPanel.SetActive(true);
    }

    //bottombar
    public void OpenRcmFurnPanel()
    {
        //cb_RcmFurnButton = RcmFurnButton.colors;
        ResetResult();
        LastPanel = NowPanel;
        BeforeBottomPanelChoose = NowPanel;
        Debug.Log("open rcm furn panel");
        NowPanel.SetActive(false);
        RcmFurnPanel.SetActive(true);
        NowPanel = RcmFurnPanel;
        StartCoroutine(ShowRecommendResult());
    }
    public void OpenSearchPanel()
    {
        ResetResult();
        LastPanel = NowPanel;
        BeforeBottomPanelChoose = NowPanel;
        Debug.Log("openOpenSearchPanel");
        NowPanel.SetActive(false);
        SearchPanel.SetActive(true);
        NowPanel = SearchPanel;
    }

    public GameObject WordSearchPanel;
    public void OpenWordSearchPanel()
    {
        ResetResult();
        LastPanel = NowPanel;
        //BeforeBottomPanelChoose = NowPanel;
        Debug.Log("OpenWordSearchPanel");
        NowPanel.SetActive(false);
        WordSearchPanel.SetActive(true);
        NowPanel = SearchPanel;
       // ShowWordSearchedFurn();
    }

    public void OpenMyPagePanel()
    {
        ResetResult();
        LastPanel = NowPanel;
        BeforeBottomPanelChoose = NowPanel;
        NowPanel.SetActive(false);
        Debug.Log("OpenMyPagePanel");
        MyPagePanel.SetActive(true);
        NowPanel = MyPagePanel;
    }

    public void OpenMyLikePanel()
    {
        ResetResult();
        LastPanel = NowPanel;
        NowPanel.SetActive(false);
        MyLikePanel.SetActive(true);
        Debug.Log("OpenMyLikePanel");
        NowPanel = MyLikePanel;
        StartCoroutine(ShowLikeResult());
    }
    public void OpenMySettingPanel()
    {
        LoadingImage.SetActive(true);
        LastPanel = NowPanel;
        NowPanel.SetActive(false);

        /*
        int mysettingindex = 0;
        for (int i = 0; i < UserDataList.Count; i++)
        {
            UserDataList[i].user_index = nowLogin_UserID;
            mysettingindex = i;
            break;
        }
        */

        Debug.Log(" OpenMySettingPanel  시작"           
           + UserDataList.user_name + "이름 "
           + UserDataList.user_age + " 살 "
           + UserDataList.user_email + " email "
           + UserDataList.user_sex + " 성별 "
           + UserDataList.user_housesize + " 집크기 "
           + UserDataList.user_roomtype + " 집형태 "
           + UserDataList.user_index + " 유저인덱스 "
           + UserDataList.user_likemood + " 유저무드 "
           );

        NowPanel = MySettingPanel;
        MySettingPanel.transform.FindChild("User_Name_InputField").GetComponent<InputField>().text = UserDataList.user_name;
        //transform.FindChild("Text").GetComponent<Text>().text = UserDataList.user_name;
        MySettingPanel.transform.FindChild("User_Age_InputField2").GetComponent<InputField>().text = UserDataList.user_age;
        //transform.FindChild("Text").GetComponent<Text>().text = UserDataList.user_age;
        MySettingPanel.transform.FindChild("User_HouseSIze_InputField2").GetComponent<InputField>().text = UserDataList.user_housesize;
        //transform.FindChild("Text").GetComponent<Text>().text = UserDataList.user_housesize;
        MySetting_Update_user_sex(UserDataList.user_sex);
        MySetting_Update_user_roomtype(UserDataList.user_roomtype);
        MySettingPanel.transform.FindChild("User_LikeMood_Furniture_Select").transform.FindChild("Text").GetComponent<Text>().text = UserDataList.user_likemood;
        Debug.Log(" OpenMySettingPanel  text 바꾸고 난 뒤 "
          + MySettingPanel.transform.FindChild("User_Name_InputField").GetComponent<InputField>().text + "이름 "
          + MySettingPanel.transform.FindChild("User_Age_InputField2").GetComponent<InputField>().text + " 살 "
          + UserDataList.user_email + " email "
          + UserDataList.user_sex + " 성별 "
          + MySettingPanel.transform.FindChild("User_HouseSIze_InputField2").GetComponent<InputField>().text + " 집크기 "
          + UserDataList.user_roomtype + " 집형태 "
          + UserDataList.user_index + " 유저인덱스 "
          + MySettingPanel.transform.FindChild("User_LikeMood_Furniture_Select").GetComponent<InputField>().text + " 유저무드 "
          );
        LoadingImage.SetActive(false);
        MySettingPanel.SetActive(true);
    }
    public void OpenAboutPanel()
    {
        LastPanel = NowPanel;
        NowPanel.SetActive(false);
        MyAboutPanel.SetActive(true);
        NowPanel = MyAboutPanel;
    }

    public void OpenSearchResultPanel()
    {
        LastPanel = NowPanel;
        NowPanel.SetActive(false);
        SearchResultPanel.SetActive(true);
        NowPanel = SearchResultPanel;
    }

    public void BottomBarQuit()
    {
        //BeforeBottomPanelChoose = NowPanel;
        NowPanel.SetActive(false);
        BeforeBottomPanelChoose.SetActive(true);
        NowPanel = BeforeBottomPanelChoose;
    }
    public void ResetResult()
    {

        //if (NowPanel == SearchResultPanel || NowPanel == MyLikePanel || NowPanel == RcmFurnPanel)
        //{
            /*
            ParentsOfResultFurnitureButton.gameObject.SetActive(false);
            ParentsOfmiddleCategoryButton.gameObject.SetActive(false);
            */
            if (ResultFurnList.Count > 0)
            {
                for (int i = 0; i < ResultFurnList.Count; i++)
                {
                    Destroy(ResultFurnList[i].gameObject);
                }
                ResultFurnList.Clear();
                ResultFurnList_Image.Clear();
                ResultFurnList_name.Clear();
                categorySearchIndex = 0;
            }

            if (FurnitureDataList.Count > 0)
            {
                FurnitureDataList.Clear();
                categorySearchIndex = 0;
            }

            LoadingImage.SetActive(false);
        //}
    }
    public void BottomBarBack()
    {
        //BeforeBottomPanelChoose = NowPanel;
        //ResetResult();    
        NowPanel.SetActive(false);
        LastPanel.SetActive(true);
        NowPanel = LastPanel;
        if (NowPanel == RcmFurnPanel)
        {
            NowPanel.transform.FindChild("BottomBarPanel").transform.FindChild("RcmFurnButton").GetComponent<Image>().sprite = Resources.Load("image/pressed_recommend", typeof(Sprite)) as Sprite;
            NowPanel.transform.FindChild("BottomBarPanel").transform.FindChild("LikePageButton").GetComponent<Image>().sprite = Resources.Load("image/하단바하트", typeof(Sprite)) as Sprite;
            NowPanel.transform.FindChild("BottomBarPanel").transform.FindChild("SearchButton").GetComponent<Image>().sprite = Resources.Load("image/하단바서치", typeof(Sprite)) as Sprite;
            LastPanel = RcmFurnPanel;
        }
        else if (NowPanel == MyLikePanel)
        {
            NowPanel.transform.FindChild("BottomBarPanel").transform.FindChild("RcmFurnButton").GetComponent<Image>().sprite = Resources.Load("image/하단바추천", typeof(Sprite)) as Sprite;
            NowPanel.transform.FindChild("BottomBarPanel").transform.FindChild("LikePageButton").GetComponent<Image>().sprite = Resources.Load("image/pressed_heart", typeof(Sprite)) as Sprite;
            NowPanel.transform.FindChild("BottomBarPanel").transform.FindChild("SearchButton").GetComponent<Image>().sprite = Resources.Load("image/하단바서치", typeof(Sprite)) as Sprite;
            LastPanel = MyLikePanel;
        }

        else if (NowPanel == SearchResultPanel)
        {
            NowPanel.transform.FindChild("BottomBarPanel").transform.FindChild("RcmFurnButton").GetComponent<Image>().sprite = Resources.Load("image/하단바추천", typeof(Sprite)) as Sprite;
            NowPanel.transform.FindChild("BottomBarPanel").transform.FindChild("LikePageButton").GetComponent<Image>().sprite = Resources.Load("image/하단바하트", typeof(Sprite)) as Sprite;
            NowPanel.transform.FindChild("BottomBarPanel").transform.FindChild("SearchButton").GetComponent<Image>().sprite = Resources.Load("image/pressed_search", typeof(Sprite)) as Sprite;
            LastPanel = SearchResultPanel;
        }

         
    }

    public void ResetUserData()
    {
        //mysetting 에서 취소했을시에 원래대로            
        user_name_temp = UserDataList.user_name;
        user_age_temp = UserDataList.user_age;
        user_housesize_temp = UserDataList.user_housesize;
        user_roomtype_temp = UserDataList.user_roomtype;
        user_sex_temp = UserDataList.user_sex;
        user_likemood_temp = UserDataList.user_likemood;
    }

    public void CancelMysetting()
    {
        ResetUserData();
        NowPanel.SetActive(false);
        LastPanel.SetActive(true);
        NowPanel = LastPanel;
    }


    public void OpenFurnitureInfoPanel(FurnitureData _fd)
    {
        if (LastPanel == RcmFurnPanel)
        {
            LastPanel.transform.FindChild("BottomBarPanel").transform.FindChild("RcmFurnButton").GetComponent<Image>().sprite = Resources.Load("image/pressed_recommend", typeof(Sprite)) as Sprite;
            LastPanel.transform.FindChild("BottomBarPanel").transform.FindChild("LikePageButton").GetComponent<Image>().sprite = Resources.Load("image/하단바하트", typeof(Sprite)) as Sprite;
            LastPanel.transform.FindChild("BottomBarPanel").transform.FindChild("SearchButton").GetComponent<Image>().sprite = Resources.Load("image/하단바서치", typeof(Sprite)) as Sprite;

        }
        else if (LastPanel == MyLikePanel)
        {
            LastPanel.transform.FindChild("BottomBarPanel").transform.FindChild("RcmFurnButton").GetComponent<Image>().sprite = Resources.Load("image/하단바추천", typeof(Sprite)) as Sprite;
            LastPanel.transform.FindChild("BottomBarPanel").transform.FindChild("LikePageButton").GetComponent<Image>().sprite = Resources.Load("image/pressed_heart", typeof(Sprite)) as Sprite;
            LastPanel.transform.FindChild("BottomBarPanel").transform.FindChild("SearchButton").GetComponent<Image>().sprite = Resources.Load("image/하단바서치", typeof(Sprite)) as Sprite;
        }

        else if (LastPanel == SearchResultPanel)
        {
            LastPanel.transform.FindChild("BottomBarPanel").transform.FindChild("RcmFurnButton").GetComponent<Image>().sprite = Resources.Load("image/하단바추천", typeof(Sprite)) as Sprite;
            LastPanel.transform.FindChild("BottomBarPanel").transform.FindChild("LikePageButton").GetComponent<Image>().sprite = Resources.Load("image/하단바하트", typeof(Sprite)) as Sprite;
            LastPanel.transform.FindChild("BottomBarPanel").transform.FindChild("SearchButton").GetComponent<Image>().sprite = Resources.Load("image/pressed_search", typeof(Sprite)) as Sprite;
        }

        FurnitureInfoPanel.transform.FindChild("FurnNameText").GetComponent<Text>().text = _fd.furnitureName;
        FurnitureInfoPanel.transform.FindChild("Scroll View").transform.FindChild("Viewport").transform.FindChild("Content").transform.FindChild("FurnInfoImage").GetComponent<Image>().sprite = LoadFurnitureImage(_fd.furnitureImageAddress);
       
        FurnitureInfoPanel.transform.FindChild("Scroll View").transform.FindChild("Viewport").transform.FindChild("Content").transform.FindChild("ARButton").GetComponent<Button>().onClick.AddListener(() => ChangeARScene());
        FurnitureInfoPanel.transform.FindChild("Scroll View").transform.FindChild("Viewport").transform.FindChild("Content").transform.FindChild("LinkButton").GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(_fd.furnitureLiked));


        FurnitureInfoPanel.transform.FindChild("Scroll View").transform.FindChild("Viewport").transform.FindChild("Content").transform.FindChild("BrandInfo").GetComponent<Text>().text = _fd.furnitureBrand;
        FurnitureInfoPanel.transform.FindChild("Scroll View").transform.FindChild("Viewport").transform.FindChild("Content").transform.FindChild("TypeInfo").GetComponent<Text>().text = _fd.type;
        FurnitureInfoPanel.transform.FindChild("Scroll View").transform.FindChild("Viewport").transform.FindChild("Content").transform.FindChild("MoodInfo").GetComponent<Text>().text = _fd.mood;
        for (int i = 0; i < _fd.texture.Count; i++)
        { 
            if(i!= _fd.texture.Count-1)
                FurnitureInfoPanel.transform.FindChild("Scroll View").transform.FindChild("Viewport").transform.FindChild("Content").transform.FindChild("TextureInfo").GetComponent<Text>().text += _fd.texture[i] + ", "; 
            else
                FurnitureInfoPanel.transform.FindChild("Scroll View").transform.FindChild("Viewport").transform.FindChild("Content").transform.FindChild("TextureInfo").GetComponent<Text>().text += _fd.texture[i];
        }
        //FurnitureInfoPanel.transform.FindChild("Scroll View").transform.FindChild("Viewport").transform.FindChild("ColorInfo").GetComponent<Text>().text = _fd.col;
        FurnitureInfoPanel.transform.FindChild("Scroll View").transform.FindChild("Viewport").transform.FindChild("Content").transform.FindChild("SizeInfo").GetComponent<Text>().text =_fd.furnitureWidth+" X " +_fd.furnitureHeight + " X " + _fd.furnitureDepth;


        //BeforeBottomPanelChoose = NowPanel;
        LastPanel = NowPanel;
        NowPanel.SetActive(false);
        FurnitureInfoPanel.SetActive(true);
        NowPanel = FurnitureInfoPanel;
    }
    void Awake()
    {

    }
    IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            //loginResult.text = " 구글 로그인 굿럭";

            Debug.Log("Success!");
        });
    }
    public void EditorOnLogin()
    {
        OpenLogInPanel();
        //OpenUserInfoPanel();
        //OpenUserInfoPanel1();
    }

    public void OnLogin()
    {
        LoadingImage.SetActive(true);

#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log("OnLogin()  kdy Success : " + Social.localUser.userName);
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().RequestIdToken()
            .RequestEmail()
            .Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool bSuccess) =>
            {
                if (bSuccess)
                {
                    StartCoroutine(TryFirebaseLogin());
                    Debug.Log("kdy Success : " + Social.localUser.userName);

                    OpenUserInfoPanel();
                    OpenUserInfoPanel1();
                }
                else
                {
                    Debug.Log("unity android authenticate Onlogin Fail!!!!!!!!!!!!!!!!!!1");
                }
            });
        }
        
#else
        OpenUserInfoPanel();
        OpenUserInfoPanel1();
#endif
        /*
        //구글로그인이라는데 goolgeidtoken을못차즌데
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        Firebase.Auth.Credential credential =
            Firebase.Auth.GoogleAuthProvider.GetCredential
            (googleIdToken, googleAccessToken);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
        */
    }
    public void OnLogOut()
    {
        // ((PlayGamesPlatform)Social.Active).SignOut();
        mDatabaseRef.Child("login").Child("login_1").Child("deviceID").SetValueAsync("");
        mDatabaseRef.Child("login").Child("login_1").Child("userID").SetValueAsync("");
        auth.SignOut();
        LogChange(false);
    }

    FirebaseUser user;
    string user_photourl_temp;
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                user_name_temp = user.DisplayName ?? "";
                user_email_temp = user.Email ?? "";
                // user_photourl_temp = user.PhotoUrl ?? "";
            }
        }
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

        /*
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        */
    }
    /*
    private void writeNewUser(string userId, string name)
    {
        User user = new User(name);
        string json = JsonUtility.ToJson(user);
        mDatabaseRef.Child("user").Child(userId).SetRawJsonValueAsync(json);
    }
    */

    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            WarnNetworkPanel1.SetActive(true);
        }
        else
        {
            WarnNetworkPanel1.SetActive(false);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            writeNewUser("USERID1234", "unitytest");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            FirebaseDatabase.DefaultInstance.GetReference("user").GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Debug.Log("failed");
                }
                else if (task.IsCompleted)
                {
                    Firebase.Database.DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot.Value.ToString());
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        Debug.Log("users name : " +
                            childSnapshot.Child("username").Value.ToString());
                    }
                }
            });
        }*/
    }
    public void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // Assign texture to a temporary quad and destroy it after 5 seconds
                //    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //    quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                //   quad.transform.forward = Camera.main.transform.forward;
                //     quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                //    Material material = quad.GetComponent<Renderer>().material;
                // if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                //     material.shader = Shader.Find("Legacy Shaders/Diffuse");

                //  material.mainTexture = texture;
               
                OpenAskSelectMood(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)), "Modern");
                
                // Destroy(quad, 5f);

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                // Destroy(texture, 5f);
            }
        }, "Select a PNG image", "image/png");//, maxSize);

        Debug.Log("Permission result: " + permission);
    }

    private void PickVideo()
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            Debug.Log("Video path: " + path);
            if (path != null)
            {
                // Play the selected video
                Handheld.PlayFullScreenMovie("file://" + path);
            }
        }, "Select a video");

        Debug.Log("Permission result: " + permission);
    }
    //  1;
    private void writeNewUser_Test2(string _username, string _useremail, string _userage, string _usersex,
        string _userhousesize, string _userroomtype, string _userindex, string _userlikemood)
    {
        //User user = new User("user_1");
        string json = JsonUtility.ToJson(user);
        mDatabaseRef.Child("user").Child("user_1").Child("user_age").SetValueAsync(_userage);
        mDatabaseRef.Child("user").Child("user_1").Child("user_email").SetValueAsync(_useremail);
        mDatabaseRef.Child("user").Child("user_1").Child("user_housesize").SetValueAsync(_userhousesize);

        // mDatabaseRef.Child("user").Child("user_1").Child("user_index").SetValueAsync(_userindex);

        mDatabaseRef.Child("user").Child("user_1").Child("user_likemood").SetValueAsync(_userlikemood);

        mDatabaseRef.Child("user").Child("user_1").Child("user_name").SetValueAsync(_username);
        mDatabaseRef.Child("user").Child("user_1").Child("user_roomtype").SetValueAsync(_userroomtype);
        mDatabaseRef.Child("user").Child("user_1").Child("user_sex").SetValueAsync(_usersex);
    }

    /*
    string[,] categoryName = new string[,]{ 
        { "furniture_brand", "Classic", "Mordern", "Natural", "Nordic", "Romantic", "Vintage", "Casual" },
        {  "mood", "space", "subtype", "type" },
        {"space", "subtype", "type" },
        {  "subtype", "type" },
        {  "type" }
    };
    */

    public string[] categoryName = new string[6] { "furniture_brand", "mood", "space", "subtype", "type", "texture" };


    public string[] bramdName = new string[4] { "Alias Design", "INART", "Monogram Appliances Collection", "Opendesk" };
    public string[] moodName = new string[7] { "Classic", "Modern", "Natural", "Nordic", "Romantic", "Vintage", "Casual" };
    public string[] spaceName = new string[4] { "bedroom", "living room", "kitchen", "anywhere" };
    public string[] textureName = new string[10] { "wood", "glass", "steel", "plastic", "ceramic", "stone", "leather",
                           "fabric","aluminium","polyurethane"};

    public string[] typeName = new string[5] { "bed", "seat", "table", "storage", "etc" };

    public string[] seat_subtypeName = new string[5] { "chair", "sofa", "stools", "office chair", "pouffes" };
    public string[] table_subtypeName = new string[4] { "dinning table", "coffee table", "desk", "side" };
    public string[] storage_subtypeName = new string[4] { "tv stands", "bookshelf", "shelves", "drawers" };
    public string[] etc_subtypeName = new string[4] { "kitchen appliances", "divide", "mirror", "etc" };

    public string[] temp_category;

    public string TopCategory, middleCategory, bottomCategory;
    public GameObject ParentsOfmiddleCategoryButton;
    // public UnityEngine.UI.LayoutGroup ParentsOfmiddleCategoryButton;
    //public UnityEngine.UI.HorizontalOrVerticalLayoutGroup ParentsOfmiddleCategoryButton;
    public Button[] middleCategoryButton;
    public GameObject ParentsOfResultFurnitureButton, ParentsOf_Search_ResultFurnitureButton, ParentsOf_Rcm_ResultFurnitureButton, ParentsOf_Like_ResultFurnitureButton;
    //public Button[] ResultFurnitureButton, Result_Search_FurnitureButton, Result_Rcm_FurnitureButton, Result_Like_FurnitureButton;
    public int categorySearchIndex = 0, middleCategoryIndex = 0;

    public Button MiddleCategoryButtonPrefab, resultButtonPrefab, temp_resultButtonPrefab, result_Search_ButtonPrefab, result_Rcm_ButtonPrefab, result_Like_ButtonPrefab;
    new Text[] middleCategoryText;
    public void ShowMiddleCategory(string _selectdCategory)
    {
        ParentsOfmiddleCategoryButton.gameObject.SetActive(true);
        SearchResultTitleText.text = _selectdCategory;
        if (middleCategoryIndex > 0)
        {
            middleCategoryIndex = 0;
            for (int i = 0; i < middleCategoryButton.Length; i++)
            {
                Debug.Log(i + "middlecategory 초기화 ㅍ괴 " + middleCategoryButton.Length);
                Destroy(middleCategoryButton[i].gameObject);
                middleCategoryButton[i] = null;
                // temp_category[i] = null;
                temp_category = new string[0];
            }
        }

        TopCategory = _selectdCategory;
        Debug.Log("select category" + _selectdCategory + TopCategory);
        switch (_selectdCategory)
        {
            case "furniture_brand":
                temp_category = bramdName;//(string[])bramdName.Clone();
                break;
            case "mood":
                temp_category = moodName;// (string[])moodName.Clone();
                Debug.Log("mood category" + moodName[0]);// +"\n" +temp_category[0]);
                Debug.Log("mood category" + temp_category[0]);
                break;
            case "space":
                temp_category = spaceName;
                break;
            case "type":
                temp_category = typeName;
                break;
            case "texture":
                temp_category = textureName;
                break;
            case "seat":
                temp_category = seat_subtypeName;
                break;
            case "table":
                temp_category = table_subtypeName;
                break;
            case "storage":
                temp_category = storage_subtypeName;
                break;
            case "etc":
                temp_category = etc_subtypeName;
                break;
        }

        middleCategoryButton = new Button[temp_category.Length];
        middleCategoryText = new Text[temp_category.Length];
        Debug.Log("mood category" + moodName[0] + "\n" + temp_category[0]);
        //대분류 버튼은 다 나와야한다 여기서왜오류가날까

        while (categorySearchIndex < temp_category.Length)
        {
            int temp;
            middleCategoryButton[categorySearchIndex] = Instantiate(MiddleCategoryButtonPrefab) as Button;//Resources.Load(Assets/"Prefab/Button/MiddleCategoryButton"
            middleCategoryButton[categorySearchIndex].transform.SetParent(ParentsOfmiddleCategoryButton.transform);
            middleCategoryButton[categorySearchIndex].name = temp_category[categorySearchIndex];
            middleCategoryButton[categorySearchIndex].transform.FindChild("Text").GetComponent<Text>().text = temp_category[categorySearchIndex];
            middleCategoryButton[categorySearchIndex].gameObject.SetActive(true);
            //GameObject.Find("MiddleCategoryText").GetComponent<Text>().text= temp_category[categorySearchIndex]; //;    .GetComponent<Text>().text = middleCategoryButton[categorySearchIndex].transform.
            temp = categorySearchIndex;
            middleCategoryButton[categorySearchIndex].onClick.AddListener(() => ShowResultFurn(temp_category[temp]));//delegate () { ShowResultFurn(temp_category[categorySearchIndex]); }
            categorySearchIndex++;
        }
        middleCategoryIndex = categorySearchIndex;
        categorySearchIndex = 0;
    }

    public List<Button> ResultFurnList = new List<Button>();
    public List<string> ResultFurnList_Image = new List<string>();
    public List<string> ResultFurnList_name = new List<string>();
    public List<string> ResultFurnList_Like = new List<string>();
    public GameObject MoreFurnButton, temp_MoreFurnButton, SearchMoreFurnButton, RcmMoreFurnButton, LikeMoreFurnButton;
    public Text SearchResultTitleText;

    public void ShowResultFurn(string _searchResultName)
    {
        LoadingImage.SetActive(true);
        for (int i=0; i < temp_category.Length; i++)
        {
            if (middleCategoryButton[i].name == _searchResultName)
            {
                middleCategoryButton[i].transform.FindChild("Text").GetComponent<Text>().text = "<color=#775EEE>" + _searchResultName + "</color>"   ;
            }
            else
            {
                middleCategoryButton[i].transform.FindChild("Text").GetComponent<Text>().text =  temp_category[i];
            }

        }
        //List 초기화
        ResetResult();

        Debug.Log(_searchResultName + " show result 시작");

        FirebaseDatabase.DefaultInstance.GetReference("furniture").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("failed");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());
                foreach (var childSnapshot in snapshot.Children)
                {
                    // Debug.Log(" foreach 들어왓다  " + childSnapshot.Child(TopCategory).Value.ToString());
                    if (childSnapshot.Child(TopCategory).Value.ToString() == _searchResultName)
                    {
                        Debug.Log(" foreach  if문 들어왓다  " + childSnapshot.Child(TopCategory).Value.ToString());

                        FurnitureDataList.Add(new FurnitureData(
                            childSnapshot.Child("furniture_brand").Value.ToString(),
                            childSnapshot.Child("furniture_clicked").Value.ToString(),
                            childSnapshot.Child("furniture_depth").Value.ToString(),
                            childSnapshot.Child("furniture_height").Value.ToString(),
                            childSnapshot.Child("furniture_liked").Value.ToString(),
                            childSnapshot.Child("furniture_name").Value.ToString(),
                            childSnapshot.Child("furniture_width").Value.ToString(),
                             childSnapshot.Child("image").Value.ToString(),
                            childSnapshot.Key.ToString(),
                            childSnapshot.Child("mood").Value.ToString(),
                            childSnapshot.Child("space").Value.ToString(),
                            childSnapshot.Child("subtype").Value.ToString(),
                            childSnapshot.Child("type").Value.ToString()
                           ));


                        ResultFurnList_Image.Add(childSnapshot.Child("image").Value.ToString());
                        Debug.Log(childSnapshot.Child("image").Value.ToString() + childSnapshot.Child("furniture_name").Value.ToString());
                        ResultFurnList_name.Add(childSnapshot.Child("furniture_name").Value.ToString());
                    }
                }
            }
            PushMoreFurnButton();
            LoadingImage.SetActive(false);
        });
    }
    const long maxAllowedSize = 1 * 1024 * 1024;
    public void PushMoreFurnButton()
    {
        Debug.Log("push more furn statrt!");
        LoadingImage.SetActive(true);
        if (NowPanel = SearchResultPanel)
        {
            Debug.Log("NowPanel = SearchResultPanel)");
            SearchMoreFurnButton.SetActive(false);
            // temp_MoreFurnButton.transform.SetParent(SearchResultPanel.transform);
        }//ResultFurnList[i].transform.SetParent(ParentsOf_Search_ResultFurnitureButton.transform);
        else if (NowPanel = RcmFurnPanel)
        {
            Debug.Log("NowPanel = RcmFurnPanel)");
            RcmMoreFurnButton.SetActive(false);
            //  temp_MoreFurnButton.transform.SetParent(RcmFurnPanel.transform);

        } //  ResultFurnList[i].transform.SetParent(ParentsOf_Rcm_ResultFurnitureButton.transform);
        else if (NowPanel = MyLikePanel)
        {
            Debug.Log("  else if (NowPanel = MyLikePanel)");
            LikeMoreFurnButton.SetActive(false);
            //  temp_MoreFurnButton.transform.SetParent(MyLikePanel.transform);
        }
        else if (NowPanel = WordSearchPanel)
        {
            Debug.Log("  else if (NowPanel = WordSearchPanel)");
            WordSearchPanel.transform.FindChild("MoreFurnButton").gameObject.SetActive(false);
            //  temp_MoreFurnButton.transform.SetParent(MyLikePanel.transform);
        }
        else if (NowPanel = RcmFurnPanel)
        {
            Debug.Log("  else if (NowPanel = RcmFurnPanel");
            RcmFurnPanel.transform.FindChild("RcmMoreFurnButton").gameObject.SetActive(false);
        }

        // temp_MoreFurnButton = Instantiate(MoreFurnButton);        
        //WordSearchPanel.transform.FindChild("ResultScroll View").transform.FindChild("Viewport").transform.FindChild("resultCategoryContent").SetActive(false);

        if (categorySearchIndex + 6 < FurnitureDataList.Count)//ResultFurnList_name
        {
            Debug.Log("  if (categorySearchIndex + 6 < FurnitureDataList.Count)");
            //MoreFurnButton.SetActive(true);
            for (int i = categorySearchIndex; i < categorySearchIndex + 6; i++)
            {
                Debug.Log(" pushmorefurn 카테고리남은게 6개이상 for문 artCoroutine ResultInstantiate");
                StartCoroutine(ResultInstantiate(i));
            }
            categorySearchIndex += 6;
            if (NowPanel = SearchResultPanel)
            {
                Debug.Log("pushmorefurn  카테고리남은게 6개이상 SearchResultPanel");
                SearchMoreFurnButton.SetActive(true);
                //ParentsOfResultFurnitureButton = ParentsOf_Search_ResultFurnitureButton;
                //resultButtonPrefab = result_Search_ButtonPrefab;
                // temp_MoreFurnButton.transform.SetParent(SearchResultPanel.transform);
            }//ResultFurnList[i].transform.SetParent(ParentsOf_Search_ResultFurnitureButton.transform);
            else if (NowPanel = RcmFurnPanel)
            {
                Debug.Log(" pushmorefurn  카테고리남은게 6개이상 RcmFurnPanel ");
                RcmMoreFurnButton.SetActive(true);
                //ParentsOfResultFurnitureButton = ParentsOf_Rcm_ResultFurnitureButton;
                //resultButtonPrefab = result_Rcm_ButtonPrefab;
                //  temp_MoreFurnButton.transform.SetParent(RcmFurnPanel.transform);

            } //  ResultFurnList[i].transform.SetParent(ParentsOf_Rcm_ResultFurnitureButton.transform);
            else if (NowPanel = MyLikePanel)
            {
                Debug.Log("pushmorefurn  카테고리남은게 6개이상 LikeMoreFurnButton");
                LikeMoreFurnButton.SetActive(true);
                //ParentsOfResultFurnitureButton = ParentsOf_Like_ResultFurnitureButton;
                //resultButtonPrefab = result_Like_ButtonPrefab; //  temp_MoreFurnButton.transform.SetParent(MyLikePanel.transform);
            }
            else if (NowPanel = WordSearchPanel)
            {
                Debug.Log("pushmorefurn  카테고리남은게 6개이상  else if (NowPanel = WordSearchPanel)");
                WordSearchPanel.transform.FindChild("MoreFurnButton").gameObject.SetActive(true);
                //  temp_MoreFurnButton.transform.SetParent(MyLikePanel.transform);
            }           
        }
        else
        {
            //Destroy(temp_MoreFurnButton);          
            for (int i = categorySearchIndex; i < FurnitureDataList.Count; i++)// ResultFurnList_name
            {
                Debug.Log(i+"카테고리남은게 6개 미만");
                StartCoroutine(ResultInstantiate(i));
            }
            categorySearchIndex = FurnitureDataList.Count;//ResultFurnList_name
        }
        //ParentsOfResultFurnitureButton.gameObject.SetActive(true);
        LoadingImage.SetActive(false);
    }
    Texture2D texture;
    private void SystemIOFileLoad()
    {
        byte[] byteTexture = System.IO.File.ReadAllBytes("Assets/2D/furniture");
        if (byteTexture.Length > 0)
        {
            texture = new Texture2D(0, 0);
            texture.LoadImage(byteTexture);
        }
    }
    IEnumerator ResultInstantiate(int i)
    {
        /*
        Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        byte[] temp;
        Firebase.Storage.StorageReference reference = storage.GetReferenceFromUrl(ResultFurnList_Image[i]);
         Debug.Log("IEnumerator ResultInstantiate  Storage" + i);

        reference.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("bundle down fail");
                return;
            }
            else
            {
                temp = task.Result;
                texture = new Texture2D(2, 2);
                texture.LoadImage(temp);

                Debug.Log("bundle down done");
                //File.WriteAllBytes("Assets" + "/2D/furniture", temp);
                //hasBundleLoaded = true;
            }
            //isBundleLoading = false;
        });
        */
        Debug.Log("start 버튼 만들기");
        if (NowPanel = SearchResultPanel)
        {
            Debug.Log("start 버튼 만들기SearchResultPanel");
            ResultFurnList.Add(Instantiate(result_Search_ButtonPrefab));
            ResultFurnList[i].transform.SetParent(ParentsOf_Search_ResultFurnitureButton.transform);
            // temp_MoreFurnButton.transform.SetParent(SearchResultPanel.transform);
        }//ResultFurnList[i].transform.SetParent(ParentsOf_Search_ResultFurnitureButton.transform);
        else if (NowPanel = RcmFurnPanel)
        {
            Debug.Log("start 버튼 만들기RcmFurnPanel");
            ResultFurnList.Add(Instantiate(result_Rcm_ButtonPrefab));
            ResultFurnList[i].transform.SetParent(ParentsOf_Rcm_ResultFurnitureButton.transform);

        } //  ResultFurnList[i].transform.SetParent(ParentsOf_Rcm_ResultFurnitureButton.transform);
        else if (NowPanel = MyLikePanel)
        {
            Debug.Log("start 버튼 만들기 MyLikePanel");
            ResultFurnList.Add(Instantiate(result_Like_ButtonPrefab));
            ResultFurnList[i].transform.SetParent(ParentsOf_Like_ResultFurnitureButton.transform);
        }
        else if (NowPanel = WordSearchPanel)
        {
            Debug.Log("start 버튼 만들기 WordSearchPanel");
            ResultFurnList.Add(Instantiate(WordSearchPanel.transform.FindChild("ResultScroll View").transform.FindChild("Viewport").transform.FindChild("Rcm_resultCategoryContent").transform.FindChild("nolike_Rcm_ResultFurnButton_like_no").GetComponent<Button>()));
            ResultFurnList[i].transform.SetParent(WordSearchPanel.transform.FindChild("ResultScroll View").transform.FindChild("Viewport").transform.FindChild("Rcm_resultCategoryContent").transform);
        }   

        //[i].transform.SetParent(ParentsOf_Like_ResultFurnitureButton.transform);
        /*
        string imageAddress = FurnitureDataList[i].furnitureImageAddress;// (파베에서 받은 이미지 주소); //이미지 주소ResultFurnList_Image
        string[] divide = imageAddress.Split('/');
        int divideNum = divide.Length;
        string imageName = divide[divideNum - 1]; //이미지 이름
        */
        yield return new WaitForSeconds(5);

        Debug.Log("ResultInstantiate yield WaitForSeconds 뒤" + i);
        ResultFurnList[i].name = FurnitureDataList[i].furnitureName; //ResultFurnList_name
        ResultFurnList[i].gameObject.SetActive(true);

        ResultFurnList[i].transform.FindChild("Text (1)").GetComponent<Text>().text
        = FurnitureDataList[i].furnitureName; //ResultFurnList_name[i]
        Debug.Log(FurnitureDataList[i].furnitureName.Replace(" ", "_"));
        Debug.Log(FurnitureDataList[i].furnitureName);// ResultFurnList_name[i]
        Rect rect = new Rect(0, 0, 232, 271);
        ResultFurnList[i].transform.FindChild("Image").GetComponent<Image>().sprite = LoadFurnitureImage(FurnitureDataList[i].furnitureImageAddress);// Resources.Load("furnImage/" + imageName, typeof(Sprite)) as Sprite;//     Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
        yield return new WaitForSeconds(5);
        ResultFurnList[i].onClick.AddListener(() => OpenFurnitureInfoPanel(FurnitureDataList[i]));//delegate () { ShowResultFurn(temp_category[categorySearchIndex]); }

        //ResultFurnList[i].gameObject.SetActive(false);
        ResultFurnList[i].gameObject.SetActive(true);

        /*
        ResultFurnList.Add(Instantiate(resultButtonPrefab));

        Debug.Log("child snapshot 부모설정");
        ResultFurnList[categorySearchIndex].transform.SetParent(ParentsOfResultFurnitureButton.transform);

        ResultFurnList[categorySearchIndex].name = _temp;
        Debug.Log("child snapshot prefab부름?");
        ResultFurnList[categorySearchIndex].transform.FindChild("nolike_result_Text").GetComponent<Text>().text
        = _temp;
        */

    }
    /*
    void InitializeSDK()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(continuationAction: task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
    }
    */
    /*
    public Scrollbar ScrollbarVertical_Search_Result;
    public void ScrollMoreFurn()
    {
        Debug.Log(ScrollbarVertical_Search_Result.value*100.00f);

        if (ScrollbarVertical_Search_Result.transform.GetComponent<Scrollbar>().value<= 0.05f)
        {            
            //PushMoreFurnButton();
        }    
    }
    */

    public void ShowWordSearchedFurn()
    {        
        string _searchString = SearchPanel.transform.FindChild("InputField").GetComponent<InputField>().text;
        Debug.Log(_searchString + "show ShowWordSearchedFurn 시작");
        WordSearchPanel.transform.FindChild("InputField").GetComponent<InputField>().text = _searchString;
        WordSearchPanel.transform.FindChild("TopCategoryText").GetComponent<Text>().text = "Search";
        OpenWordSearchPanel();
        //LoadingImage.SetActive(true);
        //List 초기화
        //ResetResult();
        Debug.Log(_searchString + "show ShowWordSearchedFurn 시작");

        FirebaseDatabase.DefaultInstance.GetReference("furniture").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("failed");
            }
            else if (task.IsCompleted)
            {
                int count = 0;

                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());
                foreach (var childSnapshot in snapshot.Children)
                {
                    // Debug.Log(" foreach 들어왓다  " + childSnapshot.Child(TopCategory).Value.ToString());
                    if (childSnapshot.Child("furniture_name").Value.ToString().ToUpper(new CultureInfo("en-US", false)).StartsWith(_searchString.ToUpper(new CultureInfo("en-US", false))))
                    {
                        Debug.Log("ShowWordSearchedFurn  foreach  if문 들어왓다  " + childSnapshot.Child("furniture_name").Value.ToString());
                        FurnitureDataList.Add(new FurnitureData(
                            childSnapshot.Child("furniture_brand").Value.ToString(),
                            childSnapshot.Child("furniture_clicked").Value.ToString(),
                            childSnapshot.Child("furniture_depth").Value.ToString(),
                            childSnapshot.Child("furniture_height").Value.ToString(),
                            childSnapshot.Child("furniture_liked").Value.ToString(),
                            childSnapshot.Child("furniture_name").Value.ToString(),
                            childSnapshot.Child("furniture_width").Value.ToString(),
                             childSnapshot.Child("image").Value.ToString(),
                            childSnapshot.Key.ToString(),
                            childSnapshot.Child("mood").Value.ToString(),
                            childSnapshot.Child("space").Value.ToString(),
                            childSnapshot.Child("subtype").Value.ToString(),
                            childSnapshot.Child("type").Value.ToString()
                           ));
                        Debug.Log(" foreach  if문 들어왓다  " + childSnapshot.Child("furniture_name").Value.ToString());
                        //ResultFurnList_Image.Add(childSnapshot.Child("image").Value.ToString());
                        Debug.Log(childSnapshot.Child("image").Value.ToString() + childSnapshot.Child("furniture_name").Value.ToString());
                        //ResultFurnList_name.Add(childSnapshot.Child("furniture_name").Value.ToString());
                        count++;
                    }
                }

                if (count < 6)
                {
                    FirebaseDatabase.DefaultInstance.GetReference("furniture").GetValueAsync().ContinueWith(task2 => {
                        if (task2.IsFaulted)
                        {
                            Debug.Log("failed");
                        }
                        else if (task2.IsCompleted)
                        {
                            DataSnapshot snapshot2 = task2.Result;
                            //Debug.Log(snapshot.Value.ToString());
                            foreach (var childSnapshot in snapshot2.Children)
                            {
                                // Debug.Log(" foreach 들어왓다  " + childSnapshot.Child(TopCategory).Value.ToString());
                                if (childSnapshot.Child("furniture_name").Value.ToString().ToUpper(new CultureInfo("en-US", false)).Contains(_searchString.ToUpper(new CultureInfo("en-US", false))))
                                {
                                    Debug.Log(" foreach  if문 들어왓다  " + childSnapshot.Child("furniture_name").Value.ToString());
                                    //ResultFurnList_Image.Add(childSnapshot.Child("image").Value.ToString());

                                    FurnitureDataList.Add(new FurnitureData(
                                    childSnapshot.Child("furniture_brand").Value.ToString(),
                                    childSnapshot.Child("furniture_clicked").Value.ToString(),
                                    childSnapshot.Child("furniture_depth").Value.ToString(),
                                    childSnapshot.Child("furniture_height").Value.ToString(),
                                    childSnapshot.Child("furniture_liked").Value.ToString(),
                                    childSnapshot.Child("furniture_name").Value.ToString(),
                                    childSnapshot.Child("furniture_width").Value.ToString(),
                                     childSnapshot.Child("image").Value.ToString(),
                                    childSnapshot.Key.ToString(),
                                    childSnapshot.Child("mood").Value.ToString(),
                                    childSnapshot.Child("space").Value.ToString(),
                                    childSnapshot.Child("subtype").Value.ToString(),
                                    childSnapshot.Child("type").Value.ToString()
                                   ));

                                    Debug.Log(childSnapshot.Child("image").Value.ToString() + childSnapshot.Child("furniture_name").Value.ToString());
                                    //ResultFurnList_name.Add(childSnapshot.Child("furniture_name").Value.ToString());
                                }
                            }

                        }
                    });
                }
            }
            PushMoreFurnButton();
            LoadingImage.SetActive(false);
        });
    }
    public void ShowSearchedFurn()
    {
        string _searchString = SearchPanel.transform.FindChild("InputField").GetComponent<InputField>().text;
        OpenSearchResultPanel();
        //LoadingImage.SetActive(true);
        //List 초기화
        //ResetResult();
        Debug.Log(_searchString + "show result 시작");

        FirebaseDatabase.DefaultInstance.GetReference("furniture").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("failed");
            }
            else if (task.IsCompleted)
            {
                int count = 0;

                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());
                foreach (var childSnapshot in snapshot.Children)
                {
                    // Debug.Log(" foreach 들어왓다  " + childSnapshot.Child(TopCategory).Value.ToString());
                    if (childSnapshot.Child("furniture_name").Value.ToString().ToUpper(new CultureInfo("en-US", false)).StartsWith(_searchString.ToUpper(new CultureInfo("en-US", false))))
                    {
                        Debug.Log(" foreach  if문 들어왓다  " + childSnapshot.Child("furniture_name").Value.ToString());
                        FurnitureDataList.Add(new FurnitureData(
                            childSnapshot.Child("furniture_brand").Value.ToString(),
                            childSnapshot.Child("furniture_clicked").Value.ToString(),
                            childSnapshot.Child("furniture_depth").Value.ToString(),
                            childSnapshot.Child("furniture_height").Value.ToString(),
                            childSnapshot.Child("furniture_liked").Value.ToString(),
                            childSnapshot.Child("furniture_name").Value.ToString(),
                            childSnapshot.Child("furniture_width").Value.ToString(),
                             childSnapshot.Child("image").Value.ToString(),
                            childSnapshot.Key.ToString(),
                            childSnapshot.Child("mood").Value.ToString(),
                            childSnapshot.Child("space").Value.ToString(),
                            childSnapshot.Child("subtype").Value.ToString(),
                            childSnapshot.Child("type").Value.ToString()
                           ));
                        Debug.Log(" foreach  if문 들어왓다  " + childSnapshot.Child("furniture_name").Value.ToString());
                        //ResultFurnList_Image.Add(childSnapshot.Child("image").Value.ToString());
                        Debug.Log(childSnapshot.Child("image").Value.ToString() + childSnapshot.Child("furniture_name").Value.ToString());
                        //ResultFurnList_name.Add(childSnapshot.Child("furniture_name").Value.ToString());
                        count++;
                    }
                }

                if (count < 6)
                {
                    FirebaseDatabase.DefaultInstance.GetReference("furniture").GetValueAsync().ContinueWith(task2 => {
                        if (task2.IsFaulted)
                        {
                            Debug.Log("failed");
                        }
                        else if (task2.IsCompleted)
                        {
                            DataSnapshot snapshot2 = task2.Result;
                            //Debug.Log(snapshot.Value.ToString());
                            foreach (var childSnapshot in snapshot2.Children)
                            {
                                // Debug.Log(" foreach 들어왓다  " + childSnapshot.Child(TopCategory).Value.ToString());
                                if (childSnapshot.Child("furniture_name").Value.ToString().ToUpper(new CultureInfo("en-US", false)).Contains(_searchString.ToUpper(new CultureInfo("en-US", false))))
                                {
                                    Debug.Log(" foreach  if문 들어왓다  " + childSnapshot.Child("furniture_name").Value.ToString());
                                    //ResultFurnList_Image.Add(childSnapshot.Child("image").Value.ToString());

                                    FurnitureDataList.Add(new FurnitureData(
                                    childSnapshot.Child("furniture_brand").Value.ToString(),
                                    childSnapshot.Child("furniture_clicked").Value.ToString(),
                                    childSnapshot.Child("furniture_depth").Value.ToString(),
                                    childSnapshot.Child("furniture_height").Value.ToString(),
                                    childSnapshot.Child("furniture_liked").Value.ToString(),
                                    childSnapshot.Child("furniture_name").Value.ToString(),
                                    childSnapshot.Child("furniture_width").Value.ToString(),
                                     childSnapshot.Child("image").Value.ToString(),
                                    childSnapshot.Key.ToString(),
                                    childSnapshot.Child("mood").Value.ToString(),
                                    childSnapshot.Child("space").Value.ToString(),
                                    childSnapshot.Child("subtype").Value.ToString(),
                                    childSnapshot.Child("type").Value.ToString()
                                   ));

                                    Debug.Log(childSnapshot.Child("image").Value.ToString() + childSnapshot.Child("furniture_name").Value.ToString());
                                    //ResultFurnList_name.Add(childSnapshot.Child("furniture_name").Value.ToString());
                                }
                            }

                        }
                    });
                }
            }
            PushMoreFurnButton();
            LoadingImage.SetActive(false);
        });
    }
    public Sprite LoadFurnitureImage(string _image)//_image는 이미지주소
    {
        string[] divide = _image.Split('/');
        int divideNum = divide.Length;
        string imageName = divide[divideNum - 1]; //이미지 이름

        string path = Application.persistentDataPath + "/furnImage/" + imageName;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }

}
