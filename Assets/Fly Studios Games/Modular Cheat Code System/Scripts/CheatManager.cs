using System; // Necesar pentru 'Action'
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SmartSpawnCalculator))]
public class CheatManager : MonoBehaviour
{
    // --- Eveniment Public Static ---
    // Orice alt script se poate abona la acest eveniment fără a avea o referință directă la CheatManager.
    public static event Action OnCheatActivated;

    [Header("Configuration")]
    public List<ModularCheatDefinition> availableCheats;
    public float inputTimeout = 1.5f;

    // Referințe interne
    private readonly StringBuilder _inputBuffer = new StringBuilder();
    private float _timer;
    private SmartSpawnCalculator _spawnCalculator;

    private void Start()
    {
        _spawnCalculator = GetComponent<SmartSpawnCalculator>();
    }

    private void Update()
    {
        // 1. Colectarea inputului
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
                cheat.ExecuteAction(gameObject, _spawnCalculator);

                // --- Declansarea Evenimentului ---
                // Anunțăm restul jocului că un cheat a fost activat.
                // Dacă niciun script nu s-a abonat (ex: NotificationController lipsește), nu se întâmplă nimic.
                OnCheatActivated?.Invoke();

                _inputBuffer.Clear();
                _timer = 0;
                break;
            }
        }
    }

    // Am eliminat corutina ShowNotification() de aici. Logica este acum în NotificationController.
}