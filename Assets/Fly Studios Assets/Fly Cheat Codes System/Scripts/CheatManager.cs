using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Adăugăm RequireComponent pentru a ne asigura că noul script există pe jucător.
[RequireComponent(typeof(SmartSpawnCalculator))]
public class CheatManager : MonoBehaviour
{
    [Header("Configuration")]
    public List<ModularCheatDefinition> availableCheats;
    public float inputTimeout = 1.5f;

    [Header("UI Feedback (Optional)")]
    public GameObject notificationPrefab;

    // Referințe interne
    private readonly StringBuilder _inputBuffer = new StringBuilder();
    private float _timer;
    private SmartSpawnCalculator _spawnCalculator; // Referința la noul script

    private void Start()
    {
        // Obținem referința la calculator la pornire
        _spawnCalculator = GetComponent<SmartSpawnCalculator>();
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(Input.inputString))
        {
            _inputBuffer.Append(Input.inputString.ToUpper());
            _timer = inputTimeout;
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _inputBuffer.Clear();
            }
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
                // Trimitem referința la calculator către funcția de execuție
                cheat.ExecuteAction(gameObject, _spawnCalculator);

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