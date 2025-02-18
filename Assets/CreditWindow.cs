using UnityEngine;

public class CreditWindow : MonoBehaviour
{
    public void OpenOnWeb(string url)
    {
        Application.OpenURL(url);
    }
}
