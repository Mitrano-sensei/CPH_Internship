using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogInfo", menuName = "Dialog System/Dialog")]
public class DialogInfo : ScriptableObject
{
    [SerializeField] private string dialogName = "Dialog";
    [SerializeField] private List<DialogLine> dialogLines = new();
    
    public string DialogName => dialogName;
    public List<DialogLine> DialogLines => dialogLines;
    public DialogLine GetDialogLine(int index) => dialogLines[index];
}

[Serializable]
public class DialogLine
{
    [SerializeField] private string speakerName = "Speaker";
    [SerializeField] private string dialogText = "Hello!";
    [SerializeField] private AudioClip dialogAudio = null;
    
    public string SpeakerName => speakerName;
    public string DialogText => dialogText;
    public AudioClip DialogAudio => dialogAudio;
}
