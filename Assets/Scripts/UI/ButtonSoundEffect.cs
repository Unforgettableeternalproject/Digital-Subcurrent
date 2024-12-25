using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip hoverSound; // �ƹ����J����
    public AudioClip clickSound; // ���U����
    public AudioSource audioSource; // ���ļ���

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        if (audioSource == null)
        {
            // �p�G�S�����w AudioSource�A�۰ʱq���������է��@��
            audioSource = FindObjectOfType<AudioSource>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound(hoverSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
