using UnityEngine;

public class PhysUIContentParenter : MonoBehaviour
{
        public Transform parentBackpack;
        public Transform parentWorld;
        private void OnTriggerEnter2D(Collider2D collision)
        {
                if (collision.gameObject.GetComponent<Item>() != null)
                {
                        collision.transform.parent = parentBackpack;
                }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
                if (parentWorld.gameObject.activeInHierarchy)
                {
                        if (parentBackpack.gameObject.activeInHierarchy)
                        {
                                if (collision.gameObject.activeInHierarchy)
                                {
                                        if (collision.gameObject.GetComponent<Item>() != null)
                                        {
                                                collision.transform.parent = parentWorld;
                                        }
                                }
                        }
                }
        }
}
