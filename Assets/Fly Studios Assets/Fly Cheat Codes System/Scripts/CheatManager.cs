using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CheatManager : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Lista tuturor definițiilor de cheat-uri disponibile în joc.")]
    public List<ModularCheatDefinition> availableCheats; // Actualizat la noul tip de script

    [Tooltip("Timpul (în secunde) după care inputul se resetează.")]
    public float inputTimeout = 1.5f;

    [Header("UI Feedback (Optional)")]
    public Text debugText;
    public GameObject notificationPrefab;

    private readonly StringBuilder _inputBuffer = new StringBuilder();
    private float _timer;

    private void Update()
    {
        // 1. Colectarea eficientă a inputului
        if (!string.IsNullOrEmpty(Input.inputString))
        {
            _inputBuffer.Append(Input.inputString.ToUpper());
            _timer = inputTimeout;
        }

        // 2. Logica de timeout
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _inputBuffer.Clear();
            }
        }

        if (debugText != null)
        {
            debugText.text = _inputBuffer.ToString();
        }

        CheckBufferForCheats();
    }

    private void CheckBufferForCheats()
    {
        if (_inputBuffer.Length == 0) return;

        string currentInput = _inputBuffer.ToString();

        foreach (ModularCheatDefinition cheat in availableCheats)
        {
            if (currentInput.EndsWith(cheat.cheatCode.ToUpper()))
            {
                // Activăm direct acțiunea din definiția cheat-ului
                cheat.ExecuteAction(gameObject);

                _inputBuffer.Clear();
                _timer = 0;
                StartCoroutine(ShowNotification());
                break;
            }
        }
    }

    private System.Collections.IEnumerator ShowNotification()
    {
        if (notificationPrefab != null)
        {
            notificationPrefab.SetActive(true);
            yield return new WaitForSeconds(2f);
            notificationPrefab.SetActive(false);
        }
    }
}