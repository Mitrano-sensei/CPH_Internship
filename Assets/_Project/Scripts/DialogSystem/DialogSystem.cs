using System.Collections;
using TMPro;
using UnityEngine;
using Utilities;

public class DialogSystem : MonoBehaviour
{
    [Header("Dialog System")]
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float letterPerSecond = 10f;

    [Header("Misc")] 
    [SerializeField] private bool followPlayer = true;
    
    private DialogInfo _dialogInfo;
    private int _currentLineIndex;
    
    private bool _isTyping = false;
    private IEnumerator _showTextCoroutine;

    private Camera _camera;
    
    public bool IsTyping => _isTyping;
    public bool IsDialogFinished => _currentLineIndex >= _dialogInfo.DialogLines.Count && !_isTyping;
    public DialogInfo CurrentDialog => _dialogInfo;
    
    public void StartDialog(DialogInfo dialog)
    {
        if (_showTextCoroutine != null) StopDialog();

        _isTyping = true;

        _dialogInfo = dialog;
        _currentLineIndex = 0;

        _showTextCoroutine = ShowDialog();
        StartCoroutine(_showTextCoroutine);
    }
    
    public void NextLine(bool lastLine = false)
    {
        if (_showTextCoroutine != null) StopDialog();
        _currentLineIndex += lastLine ? -1 : 1;

        if (_currentLineIndex >= _dialogInfo.DialogLines.Count) return; // If it is finished, do nothing
        
        _isTyping = true;
        _showTextCoroutine = ShowDialog();
        StartCoroutine(ShowDialog());
    }

    
    private IEnumerator ShowDialog()
    {
        // Reset the dialog text
        textMeshPro.text = "";
        
        // Get the current dialog line
        var dialogLine = _dialogInfo.GetDialogLine(_currentLineIndex);
        
        // Set the speaker name
        textMeshPro.text = dialogLine.SpeakerName + ": \n";
        
        // Play the dialog audio and set the audio countdown
        var startTime = Time.time;
        audioSource.clip = dialogLine.DialogAudio;

        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        
        // Show the dialog text
        foreach (var letter in dialogLine.DialogText)
        {
            textMeshPro.text += letter;
            
            if (letter != ' ') yield return new WaitForSeconds(1f/letterPerSecond);
        }

        // Wait for the audio to finish
        var audioDuration = audioSource.clip != null ? audioSource.clip.length : 0;
        var remainingTime = audioDuration - (Time.time - startTime);

        yield return new WaitForSeconds(remainingTime);

        // Finish the dialog
        _isTyping = false;
    }
    
    private void StopDialog()
    {
        if (_showTextCoroutine != null) StopCoroutine(_showTextCoroutine);
        textMeshPro.text = "";
        audioSource.Stop();
    }
    
    // Temp
    // [SerializeField] private DialogInfo dialogInfo;
    private void Start()
    {
        // StartDialog(dialogInfo);
        
        _camera = Camera.main;
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            NextLine(true);
        }
        */

        if (followPlayer)
        {
            // transform.position = _camera.transform.position + _camera.transform.forward * 2;
            transform.LookAt(_camera.transform);
        }
    }
}
