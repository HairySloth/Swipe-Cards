using System;
using UnityEngine;


//For testing only
public class MockSurveyApiBehaviour : SurveyApiBehaviour
{
    [Header("Mock questions shown in order")]
    public QuestionData[] mockQuestions;

    private int _index;

    public override void GetNextQuestion(
        string playerId,
        Action<GetQuestionResponse> onSuccess,
        Action<string> onError)
    {
        if (mockQuestions == null || mockQuestions.Length == 0)
        {
            onError?.Invoke("No mock questions configured on MockSurveyApiBehaviour.");
            return;
        }

        var q = mockQuestions[_index % mockQuestions.Length];
        _index++;

        var response = new GetQuestionResponse
        {
            question = q,
            stats = new QuestionStats
            {
                questionId = q.id,
                totalResponses = 0,
                agreeCount = 0,
                disagreeCount = 0
            }
        };

        onSuccess?.Invoke(response);
    }

    public override void SubmitAnswer(
        AnswerPayload payload,
        Action<SubmitAnswerResponse> onSuccess,
        Action<string> onError)
    {
        //fake stats for testing.
        var stats = new QuestionStats
        {
            questionId = payload.questionId,
            agreeCount = UnityEngine.Random.Range(0, 1000),
            disagreeCount = UnityEngine.Random.Range(0, 1000)
        };
        stats.totalResponses = stats.agreeCount + stats.disagreeCount;

        var response = new SubmitAnswerResponse
        {
            updatedStats = stats
        };

        onSuccess?.Invoke(response);
    }
}
