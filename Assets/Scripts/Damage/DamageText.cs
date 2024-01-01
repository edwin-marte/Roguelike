using System.Collections;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float displayDuration = 3.0f;
    public float fadeDuration = 1.0f;
    public GameObject spawnTextObject;

    private void Start()
    {
        StartCoroutine(DisplayAndFade());
    }

    private IEnumerator DisplayAndFade()
    {
        var textMesh = GetComponent<TextMeshPro>();

        yield return new WaitForSeconds(displayDuration);

        float startTime = Time.time;
        Color startColor = textMesh.color;

        while (Time.time - startTime < fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            Color newColor = Color.Lerp(startColor, Color.clear, t);
            textMesh.color = newColor;
            yield return null;
        }

        Destroy(textMesh.gameObject);
        Destroy(textMesh.gameObject.transform.parent.gameObject);
        Destroy(spawnTextObject);
    }
}
