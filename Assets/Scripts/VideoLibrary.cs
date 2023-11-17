using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public struct LibraryEntry
{
    public string name;
    public VideoClip clip;
    public GameObject decorationPrefab;
}

public class VideoLibrary: MonoBehaviour
{
    [SerializeField]
    public List<LibraryEntry> entries;
}
