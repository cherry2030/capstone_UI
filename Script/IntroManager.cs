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

public class User
{
    public string user_name;
    public string user_email;
    public int user_age;
    public int user_sex;
    public int user_housesize;
    public int user_roomtype;
    public int user_index;
    public string user_likemood;

    public User()
    {
    }

    public User(string _userEmail)
    {
        this.user_email = _userEmail;
    }
    public User(int _userIndex)
    {
        this.user_index = _userIndex;
    }
    public User(string _username, string _useremail, int _userage, int _usersex,
        int _userhousesize, int _userroomtype, int _userindex, string _userlikemood)
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
        cb_manButton = manButton.colors;
        cb_womanButton = womanButton.colors;

        user_sex_temp = _sex;
        if (_sex == "0")
        {
            cb_manButton.normalColor = SelectButtonColor;
            cb_womanButton.normalColor = DeselectButtonColor;
        }
        else if (_sex == "1")
        {
            cb_manButton.normalColor = DeselectButtonColor;
            cb_womanButton.normalColor = SelectButtonColor;
        }
        manButton.colors = cb_manButton;
        womanButton.colors = cb_womanButton;
    }

    public Button OneRoomButton, TwoRoomButton, ETCRoomButton;
    public ColorBlock cb_OneRoomButton, cb_TwoRoomButton, cb_ETCRoomButton;
    public string user_roomtype_temp;

    public void Update_user_roomtype(string _room)
    {
        user_roomtype_temp = _room;
        cb_OneRoomButton = OneRoomButton.colors;
        cb_TwoRoomButton = TwoRoomButton.colors;
        cb_ETCRoomButton = ETCRoomButton.colors;

        if (_room == "0")
        {
            cb_OneRoomButton.normalColor = SelectButtonColor;
            cb_TwoRoomButton.normalColor = DeselectButtonColor;
            cb_ETCRoomButton.normalColor = DeselectButtonColor;
        }
        else if (_room == "1")
        {
            cb_OneRoomButton.normalColor = DeselectButtonColor;
            cb_TwoRoomButton.normalColor = SelectButtonColor;
            cb_ETCRoomButton.normalColor = DeselectButtonColor;
        }

        else if (_room == "2")
        {
            cb_OneRoomButton.normalColor = DeselectButtonColor;
            cb_TwoRoomButton.normalColor = DeselectButtonColor;
            cb_ETCRoomButton.normalColor = SelectButtonColor;
        }

        OneRoomButton.colors = cb_OneRoomButton;
        TwoRoomButton.colors = cb_TwoRoomButton;
        ETCRoomButton.colors = cb_ETCRoomButton;

    }

    public Button MySetting_manButton, MySetting_womanButton;
    //public ColorBlock MySetting_cb_manButton, MySetting_cb_womanButton;


    public void MySetting_Update_user_sex(string _sex)
    {
        cb_manButton = MySetting_manButton.colors;
        cb_womanButton = MySetting_womanButton.colors;

        user_sex_temp = _sex;
        if (_sex == "0")
        {
            cb_manButton.normalColor = SelectButtonColor;
            cb_womanButton.normalColor = DeselectButtonColor;
        }
        else if (_sex == "1")
        {
            cb_manButton.normalColor = DeselectButtonColor;
            cb_womanButton.normalColor = SelectButtonColor;
        }
        MySetting_manButton.colors = cb_manButton;
        MySetting_womanButton.colors = cb_womanButton;
    }

    public Button MySetting_OneRoomButton, MySetting_TwoRoomButton, MySetting_ETCRoomButton;
    //public ColorBlock MySetting_cb_OneRoomButton, MySetting_cb_TwoRoomButton, MySetting_cb_ETCRoomButton;


    public void MySetting_Update_user_roomtype(string _room)
    {
        user_roomtype_temp = _room;
        cb_OneRoomButton = MySetting_OneRoomButton.colors;
        cb_TwoRoomButton = MySetting_TwoRoomButton.colors;
        cb_ETCRoomButton = MySetting_ETCRoomButton.colors;

        if (_room == "0")
        {
            cb_OneRoomButton.normalColor = SelectButtonColor;
            cb_TwoRoomButton.normalColor = DeselectButtonColor;
            cb_ETCRoomButton.normalColor = DeselectButtonColor;
        }
        else if (_room == "1")
        {
            cb_OneRoomButton.normalColor = DeselectButtonColor;
            cb_TwoRoomButton.normalColor = SelectButtonColor;
            cb_ETCRoomButton.normalColor = DeselectButtonColor;
        }

        else if (_room == "2")
        {
            cb_OneRoomButton.normalColor = DeselectButtonColor;
            cb_TwoRoomButton.normalColor = DeselectButtonColor;
            cb_ETCRoomButton.normalColor = SelectButtonColor;
        }

        MySetting_OneRoomButton.colors = cb_OneRoomButton;
        MySetting_TwoRoomButton.colors = cb_TwoRoomButton;
        MySetting_ETCRoomButton.colors = cb_ETCRoomButton;

    }

    public Text user_housesize_Text;
    public string user_housesize_temp;
    public void Update_user_housesize()
    {
        user_housesize_temp = user_housesize_Text.text;
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
        if (LikeMoodButton[_index].GetComponent<Toggle>().isOn)
        {
            for (int i = 0; i < 7; i++)
            {
                if (i == _index)
                {
                    selectedIndex = _index;
                    LikeMoodButtonRectTransform = LikeMoodButton[_index].transform.FindChild("Background").GetComponent<RectTransform>().sizeDelta;
                    LikeMoodButtonRectTransform_position = LikeMoodButton[_index].transform.position;
                    user_likemood_temp = LikeMoodButtonText[_index].text.ToString();
                    LikeMoodButton[_index].transform.FindChild("Background").GetComponent<RectTransform>().sizeDelta= new Vector2(LikeMoodButtonRectTransform.x * 1.5f , LikeMoodButtonRectTransform.y * 1.5f);
                    Debug.Log(Screen.width + "ff" + Screen.height);
                    LikeMoodButton[_index].transform.position = new Vector3((Screen.width- LikeMoodButtonRectTransform.x * 1.5f) / 2, -(-Screen.height - LikeMoodButtonRectTransform.y * 1.5f) / 2 ,0); 
                    UserInfoReadyButton.SetActive(true);
                }
                else
                {                    
                    LikeMoodButton[i].SetActive(false);
                }
            }
            ImageUploadButton.SetActive(false);
        }
        else
        {
            user_likemood_temp = "";
            UserInfoReadyButton.SetActive(false);
            LikeMoodButton[selectedIndex].transform.FindChild("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(LikeMoodButtonRectTransform.x, LikeMoodButtonRectTransform.y);
            LikeMoodButton[_index].transform.position = LikeMoodButtonRectTransform_position;
            for (int i = 0; i < 7; i++)
            {
                LikeMoodButton[i].SetActive(true);
            }
            ImageUploadButton.SetActive(true);
        }
    }

    public int userNum = 1;
    private void writeNewUser_Test(string _username, string _useremail, string _userage, string _usersex,
        string _userhousesize, string _userroomtype, string _userindex, string _userlikemood)
    {
        User user = new User("user_1");
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

    public void USerInfoUpdate()  // 빌드시에 설정 필수!! editor에선 풀어주기 
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log("사용자불러오기성공!");
            // user_name_temp = user.DisplayName;
            user_email_temp = user.Email;
            System.Uri photo_url = user.PhotoUrl;
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
        writeNewUser_Test(user_name_temp, user_email_temp, user_age_temp, user_sex_temp,
            user_housesize_temp, user_roomtype_temp, userNum.ToString(), user_likemood_temp);
        userNum++;
    }

    public void USerInfoEdit()
    {
        writeNewUser_Test(user_name_temp, user_email_temp, user_age_temp, user_sex_temp,
            user_housesize_temp, user_roomtype_temp, userNum.ToString(), user_likemood_temp);
        //userNum++;
    }

    // = FirebaseAuth.DefaultInstance;
    FirebaseAuth auth = FirebaseAuth.DefaultInstance;
    public void OpenUserInfoPanel()
    {
        LoadingImage.SetActive(false);

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

            NowPanel = UserInfoPanel2;
        }
    }
    public void OpenUserInfoPanel3()
    {
        if (user_housesize_temp == "" || user_roomtype_temp == "")
        {
            WarnNoInsertInfo.SetActive(true);
        }

        else
        {
            UserInfoPanel1.transform.forward = new Vector3(500, 0, 0);
            UserInfoPanel2.transform.forward = new Vector3(500, 0, 0);
            UserInfoPanel3.transform.forward = new Vector3(0, 0, 0);

            NowPanel = UserInfoPanel3;
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
        NowPanel.SetActive(false);
        LogInPanel.SetActive(true);
        NowPanel = LogInPanel;
        BottomBarPanel.SetActive(false);
        LastPanel = LogInPanel;
    }
    public void OpenLogReadyPanel()
    {
        IsLogIn = true;
        NowPanel.SetActive(false);
        LogReadyPanel.SetActive(true);
        NowPanel = LogReadyPanel;
        //BottomBarPanel.SetActive(true);
        LastPanel = LogReadyPanel;
    }

    //bottombar
    public void OpenRcmFurnPanel()
    {
        //cb_RcmFurnButton = RcmFurnButton.colors;
        LastPanel = NowPanel;
        BeforeBottomPanelChoose = NowPanel;
        Debug.Log("open rcm furn panel");
        NowPanel.SetActive(false);
        RcmFurnPanel.SetActive(true);
        NowPanel = RcmFurnPanel;
    }
    public void OpenSearchPanel()
    {
        LastPanel = NowPanel;
        BeforeBottomPanelChoose = NowPanel;
        Debug.Log("openOpenSearchPanel");
        NowPanel.SetActive(false);
        SearchPanel.SetActive(true);
        NowPanel = SearchPanel;
    }
    public void OpenMyPagePanel()
    {
        LastPanel = NowPanel;
        BeforeBottomPanelChoose = NowPanel;
        NowPanel.SetActive(false);
        Debug.Log("OpenMyPagePanel");
        MyPagePanel.SetActive(true);
        NowPanel = MyPagePanel;
    }

    public void OpenMyLikePanel()
    {
        LastPanel = NowPanel;
        NowPanel.SetActive(false);
        MyLikePanel.SetActive(true);
        Debug.Log("OpenMyLikePanel");
        NowPanel = MyLikePanel;
    }
    public void OpenMySettingPanel()
    {
        LastPanel = NowPanel;
        NowPanel.SetActive(false);
        MySettingPanel.SetActive(true);
        NowPanel = MySettingPanel;
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

    public void BottomBarBack()
    {
        //BeforeBottomPanelChoose = NowPanel;

        if (NowPanel == SearchResultPanel)
        {/*
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

            LoadingImage.SetActive(false);
        }

        NowPanel.SetActive(false);
        LastPanel.SetActive(true);
        NowPanel = LastPanel;
        //LastPanel = MyPagePanel;
    }

    public void OpenFurnitureInfoPanel()
    {
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
        OpenUserInfoPanel();
        OpenUserInfoPanel1();
    }

    public void OnLogin()
    {
        LoadingImage.SetActive(true);

#if UNITY_ANDROID && !UNITY_EDITOR
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
        auth.SignOut();
        LogChange(false);
    }

    // Start is called before the first frame update
    void Start()
    {
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
        });*/

        InitializeFirebase();
        //
#if UNITY_ANDROID && !UNITY_EDITOR
        //InitializeSDK();
        
          //로그인 체크후 로그인 처음부터할꺼면 이거하고 아니면 말고
       LogChange(IsLogIn);
#elif UNITY_EDITOR
        //OpenLogReadyPanel();
        //OpenUserInfoPanel();
         //OpenUserInfoPanel1();
#endif
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
    private void writeNewUser(string userId, string name)
    {
        User user = new User(name);
        string json = JsonUtility.ToJson(user);
        mDatabaseRef.Child("user").Child(userId).SetRawJsonValueAsync(json);
    }
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
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                quad.transform.forward = Camera.main.transform.forward;
                quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                Material material = quad.GetComponent<Renderer>().material;
                if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                material.mainTexture = texture;

                Destroy(quad, 5f);

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                Destroy(texture, 5f);
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

        User user = new User("user_1");
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
    public GameObject ParentsOfResultFurnitureButton;
    public Button[] ResultFurnitureButton;
    public int categorySearchIndex = 0, middleCategoryIndex=0;

    public Button MiddleCategoryButtonPrefab, resultButtonPrefab, temp_resultButtonPrefab;
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
                temp_category[i] = null;
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
    public GameObject MoreFurnButton;
    public Text SearchResultTitleText;

    public void ShowResultFurn(string _searchResultName)
    {
        LoadingImage.SetActive(true);
        //List 초기화
        if (ResultFurnList.Count > 0)
        {            
            for (int i = 0; i < ResultFurnList.Count; i++)
            {
                Debug.Log("삭제 " + ResultFurnList.Count + i);
                Destroy(ResultFurnList[i].gameObject);
            }
            ResultFurnList.Clear();
            ResultFurnList_Image.Clear();
            ResultFurnList_name.Clear();
            categorySearchIndex = 0;
        }

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
                        Debug.Log(" foreach  if문 들어왓다  "+childSnapshot.Child(TopCategory).Value.ToString());
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

        if (categorySearchIndex + 6 < ResultFurnList_name.Count)
        {
            MoreFurnButton.SetActive(true);
            for (int i = categorySearchIndex; i < categorySearchIndex + 6; i++)
            {                
                StartCoroutine(ResultInstantiate(i));               
                /*
                Debug.Log(_searchResultName + "child snapshot if문안에들어왓니" + childSnapshot.Key.ToString());
                //StartCoroutine(ResultInstantiate(childSnapshot.Child("furniture_name").Value.ToString()));                        

                temp_resultButtonPrefab = Instantiate(resultButtonPrefab);
                Debug.Log("child snapshot 부모설정");

                temp_resultButtonPrefab.transform.SetParent(ParentsOfResultFurnitureButton.transform);

                temp_resultButtonPrefab.name = childSnapshot.Child("furniture_name").Value.ToString();
                //Resources.Load(Assets/"Prefab/Button/MiddleCategoryButton"                        

                Debug.Log("child snapshot prefab부름?");
                temp_resultButtonPrefab.transform.FindChild("nolike_result_Text").GetComponent<Text>().text = childSnapshot.Child("furniture_name").Value.ToString();
               // temp_resultButtonPrefab.transform.FindChild("nolike_result_Image").GetComponent<Image>.
                */
            }
            categorySearchIndex += 6;
        }
        else
        {
            MoreFurnButton.SetActive(false);

            for (int i = categorySearchIndex; i < ResultFurnList_name.Count; i++)
            {                
                StartCoroutine(ResultInstantiate(i));                
                /*
                Debug.Log(_searchResultName + "child snapshot if문안에들어왓니" + childSnapshot.Key.ToString());
                //StartCoroutine(ResultInstantiate(childSnapshot.Child("furniture_name").Value.ToString()));                        

                temp_resultButtonPrefab = Instantiate(resultButtonPrefab);
                Debug.Log("child snapshot 부모설정");

                temp_resultButtonPrefab.transform.SetParent(ParentsOfResultFurnitureButton.transform);

                temp_resultButtonPrefab.name = childSnapshot.Child("furniture_name").Value.ToString();
                //Resources.Load(Assets/"Prefab/Button/MiddleCategoryButton"                        

                Debug.Log("child snapshot prefab부름?");
                temp_resultButtonPrefab.transform.FindChild("nolike_result_Text").GetComponent<Text>().text = childSnapshot.Child("furniture_name").Value.ToString();
               // temp_resultButtonPrefab.transform.FindChild("nolike_result_Image").GetComponent<Image>.
                */
            }
            categorySearchIndex = ResultFurnList_name.Count;
        }        
        ParentsOfResultFurnitureButton.gameObject.SetActive(true);
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

        LoadingImage.SetActive(true);
        ResultFurnList.Add(Instantiate(resultButtonPrefab));
        
        ResultFurnList[i].transform.SetParent(ParentsOfResultFurnitureButton.transform);
        yield return new WaitForSeconds(1);
        Debug.Log("IEnumerator ResultInstantiate child snapshot prefab부름?" + i);
        ResultFurnList[i].name = ResultFurnList_name[i]; ;
        ResultFurnList[i].gameObject.SetActive(true);

        ResultFurnList[i].transform.FindChild("nolike_result_Text").GetComponent<Text>().text
        = ResultFurnList_name[i];

        Rect rect = new Rect(0, 0, 232, 271);
        ResultFurnList[i].transform.FindChild("nolike_result_Image").GetComponent<Image>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

        LoadingImage.SetActive(false);
        //  (Sprite)texture as Sprite;
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

    public Scrollbar ScrollbarVertical_Search_Result;
    public void ScrollMoreFurn()
    {
        if (ScrollbarVertical_Search_Result.value >= 0.99)
        {
            PushMoreFurnButton();
        }    
    }
}
