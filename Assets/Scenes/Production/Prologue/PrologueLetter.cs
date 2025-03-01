using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueLetter : MonoBehaviour
{
        [SerializeField] Transform fastTravelPos;
        [SerializeField] AudioSource monologue;
        Player player;
        void Start()
        {
                player = GameManager.Instance.player;
                player.playerController.onEscape.AddListener(NormalCloseLetter);
                player.playerController.onJump.AddListener(FastTravelCloseLetter);
                player.playerController.canMove = false;
                player.playerController.canJump = false;
                player.playerController.canLookAround = false;
        }

        void NormalCloseLetter()
        {
                monologue.Play();
                CloseLetter();
        }

        void FastTravelCloseLetter()
        {
                player.gameObject.GetComponent<CharacterController>().enabled = false;
                player.gameObject.transform.position = fastTravelPos.position;
                player.gameObject.GetComponent<CharacterController>().enabled = true;
                CloseLetter();
        }

        void CloseLetter()
        {
                TutorialManager.Instance.ShowDefaultTutorial(true);
                player.playerController.canMove = true;
                player.playerController.canJump = true;
                player.playerController.canLookAround = true;
                Destroy(gameObject);
        }
}
