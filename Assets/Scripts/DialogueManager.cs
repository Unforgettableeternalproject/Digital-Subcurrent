﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Digital_Subcurrent
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        public GameObject leftDialogueBox;
        public GameObject rightDialogueBox;

        private Animator leftAnimator;
        private Animator rightAnimator;

        [Range(0.01f, 0.2f)]
        public float typingSpeed = 0.05f;

        private Queue<string> sentences;

        private TextMeshProUGUI leftNameText;
        private TextMeshProUGUI leftDialogueText;
        private Button leftContinueButton;

        private TextMeshProUGUI rightNameText;
        private TextMeshProUGUI rightDialogueText;
        private Button rightContinueButton;

        private bool isTyping = false, inStory = false, showCutscene = false;
        private string currentSentence;

        private StoryManager storyManager;

        // 預設角色名稱
        private const string LeftCharacterName = "YOU";
        private const string RightCharacterName = "MIPPY";

        public AudioClip audioClipL;
        public AudioClip audioClipR;
        public AudioSource audioSource;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            storyManager = StoryManager.Instance;
            sentences = new Queue<string>();

            // 動態查找左側對話框內部組件
            leftNameText = leftDialogueBox.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            leftDialogueText = leftDialogueBox.transform.Find("Dialogue").GetComponent<TextMeshProUGUI>();
            leftContinueButton = leftDialogueBox.transform.Find("ContinueButton").GetComponent<Button>();
            leftAnimator = leftDialogueBox.GetComponent<Animator>();

            // 動態查找右側對話框內部組件
            rightNameText = rightDialogueBox.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            rightDialogueText = rightDialogueBox.transform.Find("Dialogue").GetComponent<TextMeshProUGUI>();
            rightContinueButton = rightDialogueBox.transform.Find("ContinueButton").GetComponent<Button>();
            rightAnimator = rightDialogueBox.GetComponent<Animator>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            showCutscene = dialogue.showCutscene;
            inStory = true;
            sentences.Clear();
            foreach (string line in dialogue.sentences)
            {
                sentences.Enqueue(line);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {

            AudioClip actualAudioClip;

            if (sentences.Count <= 0)
            {
                EndDialogue();
                return;
            }

            currentSentence = sentences.Dequeue();
            StopAllCoroutines();

            // 判斷開頭是 L: 或 R: 並切換對話框
            if (currentSentence.StartsWith("L:"))
            {
                actualAudioClip = audioClipL;
                SetActiveDialogueBox(leftDialogueBox, rightDialogueBox, currentSentence.Substring(2), LeftCharacterName, actualAudioClip);
                leftAnimator.SetBool("IsOpen", true);
                rightAnimator.SetBool("IsOpen", false);
            }
            else if (currentSentence.StartsWith("R:"))
            {
                actualAudioClip = audioClipR;
                SetActiveDialogueBox(rightDialogueBox, leftDialogueBox, currentSentence.Substring(2), RightCharacterName, actualAudioClip);
                rightAnimator.SetBool("IsOpen", true);
                leftAnimator.SetBool("IsOpen", false);
            }
        }

        private void SetActiveDialogueBox(GameObject activeBox, GameObject inactiveBox, string dialogueText, string characterName, AudioClip audio)
        {
            TextMeshProUGUI activeNameText = activeBox == leftDialogueBox ? leftNameText : rightNameText;
            TextMeshProUGUI activeDialogueText = activeBox == leftDialogueBox ? leftDialogueText : rightDialogueText;

            activeBox.SetActive(true);

            inactiveBox.SetActive(false);

            activeNameText.text = characterName;
            StartCoroutine(TypeSentence(dialogueText, activeDialogueText, audio));
        }

        IEnumerator TypeSentence(string sentence, TextMeshProUGUI dialogueText, AudioClip audio)
        {
            isTyping = true;
            dialogueText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                audioSource.PlayOneShot(audio);
                yield return new WaitForSeconds(typingSpeed);
            }

            isTyping = false;
        }

        void EndDialogue()
        {
            inStory = false;
            Debug.Log("End of conversation.");
            leftDialogueBox.SetActive(true);
            rightDialogueBox.SetActive(true);
            leftAnimator.SetBool("IsOpen", false);
            rightAnimator.SetBool("IsOpen", false);
            leftDialogueBox.SetActive(false);
            rightDialogueBox.SetActive(false);

            if (showCutscene)
            {
                StartCoroutine(storyManager.PlayCutscene());
            }
            else
            {
                storyManager.EndStory();
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && inStory)
            {
                if (!isTyping)
                {
                    DisplayNextSentence();
                }
                else
                {
                    StopAllCoroutines();
                    TextMeshProUGUI activeDialogueText = leftDialogueBox.activeSelf ? leftDialogueText : rightDialogueText;
                    activeDialogueText.text = currentSentence.Substring(2);
                    isTyping = false;
                }
            }
        }
    }
}
