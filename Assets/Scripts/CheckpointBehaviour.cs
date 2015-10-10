using UnityEngine;
using System.Collections;

public class CheckpointBehaviour : MonoBehaviour
{

    public enum Type
    {
        BossFightStart,
        BossFightEnd,
        CompositionPlayback
    }

    public Type type = Type.CompositionPlayback;

    void Awake()
    {
        SetColor();
    }

    void SetColor()
    {
        Renderer r = GetComponent<Renderer>();

        switch (type)
        {
            case Type.CompositionPlayback:
                r.materials[0].color = Color.blue;
                break;
            case Type.BossFightStart:
                r.materials[0].color = Color.red;
                break;
            case Type.BossFightEnd:
                r.materials[0].color = Color.green;
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        ComposerBehaviour jukebox = FindObjectOfType<ComposerBehaviour>();

        switch (type)
        {
            case Type.CompositionPlayback:
                jukebox.PlayComposition();
                break;
            case Type.BossFightStart:
                jukebox.StartSoloRecording();
                break;
            case Type.BossFightEnd:
                jukebox.StopSoloRecording();
                break;
        }
    }
}
