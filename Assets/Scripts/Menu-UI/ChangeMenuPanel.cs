using UnityEngine;

public class ChangeMenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject audioPanel;

    [SerializeField] private GameObject graphicsPanel;
    [SerializeField] private GameObject inputPanel;

    private bool isFirstPanel = true;

    private GameObject lastOpenPanel;


    // ActivatePanel
    public void UsePanel(string panelName)
    {
        GameObject panelToActivate = null;

        switch (panelName.ToLower())
        {
            case "game":
                panelToActivate = gamePanel;
                break;
            case "audio":
                panelToActivate = audioPanel;
                break;
            case "graphics":
                panelToActivate = graphicsPanel;
                break;
            case "input":
                panelToActivate = inputPanel;
                break;
            default:
                Debug.LogError("Invalid panel name: " + panelName);
                return;
        }

        if (panelToActivate != null)
        {
            panelToActivate.SetActive(true);
            SetLastOpenPanel(panelToActivate);
        }
    }

    // Set the last panel.
    private void SetLastOpenPanel(GameObject newLastOpenPanel)
    {
        if (isFirstPanel && newLastOpenPanel != gamePanel)
        {
            gamePanel.SetActive(false);
            isFirstPanel = false;
        }


        if (lastOpenPanel != null && lastOpenPanel != newLastOpenPanel && !isFirstPanel)
        {
            lastOpenPanel.SetActive(false);
        }
        lastOpenPanel = newLastOpenPanel;
    }
}
