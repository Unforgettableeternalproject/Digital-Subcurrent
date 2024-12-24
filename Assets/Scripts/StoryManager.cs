using System.Collections;
using UnityEngine;


namespace Digital_Subcurrent
{
    public class StoryManager : MonoBehaviour
    {
        public static StoryManager Instance;
        public GameObject player;
        private DialogueManager dialogueManager;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            dialogueManager = DialogueManager.Instance;
        }

        public void StartStory(Dialogue dialogue)
        {
            player.GetComponent<PlayerController>().enabled = false; // 禁用玩家控制
            dialogueManager.StartDialogue(dialogue);
        }

        public IEnumerator PlayCutscene()
        {
            //Transform player = GameObject.FindWithTag("Player").transform;
            //Transform target = GameObject.FindWithTag("PlayerTarget").transform;

            //float duration = 1f;
            //Vector3 startPos = player.position;
            //Vector3 endPos = target.position;

            //float elapsed = 0f;
            //while (elapsed < duration)
            //{
            //    elapsed += Time.deltaTime;
            //    player.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            //    yield return null;
            //}

            //player.position = endPos
            //Wait One second
            yield return new WaitForSeconds(1f);

            player.GetComponent<PlayerController>().enabled = true; // 恢復玩家控制
        }
    }
}
