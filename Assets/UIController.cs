using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] string difficulty;
    [SerializeField] int health = 3;
    [SerializeField] int score = 0;
    [SerializeField] int selector = -1;
    [SerializeField] float timer = 3f;
    [SerializeField] float countdownTimer = 3f;
    [SerializeField] bool succeeded = true;
    [SerializeField] bool clickAttempted = false;
    [SerializeField] bool countdown = true;

    [SerializeField] RawImage[] healthHearts;
    [SerializeField] Button[] colors;
    [SerializeField] RawImage[] arrows;
    [SerializeField] GameObject[] lasers;

    [SerializeField] TMP_Text difficultyText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject difficultyScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject hud;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject instructionsScreen;
    [SerializeField] TMP_Text victoryText;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text countdownText;

    void Start()
    {
        Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if(!countdown)
        {
            if (timer <= 0)
            {
                foreach (Button color in colors)
                {
                    color.gameObject.SetActive(false);
                }

                foreach (RawImage arrow in arrows)
                {
                    arrow.gameObject.SetActive(false);
                }

                if (!succeeded && !clickAttempted)
                {
                    LoseHealth();
                }
                else
                {
                    foreach (GameObject laser in lasers)
                    {
                        laser.gameObject.SetActive(false);
                    }
                }

                ResetTimer();
                selector = Random.Range(0, 16);
                succeeded = false;
                clickAttempted = false;
            }

            timer -= Time.deltaTime;
            arrows[selector].gameObject.SetActive(true);

            if (selector >= 0 && selector <= 3)
            {
                colors[0].gameObject.SetActive(true);
            }
            else if (selector >= 4 && selector <= 7)
            {
                colors[1].gameObject.SetActive(true);
            }
            else if (selector >= 8 && selector <= 11)
            {
                colors[2].gameObject.SetActive(true);
            }
            else
            {
                colors[3].gameObject.SetActive(true);
            }
        }
        else
        {
            countdownTimer -= Time.deltaTime;
            countdownText.SetText(countdownTimer.ToString("F0"));
            if(countdownTimer <= 0)
            {
                countdown = false;
                countdownText.gameObject.SetActive(false);
            }
        }  
    }

    public void PlayerClickedButton()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (selector == 0 || selector == 4 || selector == 8 || selector == 12)
            {
                SuccessfulBeat();
            }
            else
            {
                clickAttempted = true;
                LoseHealth();
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (selector == 1 || selector == 5 || selector == 9 || selector == 13)
            {
                SuccessfulBeat();
            }
            else
            {
                clickAttempted = true;
                LoseHealth();
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (selector == 2 || selector == 6 || selector == 10 || selector == 14)
            {
                SuccessfulBeat();
            }
            else
            {
                clickAttempted = true;
                LoseHealth();
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (selector == 3 || selector == 7 || selector == 11 || selector == 15)
            {
                SuccessfulBeat();
            }
            else
            {
                clickAttempted = true;
                LoseHealth();
            }
        }
        else
        {
            clickAttempted = true;
            LoseHealth();
        }

    }

    private void LoseHealth()
    {
        if(health > 0)
        {
            healthHearts[health - 1].gameObject.SetActive(false);
            health--;
        }
        
        if(health == 0)
        {
            EndScreen("YOU LOST...");
        }
    }
    
    private void SuccessfulBeat()
    {
        if(!succeeded)
        {
            succeeded = true;
            score++;
            scoreText.SetText("SCORE: " + score);

            if (colors[0].gameObject.activeSelf)
            {
                lasers[0].gameObject.SetActive(true);
            }
            else if (colors[1].gameObject.activeSelf)
            {
                lasers[1].gameObject.SetActive(true);
            }
            else if (colors[2].gameObject.activeSelf)
            {
                lasers[2].gameObject.SetActive(true);
            }
            else
            {
                lasers[3].gameObject.SetActive(true);
            }

            if(score == 30)
            {
                EndScreen("YOU WON!");
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        countdown = true;
        countdownTimer = 3;
        countdownText.gameObject.SetActive(true);
        foreach (Button color in colors)
        {
            color.gameObject.SetActive(false);
        }

        foreach (RawImage arrow in arrows)
        {
            arrow.gameObject.SetActive(false);
        }
    }

    public void ResetGame()
    {
        health = 3;
        foreach (RawImage healthHeart in healthHearts)
        {
            healthHeart.gameObject.SetActive(true);
        }
        score = 0;
        scoreText.SetText("SCORE: " + score);
    }

    public void SetDifficulty(string level)
    {
        difficulty = level;
        showUIScreen(hud);
        showUIScreen(difficultyScreen);
        difficultyText.SetText("DIFFICULTY: " + difficulty);
        Unpause();
    }

    public void ResetTimer()
    {
        if(difficulty.Equals("BEGINNER"))
        {
            timer = 3;
        }
        else if(difficulty.Equals("EASY"))
        {
            timer = 2;
        }
        else if(difficulty.Equals("MEDIUM"))
        {
            timer = 1;
        }
        else
        {
            timer = 0.5f;
        }
    }

    public void StartGame()
    {
        showUIScreen(mainMenu);
        showUIScreen(difficultyScreen);
    }

    public void Quit()
    {
        ResetGame();
        Pause();
        showUIScreen(mainMenu);
        showUIScreen(hud);
    }

    public void EndScreen(string winOrLose)
    {
        Pause();
        showUIScreen(endScreen);
        showUIScreen(hud);
        victoryText.SetText(winOrLose);
        finalScoreText.SetText("FINAL SCORE: " + score);
        ResetGame();
    }

    public void showUIScreen(GameObject uiScreen)
    {
        if(uiScreen.gameObject.activeSelf)
        {
            uiScreen.gameObject.SetActive(false);
        }
        else
        {
            uiScreen.gameObject.SetActive(true);
        }
    }
}
