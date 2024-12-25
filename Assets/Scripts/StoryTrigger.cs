using UnityEngine;

namespace Digital_Subcurrent
{
    public class StoryTrigger : MonoBehaviour
    {
        public Dialogue dialogue;
        private StoryManager storyManager;

        private bool isTriggered = false;

        void Start()
        {
            storyManager = StoryManager.Instance;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !isTriggered)
            {
                collision.GetComponent<PlayerController>().enabled = false;
                storyManager.StartStory(dialogue);
                isTriggered = true;
            }
        }
    }
}
