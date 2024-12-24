using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip hoverSound; // 滑鼠移入音效
    public AudioClip clickSound; // 按下音效
    public AudioSource audioSource; // 音效播放器

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        if (audioSource == null)
        {
            // 如果沒有指定 AudioSource，自動從場景中嘗試找到一個
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
