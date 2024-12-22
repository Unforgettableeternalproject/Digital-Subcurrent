using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public bool isTyping = false; // ����b���r�ɭ��Ƹ��L
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;

    [Range(0.01f, 0.2f)] // �K�[�Ʊ챱��
    public float typingSpeed = 0.05f; // ����r�t��

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

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true; // �]�w�����b���r
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // �ϥΦ۩w�q�t�ױ���
        }

        isTyping = false; // ���r����
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

    void Update()
    {
        // �ˬd�ϥΪ̬O�_�I���ƹ������Ĳ���ù�
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            if (!isTyping) // ����b���r�ɸ��L
            {
                DisplayNextSentence();
            }
            else
            {
                // �p�G���b���r�A������ܧ���y�l
                StopAllCoroutines();
                dialogueText.text = sentences.Peek(); // ��ܧ���y�l
                isTyping = false;
            }
        }
    }
}
