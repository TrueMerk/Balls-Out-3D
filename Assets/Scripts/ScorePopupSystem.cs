using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScorePopupSystem : MonoBehaviour, ICaptureBallObserver
{
    public PlayerState playerState;

    public ScorePopup scorePrefab;   
    public ScorePopup perfectPrefab;   

    List<ScorePopup> scores = new List<ScorePopup>();

    private float _scoreLifeTime;
    private float _firstBallTime;
    public static bool isFirstBall = true;

    private void OnEnable()
    {
        playerState.captureBallObservers.Add(this);

        _scoreLifeTime = RemoteSettings.GetFloat("ScoreLifeTime", 1.6f);
    }

    private void OnDisable()
    {
        playerState.captureBallObservers.Remove(this);
    }

    public void OnCaptureBall(Ball ball)
    {
        if(isFirstBall)
        {
            _firstBallTime = Time.time;
            isFirstBall = false;
        }

        var capturedTime = Time.time - _firstBallTime;

        var isPerfect = playerState.capturedBalls == playerState.totalBallsPerfect - 1 && capturedTime < (playerState.totalBalls / 100f + 1f)*RemoteSettings.GetFloat("PerfectTimeMult", 1f);

        if (!RemoteSettings.GetBool("emit_digits", false) && !isPerfect) return;

        var score = Instantiate(isPerfect ? perfectPrefab : scorePrefab, transform);
        
        score.transform.position = Camera.main.WorldToScreenPoint(ball.transform.position) + Vector3.right * Random.Range(-100f, 100f);

        if (!isPerfect)
        {
            score.tmText.text = (playerState.level + 1).ToString();
            score.speed = Random.Range(RemoteSettings.GetFloat("ScoreJumpSpeedMin", 350f), RemoteSettings.GetFloat("ScoreJumpSpeedMax", 450f));

            scores.Add(score);
        }
        else
        {
            var duration = Random.Range(RemoteSettings.GetFloat("ScoreJumpTimeMin", 0.6f), RemoteSettings.GetFloat("ScoreJumpTimeMax", 0.9f));
            var durationScale = RemoteSettings.GetFloat("ScoreScaleTime", 0.2f);

            var v = score.transform.localPosition;
            v.x = 0f;
            score.transform.localPosition = v;

            DOTween.Sequence()
                .Append(score.transform.DOLocalMove(score.transform.localPosition+ Vector3.up * Random.Range(RemoteSettings.GetFloat("ScoreJumpMin", 550f), RemoteSettings.GetFloat("ScoreJumpMax", 650f)), duration))
                .Append(score.transform.DOScale(1.2f, durationScale))
                .Append(score.transform.DOScale(1f, durationScale))
                .Append(score.transform.DOScale(1.2f, durationScale))
                .Append(score.transform.DOScale(1f, durationScale))
                .Append(score.transform.DOScale(1.2f, durationScale))
                .Append(score.transform.DOScale(1f, durationScale))
                .AppendInterval(RemoteSettings.GetFloat("ScoreDestroyTime", 0.1f))
                .AppendCallback(()=>{ Destroy(score.gameObject); });
        }
    }
    
    void Update()
    {
        var i = scores.Count;
        while(i > 0)
        {
            i--;
            var score = scores[i];

            score.lifeTime += Time.deltaTime;
            score.transform.localPosition += Vector3.up * score.speed * Time.deltaTime;
            if(score.lifeTime > _scoreLifeTime)
            {
                scores.Remove(score);
                Destroy(score.gameObject);
            }
            else if(score.lifeTime > _scoreLifeTime - 0.3f)
            {
                var color = score.tmText.color;
                color.a = (_scoreLifeTime - score.lifeTime) / 0.3f;
                score.tmText.color = color;
            }
        }
    }
}
