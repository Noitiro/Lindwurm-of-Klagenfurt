using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class CameraShake : MonoBehaviour {

    public static CameraShake Instance { get; private set; }
    private CinemachineImpulseSource impulseSource;

    private void Awake() {
        Instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float duration, float magnitude) {

        impulseSource.GenerateImpulseWithForce(magnitude);
    }
}