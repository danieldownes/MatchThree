using UnityEngine;
using UnityEngine.UI;

using System;

 
/// <summary>
/// Dispatched when score updated
/// </summary>
public class ScoreUpdatedEventArgs : EventArgs
{
}


public interface IScore
{
    // Dispatched when the enemy is clicked
    //event EventHandler<ScoreUpdatedEventArgs> ScoreUpdated;

    /// <summary>
    /// To be called on start of new game
    /// </summary>
    void Init();


    /// <summary>
    /// Add (or subtract) to the total score
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    int AddPoints(int amount);

    void IncreaseMultiplier();

    void ResetMultiplier();
}


public class Score : MonoBehaviour, IScore
{
    public Text ScoreText;
    private int _total;
    private int _multiplier;
    private static int MULTIPLIER_INCREMENTS = 10;

    //public event EventHandler<ScoreUpdatedEventArgs> ScoreUpdated = (sender, e) => { };

    public void Init()
    {
        _total = 0;
        UpdateText();
    }

    public int AddPoints(int newPoints)
    {
        _total += newPoints * _multiplier;
        UpdateText();
        return _total;
    }

    public void IncreaseMultiplier()
    {
        _multiplier += MULTIPLIER_INCREMENTS;
    }

    public void ResetMultiplier()
    {
        _multiplier = MULTIPLIER_INCREMENTS;
    }


    private void UpdateText()
    {
        ScoreText.text = "Score: " + _total.ToString();
    }

    void Update()
    {
    }
}
