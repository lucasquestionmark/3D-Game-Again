using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Drag your Boss enemy here.")]
    public GameObject bossEnemy;

    [Tooltip("Drag your 'You Win' UI text or panel here.")]
    public GameObject winScreenUI;

    // We use a flag so we don't try to turn it on 60 times a second
    private bool gameWon = false;

    void Update()
    {
        // Check if the boss was assigned but is now gone (destroyed)
        if (bossEnemy == null && !gameWon)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameWon = true;
        Debug.Log("Boss Defeated! You Win!");

        // Turn on the Win Screen
        if (winScreenUI != null)
        {
            winScreenUI.SetActive(true);
        }

        // Optional: Unlock the mouse cursor so they can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}