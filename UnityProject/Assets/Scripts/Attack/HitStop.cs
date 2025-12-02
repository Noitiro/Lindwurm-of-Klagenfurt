using UnityEngine;
using System.Collections;

public class HitStop : MonoBehaviour {
    public static HitStop Instance;
    private bool isWaiting = false;

    private void Awake() { Instance = this; }

    public void Stop(float duration) {
        if (isWaiting) return;

        StartCoroutine(DoHitStop(duration));
    }

    private IEnumerator DoHitStop(float duration) {
        isWaiting = true;

        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(duration);

        // 3. Wznów czas
        Time.timeScale = 1.0f;
        isWaiting = false;
    }
}