using System.Collections;
using UnityEngine;


namespace Digital_Subcurrent
{
    public class StoryManager : MonoBehaviour
    {
        public static StoryManager Instance;
        public GameObject player;
        public GameObject target;
        public GameObject effect;
        private DialogueManager dialogueManager;
        private SceneManagerCUS sceneChanger;

        private Animator playerAnimator;

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
            sceneChanger = SceneManagerCUS.Instance;
            dialogueManager = DialogueManager.Instance;

            if (player == null)
            {
                Debug.LogError("Player not found!");
            }else
            {
                playerAnimator = player.GetComponent<Animator>();
            }
        }

        public void StartStory(Dialogue dialogue)
        {
            if(dialogue.showCutscene)
            {
                playerAnimator.SetInteger("Direction", 1);
            }else
            {
                playerAnimator.SetInteger("Direction", 0);
            }
            
            playerAnimator.SetBool("IsMoving", false);
            player.GetComponent<PlayerController>().enabled = false; // 禁用玩家控制
            dialogueManager.StartDialogue(dialogue);
        }

        public void EndStory()
        {
            player.GetComponent<PlayerController>().enabled = true; // 恢復玩家控制
        }

        public IEnumerator PlayCutscene()
        {
            effect.SetActive(true);
            Transform playerT = player.transform;
            Transform targetT = target.transform;

            playerAnimator.SetInteger("Direction", 3);
            playerAnimator.SetBool("IsMoving", true);

            float duration = 2.5f;
            Vector3 startPos = playerT.position;
            Vector3 endPos = targetT.position;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                playerT.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
                yield return null;
            }

            playerT.position = endPos;
            //Wait One second
            yield return new WaitForSeconds(1f);

            ChangeScene();
            //player.GetComponent<PlayerController>().enabled = true; // 恢復玩家控制
        }

        private void ChangeScene()
        {
            sceneChanger.ChangeScene("IF");
        }
    }
}
