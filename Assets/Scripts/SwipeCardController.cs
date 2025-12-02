using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SurveyUIState
{
    LoadingQuestion,
    WaitingForAnswer,
    SubmittingAnswer,
    ShowingResults
}

// TODO: Refactor local parts when BE is ready

public class SwipeCardController : MonoBehaviour
{
    [Header("Core References")]
    [SerializeField] private PlayerIdentity playerIdentity;
    [SerializeField] private SurveyApiBehaviour api;

    [Header("Question UI")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;
    [SerializeField] private TextMeshProUGUI statusText; // "Loading...", error messages, etc.

    [Header("Results UI")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TextMeshProUGUI agreePercentText;
    [SerializeField] private TextMeshProUGUI disagreePercentText;
    [SerializeField] private Button nextQuestionButton;

    private QuestionData _currentQuestion;
    private SurveyUIState _state;

    private void Awake()
    {
        if (resultsPanel != null)
        {
            resultsPanel.SetActive(false);
        }
    }

    private void Start()
    {
        if (playerIdentity == null)
        {
            Debug.LogError("PlayerIdentity reference is missing.");
            enabled = false;
            return;
        }

        if (api == null)
        {
            Debug.LogError("SurveyApiBehaviour reference is missing.");
            enabled = false;
            return;
        }

        if (agreeButton != null)
            agreeButton.onClick.AddListener(OnAgreeClicked);

        if (disagreeButton != null)
            disagreeButton.onClick.AddListener(OnDisagreeClicked);

        if (nextQuestionButton != null)
            nextQuestionButton.onClick.AddListener(OnNextQuestionClicked);

        LoadNextQuestion();
    }

    private void SetState(SurveyUIState newState, string optionalStatus = null)
    {
        _state = newState;

        bool buttonsInteractable = _state == SurveyUIState.WaitingForAnswer;

        if (agreeButton != null) agreeButton.interactable = buttonsInteractable;
        if (disagreeButton != null) disagreeButton.interactable = buttonsInteractable;

        if (statusText != null)
        {
            statusText.text = optionalStatus ?? "";
        }
    }

    private void LoadNextQuestion()
    {
        SetState(SurveyUIState.LoadingQuestion, "Loading question...");
        if (resultsPanel != null) resultsPanel.SetActive(false);

        api.GetNextQuestion(
            playerIdentity.PlayerId,
            OnQuestionLoaded,
            OnApiError);
    }

    private void OnQuestionLoaded(GetQuestionResponse response)
    {
        _currentQuestion = response.question;

        if (_currentQuestion == null)
        {
            OnApiError("Backend returned no question.");
            return;
        }

        if (questionText != null)
        {
            questionText.text = _currentQuestion.text;
        }

        SetState(SurveyUIState.WaitingForAnswer, "");
    }

    private void OnAgreeClicked()
    {
        if (_state != SurveyUIState.WaitingForAnswer) return;
        SubmitAnswer(AnswerChoice.Agree);
    }

    private void OnDisagreeClicked()
    {
        if (_state != SurveyUIState.WaitingForAnswer) return;
        SubmitAnswer(AnswerChoice.Disagree);
    }

    private void SubmitAnswer(AnswerChoice choice)
    {
        if (_currentQuestion == null)
        {
            Debug.LogWarning("Tried to submit answer without a current question.");
            return;
        }

        SetState(SurveyUIState.SubmittingAnswer, "Submitting...");

        var payload = new AnswerPayload
        {
            playerId = playerIdentity.PlayerId,
            questionId = _currentQuestion.id,
            choice = choice,
            answeredAtUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        api.SubmitAnswer(
            payload,
            OnAnswerSubmitted,
            OnApiError);
    }

    private void OnAnswerSubmitted(SubmitAnswerResponse response)
    {
        if (response == null || response.updatedStats == null)
        {
            OnApiError("Backend returned no stats.");
            return;
        }

        var stats = response.updatedStats;

        if (resultsPanel != null)
            resultsPanel.SetActive(true);

        if (agreePercentText != null)
        {
            agreePercentText.text =
                $"Agree: {(stats.AgreePercentage * 100f):0}% ({stats.agreeCount}/{stats.totalResponses})";
        }

        if (disagreePercentText != null)
        {
            disagreePercentText.text =
                $"Disagree: {(stats.DisagreePercentage * 100f):0}% ({stats.disagreeCount}/{stats.totalResponses})";
        }

        SetState(SurveyUIState.ShowingResults, "");
    }

    private void OnNextQuestionClicked()
    {
        if (_state != SurveyUIState.ShowingResults) return;
        LoadNextQuestion();
    }

    private void OnApiError(string message)
    {
        Debug.LogError("[API error] " + message);
        if (statusText != null)
        {
            statusText.text = "Error: " + message;
        }
    }
}
