using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class RL_BedroomCamera : MonoBehaviour
{
    public float damping = 2.0f;

    [SerializeField]
    private float cameraMin;
    [SerializeField]
    private float cameraMax;
    [SerializeField]
    private float characterMin;
    [SerializeField]
    private float characterMax;
    [SerializeField]
    private Transform CharacterTransform;

    void Update()
    {
        if (CharacterTransform == null)
            return;

        float z = cameraMin +
            (cameraMin - cameraMax) * (CharacterTransform.position.z - characterMin) / (characterMin - characterMax);

        // Smooth Follow
        z = Mathf.Lerp(transform.position.z, z, Time.deltaTime * damping);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
