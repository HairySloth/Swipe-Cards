using System;
using UnityEngine;

public enum AnswerChoice
{
    Agree = 1,
    Disagree = -1
}

// Rest API models
// TODO: When BE is ready, refactor models

[Serializable]
public class QuestionData
{
    [Tooltip("Unique identifier coming from your backend.")]
    public string id;

    [TextArea]
    [Tooltip("Localized question text to display to the player.")]
    public string text;

    [Tooltip("Optional category tag ('games', 'NS', 'sports').")]
    public string category;

    [Tooltip("Optional locale code, e.g. 'en', 'nl'. Added just in case, may delete in the future.")]
    public string locale;
}

[Serializable]
public class QuestionStats
{
    public string questionId;
    public int totalResponses;
    public int agreeCount;
    public int disagreeCount;

    public float AgreePercentage => totalResponses > 0
        ? (float)agreeCount / totalResponses
        : 0f;

    public float DisagreePercentage => totalResponses > 0
        ? (float)disagreeCount / totalResponses
        : 0f;
}

[Serializable]
public class AnswerPayload
{
    public string playerId;
    public string questionId;
    public AnswerChoice choice;
    public long answeredAtUnix; // seconds since epoch (UTC)
}

[Serializable]
public class GetQuestionResponse
{
    public QuestionData question;
    public QuestionStats stats; // Can be null if you don't want to send it.
}

[Serializable]
public class SubmitAnswerResponse
{
    public QuestionStats updatedStats;
}