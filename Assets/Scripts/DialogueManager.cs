using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Digital_Subcurrent
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        private bool isTyping = false;
        private string currentSentence;

        public GameObject leftDialogueBox;
        public GameObject rightDialogueBox;

        private Animator animator;

        [Range(0.01f, 0.2f)]
        public float typingSpeed = 0.05f;

        private Queue<string> sentences;

        private TextMeshProUGUI leftNameText;
        private TextMeshProUGUI leftDialogueText;
        private Button leftContinueButton;

        private TextMeshProUGUI rightNameText;
        private TextMeshProUGUI rightDialogueText;
        private Button rightContinueButton;

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
            sentences = new Queue<string>();

            // 動態查找左側對話框內部組件
            leftNameText = leftDialogueBox.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            leftDialogueText = leftDialogueBox.transform.Find("Dialogue").GetComponent<TextMeshProUGUI>();
            leftContinueButton = leftDialogueBox.transform.Find("ContinueButton").GetComponent<Button>();

            // 動態查找右側對話框內部組件
            rightNameText = rightDialogueBox.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            rightDialogueText = rightDialogueBox.transform.Find("Dialogue").GetComponent<TextMeshProUGUI>();
            rightContinueButton = rightDialogueBox.transform.Find("ContinueButton").GetComponent<Button>();

            // 為繼續按鈕添加事件
            leftContinueButton.onClick.AddListener(DisplayNextSentence);
            rightContinueButton.onClick.AddListener(DisplayNextSentence);
        }

        public void StartDialogue(Dialogue dialogue)
        {

            // 翻轉對話框並初始化
            if (dialogue.isLeft)
            {
                SetActiveDialogueBox(leftDialogueBox, rightDialogueBox, dialogue.name);
                animator = leftDialogueBox.GetComponent<Animator>();
            }
            else
            {
                SetActiveDialogueBox(rightDialogueBox, leftDialogueBox, dialogue.name);
                animator = rightDialogueBox.GetComponent<Animator>();
            }

            animator.SetBool("IsOpen", true);

            sentences.Clear();
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        private void SetActiveDialogueBox(GameObject activeBox, GameObject inactiveBox, string speakerName)
        {
            activeBox.SetActive(true);
            inactiveBox.SetActive(false);

            if (activeBox == leftDialogueBox)
            {
                leftNameText.text = speakerName;
                leftDialogueText.text = "";
            }
            else
            {
                rightNameText.text = speakerName;
                rightDialogueText.text = "";
            }
        }

        public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            currentSentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentSentence));
        }


        IEnumerator TypeSentence(string sentence)
        {
            isTyping = true;
            TextMeshProUGUI activeDialogueText = leftDialogueBox.activeSelf ? leftDialogueText : rightDialogueText;
            activeDialogueText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                activeDialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            isTyping = false;
        }

        void EndDialogue()
        {
            animator.SetBool("IsOpen", false);
            StartCoroutine(StoryManager.Instance.PlayCutscene());
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isTyping)
                {
                    DisplayNextSentence();
                }
                else
                {
                    StopAllCoroutines();
                    TextMeshProUGUI activeDialogueText = leftDialogueBox.activeSelf ? leftDialogueText : rightDialogueText;
                    activeDialogueText.text = currentSentence;
                    isTyping = false;
                }
            }
        }
    }
}
