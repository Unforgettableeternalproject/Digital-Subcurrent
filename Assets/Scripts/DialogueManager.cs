using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Digital_Subcurrent
{
    public class DialogueManager : MonoBehaviour
    {
        private bool isTyping = false; // 防止在打字時重複跳過
        private string currentSentence; // 用來儲存當前句子

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;

        public Animator animator;

        [Range(0.01f, 0.2f)] // 添加滑桿控制
        public float typingSpeed = 0.05f; // 控制打字速度

        private Queue<string> sentences;

        // Use this for initialization
        void Start()
        {
            sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            animator.SetBool("IsOpen", true);

            nameText.text = dialogue.name;

            sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
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
            isTyping = true; // 設定為正在打字
            dialogueText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(Mathf.Clamp(typingSpeed, 0.01f, 0.2f)); // 使用自定義速度控制
            }

            isTyping = false; // 打字完成
        }

        void EndDialogue()
        {
            animator.SetBool("IsOpen", false);
            // TODO: 可在此加入更多對話結束後的行為，例如恢復玩家控制等
        }

        void Update()
        {
            // 檢查使用者是否點擊滑鼠左鍵或觸控螢幕
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                if (!isTyping) // 防止在打字時跳過
                {
                    DisplayNextSentence();
                }
                else
                {
                    // 如果正在打字，直接顯示完整句子
                    StopAllCoroutines();
                    dialogueText.text = currentSentence; // 顯示完整句子
                    isTyping = false;
                }
            }
        }
    }
}
