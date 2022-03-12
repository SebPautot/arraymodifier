using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArrayModifier : MonoBehaviour
{
    public bool disabled = false;
    public int count;
    public Vector3 relativeOffset;
    public Vector3 constantOffset;
    public GameObject gO;
    private bool isGOthis = false;
    private string GOname;
    public List<GameObject> generatedGO;

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        if (!disabled)
        {
            if(!gO) gO = gameObject;
            EmptyPreviews();
            GOname = gO.name;
            UnityEditor.EditorApplication.delayCall += () =>
            {
                Apply(gO);
            };
        }


    }

    void Apply(GameObject gO)
    {

        //Editor only
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        disabled = true;

        

        var toDuplicate = gO;

        // if(toDuplicate == gameObject && toDuplicate.GetComponent<ArrayModifier>()) return;

        if (!toDuplicate) return;

        Renderer renderer = toDuplicate.GetComponent<Renderer>();

        for (int i = 0; i < count; i++)
        {
            disabled = true;
            Vector3 pos = new Vector3(relativeOffset.x * i * renderer.bounds.size.x + constantOffset.x * i + transform.localPosition.x,
            relativeOffset.y * i * renderer.bounds.size.y + constantOffset.y * i + transform.localPosition.y,
            relativeOffset.z * i * renderer.bounds.size.z + constantOffset.z * i + transform.localPosition.z);

            GameObject newGO = Instantiate(toDuplicate, pos, Quaternion.Euler(0, 0, 0), transform) as GameObject;
            generatedGO.Add(newGO);
            toDuplicate = newGO;

            newGO.name = $"{GOname}_{i}";
        }
        disabled = false;
    }

    void EmptyPreviews()
    {
        foreach (GameObject g in generatedGO)
            //Destroy all children
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(g);
            };

        Debug.Log("Clean");

        generatedGO.Clear();
    }

}