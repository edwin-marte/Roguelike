using System.Collections;
using UnityEngine;
using TMPro;

public class SpawnTextMesh : MonoBehaviour
{
    public GameObject textPrefab;

    public void SpawnText(string textToDisplay, Vector3 spawnPosition, GameObject spawnTextObj)
    {
        // Create a new GameObject with TextMeshPro component
        var textGameObject = Instantiate(textPrefab, spawnPosition, Quaternion.identity);
        textGameObject.GetComponentInChildren<TextMeshPro>().text = textToDisplay;
        textGameObject.GetComponentInChildren<DamageText>().spawnTextObject = spawnTextObj;
    }
}
