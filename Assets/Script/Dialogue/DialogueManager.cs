using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    private GameObject Player;
    private GameObject dialoguePanel;
    TextMeshPro dialogueText;


    private Story currentStory;

     public static bool dialogueIsPlaying { get;private set; }

    private static DialogueManager instance;

    [SerializeField] private float typingEffectSpeed = 0.04f;


    private Coroutine displayLineCoroutine;

    bool isAddingRichTextTag,skipTyping;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] dialogueSoundClips;

    [SerializeField, Range(1f, 6f)]
    private float frequencyLevel = 2;

    [SerializeField, Range(-5f, 5f)]
    private float maxPitch = 3;

    [SerializeField, Range(-5f, 5f)]
    private float minPitch = 0.5f;

    [SerializeField]
    private bool makePredictable;


    [SerializeField] 
    private bool StopAudioSourceAfterPlay;

    private void Awake()
    {

        if(instance != null)
        {
            Debug.LogWarning("find onother dialogue manager");
        }else  instance = this;

        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public static DialogueManager getInstance()
    {
        return instance;
    }
    void Start()
    {
        Player = GameObject.Find("Player");
        dialoguePanel = FindChildrenObject.GetChildWithName(Player, "DialoguePanel");
        dialogueText = FindChildrenObject.GetChildWithName(dialoguePanel, "Text").GetComponent<TextMeshPro>();

        dialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogueIsPlaying)  //in case dialogue is playing
        {
            return;
        }

        if (InputManager.GetInstance().GetInteractPressed())
        {
            Debug.Log("Continue");
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        if (!dialogueIsPlaying)
        {
            #region Disable Movement
            PlayerMovement._disableAllMovement = true;
            #endregion

            currentStory = new Story(inkJSON.text);
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);
            ContinueStory();
        }
    }

    public void ContinueStory()
    {

        if (displayLineCoroutine == null)
        {
            if (currentStory.canContinue)
            {
                displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            }
            else
            {
                ExitDialogueMode();
            }
        }
        else
        {
            skipTyping = true;
        }
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        #region Enable Movement
        PlayerMovement._disableAllMovement = false;
        #endregion
    }

    public IEnumerator DisplayLine(string line)
    {
        int visiableChacterCount = 0;
        dialogueText.text = "";
        foreach(char letter in line.ToCharArray())
        {
            if (skipTyping)
            {
                skipTyping = false;
                dialogueText.text = line;
                break;
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                dialogueText.text += letter;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else  //skip < > 
            {
                PlayDialogueSound(visiableChacterCount,letter);

                dialogueText.text += letter;
                visiableChacterCount++;
                 yield return new WaitForSeconds(typingEffectSpeed);
            }
        }
        displayLineCoroutine = null;

    }

    private void PlayDialogueSound(int currentDisplayCharacterCount, char currentCharacter)
    {
        if(currentDisplayCharacterCount % frequencyLevel == 0) //play sound every 2 character
        {
            //stop sound before play new
            if (StopAudioSourceAfterPlay)
            {
                audioSource.Stop();

            }


            AudioClip soundClip = null;
            //predictable
            if (makePredictable)
            {
                int hashCode = currentCharacter.GetHashCode();

                //sound clip
                int predictableIndex = hashCode % dialogueSoundClips.Length;
                soundClip = dialogueSoundClips[predictableIndex];

                //pitch
                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int)(maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;

                if(pitchRangeInt != 0)
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;
                }
                else
                {
                    audioSource.pitch = minPitch;
                }

            }
            //otherwise, random audio clip
            else
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                soundClip = dialogueSoundClips[Random.Range(0, dialogueSoundClips.Length)];
            }

       
            audioSource.PlayOneShot(soundClip); 
        }    
    }
}