using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 _enabledTime;
    [SerializeField] private Vector2 _disabledTime;

    [Header("Components")]
    [SerializeField] private Light _light;

    void Start()
    {
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            _light.enabled = false;
            yield return null;
            yield return new WaitForSeconds(Random.Range(_disabledTime.x, _disabledTime.y));

            _light.enabled = true;
            yield return null;
            yield return new WaitForSeconds(Random.Range(_enabledTime.x, _enabledTime.y));
        }
    }
}