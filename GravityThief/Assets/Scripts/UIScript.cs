using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    [SerializeField] TMP_Text highScoreText;

    private void Start() {
        highScoreText.text = "Highscore: Time: " + PlayerPrefs.GetInt("Time") + " Money: " + PlayerPrefs.GetInt("Money");
    }

    public void restartLevel(){
        SceneManager.LoadScene("Level1");
    }
}
