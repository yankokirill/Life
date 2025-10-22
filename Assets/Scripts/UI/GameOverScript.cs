using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI scoreOneText;
    [SerializeField] private TextMeshProUGUI scoreTwoText;

    public void Setup(CellCounter counter)
    {
        gameObject.SetActive(true);
        if (counter.CountPlayer1 > counter.CountPlayer2)
        {
            titleText.text = "Победа Красных";
            titleText.color = Color.red;
        } else if (counter.CountPlayer1 < counter.CountPlayer2)
        {
            titleText.text = "Победа Синих";
            titleText.color = Color.blue;
        } else
        {
            titleText.text = "Ничья";
            titleText.color = Color.gray;
        }
        scoreOneText.text = counter.CountPlayer1.ToString();
        scoreTwoText.text = counter.CountPlayer2.ToString();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("GamePvP");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
