using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishMenu : MonoBehaviour, ILevelObserver
{
    public static FinishMenu Instance;
    public PlayerState playerState;
    public GameConfig gameConfig;

    public Text scoreText;
    public Text levelText;
    public Text newBestText;

    public Text ballsCountText;
    public Text ballsText;

    public GameObject twentyBalls;
    
    public GameObject VideoIcon;
    public GameObject NoThankYouButton;
    public GameObject NonBonus;

   private bool _bonus;

    public void OnLevelComplete()
    {
        DOTween.Sequence().AppendInterval(RemoteSettings.GetFloat("FinishMenuDelay", 1.5f)).AppendCallback(() =>
        {
            var isNewBest = playerState.score > playerState.best;
            if (isNewBest)
            {
                newBestText.gameObject.SetActive(true);

                playerState.best = playerState.score;
                playerState.Save();
            }

            scoreText.text = (playerState.score).ToString();
            levelText.text = (playerState.level + 1).ToString();
            ballsCountText.text = "+" + RemoteSettings.GetInt("ballsPerLevel", 15);

            bool isExtraBalls = false;
            if (playerState.level % RemoteSettings.GetInt("ExtraBallsLevel", 3) == 0)
            {
                twentyBalls.SetActive(true);
                isExtraBalls = true;
            }

            gameObject.SetActive(true);

            if (!isExtraBalls && Random.value < RemoteSettings.GetFloat("BonusLevelChance", 0.3f) && playerState.level > RemoteSettings.GetInt("AdsMinimumLevel", 10))
            {
                _bonus = true;
            }
            NoThankYouButton.SetActive(_bonus);
            VideoIcon.SetActive(_bonus);
            NonBonus.SetActive(!_bonus);
        });
    }
    
    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);

        playerState.levelObservers.Add(this);

        twentyBalls.SetActive(false);
        newBestText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        playerState.levelObservers.Remove(this);
    }

    public void NoThankYou()
    {
        Go();
    }

    void Go()
    {
        playerState.level++;
        playerState.Save();

        if (Level.instance.CanShowAds())
        {
            Level.instance.ShowAds((bool result) => {
                SceneManager.LoadScene(1);
            });
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void Next()
    {
        if (_bonus)
        {
            if (AdsAnaliticsManager.instance.CanShowRewarded())
            {
                AdsAnaliticsManager.instance.ShowRewarded((bool result) =>
                    {
                        if (result)
                        {
                            PlayerPrefs.SetInt("BonusLevel", 1);
                            PlayerPrefs.SetInt("SkipAds", 1);
                            Go();
                        }
                    }
                );
            }
        }
        else
        {
            Go();
        }
    }

    public void OnLevelStart()
    {
    }
}
