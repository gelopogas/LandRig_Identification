﻿using System.Collections;
using Mouledoux.Callback;
using Mouledoux.Components;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int _score;
    private int _targetScore;
    public string Message;
    private Mediator.Subscriptions _subscriptions;
    private Callback _incrementScoreCallback;

    private void Awake()
    {
        _targetScore = transform.childCount;
        _subscriptions = new Mediator.Subscriptions();
        _incrementScoreCallback += IncrementScore;
        _subscriptions.Subscribe(gameObject.name, _incrementScoreCallback);
    }

    /// <summary>
    ///     Reset Score and then wait until target score is it to notify subs
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        ResetScore();
        yield return new WaitUntil(() => GetScore() >= _targetScore);
        Mediator.instance.NotifySubscribers(Message, new Packet());
    }

    /// <summary>
    ///     Returns the current score
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return _score;
    }

    /// <summary>
    ///     Sets the current score
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int SetScore(int value)
    {
        return _score = value;
    }

    /// <summary>
    ///     Resets the current score
    /// </summary>
    public void ResetScore()
    {
        SetScore(0);
    }

    /// <summary>
    ///     Increment the score by 1
    /// </summary>
    public void IncrementScore()
    {
        _score++;
    }

    public void IncrementScore(Packet emptyPacket)
    {
        _score++;
    }

    private void OnDestroy()
    {
        _subscriptions.UnsubscribeAll();
    }
}
