using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

public class FirebaseAuthManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI statusText;

    public GameObject loginPanel;
    public GameObject logoutPanel;

    private FirebaseAuth auth;
    private FirebaseUser user;

    // 初始化 Firebase
    async void Start()
    {
        statusText.text = "Initializing Firebase...";

        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            statusText.text = "Firebase ready ✅";

            user = auth.CurrentUser;
            if (user != null && user.IsEmailVerified)
                UpdateUIAfterLogin();
            else
                UpdateUIBeforeLogin();
        }
        else
        {
            statusText.text = "Firebase init failed: " + dependencyStatus;
        }
    }

    // 註冊帳號
    public async void Register()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        try
        {
var newUserResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
user = newUserResult.User;

            await user.SendEmailVerificationAsync();

            statusText.text = $"✅ 註冊成功！驗證信已寄至 {user.Email}\n請至信箱完成驗證後再登入。";
        }
        catch (System.Exception e)
        {
            statusText.text = "❌ 註冊失敗：" + e.Message;
        }
    }

    // 登入帳號
    public async void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        try
        {
var loginResult = await auth.SignInWithEmailAndPasswordAsync(email, password);
user = loginResult.User;

            await user.ReloadAsync();

            if (!user.IsEmailVerified)
            {
                statusText.text = $"⚠️ 尚未驗證信箱：{user.Email}\n請先至信箱點擊驗證連結。";
                return;
            }

            statusText.text = $"🎉 登入成功！歡迎回來 {user.Email}";
            UpdateUIAfterLogin();
        }
        catch (System.Exception e)
        {
            statusText.text = "❌ 登入失敗：" + e.Message;
        }
    }

    // 重新寄送驗證信
    public async void ResendVerificationEmail()
    {
        if (user != null)
        {
            await user.SendEmailVerificationAsync();
            statusText.text = $"📧 已重新寄送驗證信至 {user.Email}";
        }
        else
        {
            statusText.text = "⚠️ 尚未登入，無法寄送驗證信。";
        }
    }

    // 登出
    public void Logout()
    {
        auth.SignOut();
        user = null;
        statusText.text = "👋 已登出";
        UpdateUIBeforeLogin();
    }

    // 切換 UI 狀態
    void UpdateUIBeforeLogin()
    {
        loginPanel.SetActive(true);
        logoutPanel.SetActive(false);
    }

    void UpdateUIAfterLogin()
    {
        loginPanel.SetActive(false);
        logoutPanel.SetActive(true);
    }
}
