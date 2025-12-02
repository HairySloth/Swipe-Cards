using System;
using UnityEngine;

public class PlayerIdentity : MonoBehaviour
{
    public const string PlayerIdKey = "PlayerId";

    [Tooltip("Optional fixed player id for debugging. If empty, an ID is auto-generated and persisted.")]
    [SerializeField] private string overridePlayerId;

    public string PlayerId { get; private set; }

    private void Awake()
    {
        if (!string.IsNullOrEmpty(overridePlayerId))
        {
            PlayerId = overridePlayerId;
            return;
        }

        //auto generate an ID
        if (PlayerPrefs.HasKey(PlayerIdKey))
        {
            PlayerId = PlayerPrefs.GetString(PlayerIdKey);
        }
        else
        {
            PlayerId = Guid.NewGuid().ToString("N"); // 32-char hex
            PlayerPrefs.SetString(PlayerIdKey, PlayerId);
            PlayerPrefs.Save();
        }
    }
}
