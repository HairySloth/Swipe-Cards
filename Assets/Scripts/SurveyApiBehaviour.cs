using System;
using UnityEngine;

public abstract class SurveyApiBehaviour : MonoBehaviour
{
    public abstract void GetNextQuestion(
        string playerId,
        Action<GetQuestionResponse> onSuccess,
        Action<string> onError);

    public abstract void SubmitAnswer(
        AnswerPayload payload,
        Action<SubmitAnswerResponse> onSuccess,
        Action<string> onError);
}
