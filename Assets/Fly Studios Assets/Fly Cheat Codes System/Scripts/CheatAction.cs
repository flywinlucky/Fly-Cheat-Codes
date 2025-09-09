using UnityEngine;

// Clasa de bază abstractă pentru toate acțiunile de cheat.
// Toate acțiunile noi (ex: GiveHealth, ChangeWeather) vor moșteni din aceasta.
public abstract class CheatAction : ScriptableObject
{
    /// <summary>
    /// Execută logica specifică a cheat-ului.
    /// </summary>
    /// <param name="activator">Obiectul care a activat cheat-ul (de obicei, jucătorul).</param>
    public abstract void Execute(GameObject activator);
}