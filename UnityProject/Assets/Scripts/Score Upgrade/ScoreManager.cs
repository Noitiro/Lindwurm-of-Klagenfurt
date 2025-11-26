using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour {
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; }

    public event Action<int> OnScoreChanged;

    private void Awake() {
        Debug.Log("1. ScoreManager siê budzi!");
        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(gameObject); //punkty miêdzy scenami
        }
        else {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount) {
        CurrentScore += amount;
        Debug.Log("Dodano punkty: " + amount + ". Razem: " + CurrentScore);

        OnScoreChanged?.Invoke(CurrentScore);
    }

    public bool SpendScore(int amount) {
        if (CurrentScore >= amount) {
            CurrentScore -= amount;
            OnScoreChanged?.Invoke(CurrentScore);
            return true; 
        }
        else {
            Debug.Log("Nie staæ ciê! Brakuje: " + (amount - CurrentScore));
            return false; 
        }
    }
}