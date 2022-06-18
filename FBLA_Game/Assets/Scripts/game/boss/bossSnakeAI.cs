/*
 * Hanlin Zhang
 * Purpose: AI for the snake enemies in the boss level
 *          Very similar to regular snake AI script (see normal snake AI script for in-depth explanations)
 */

using UnityEngine;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game.Boss
{
    public class bossSnakeAI : MonoBehaviour
    {
        public GameObject artifact;
        public LayerMask boundary;
        public bool facingRight;
        public float speed;

        private bool canGoForward()
        {
            Vector2 forwardVector2;
            Vector2 downVector2;
            Vector2 forwardDirection;

            // Different values are needed as the snake is a different size
            if (facingRight)
            {
                forwardVector2 = new Vector2(transform.position.x + 0.7f, transform.position.y - 0.25f);
                downVector2 = new Vector2(transform.position.x + 0.45f, transform.position.y - 0.5f);
                forwardDirection = Vector2.right;
            } else
            {
                forwardVector2 = new Vector2(transform.position.x - 0.35f, transform.position.y - 0.25f);
                downVector2 = new Vector2(transform.position.x - 0.45f, transform.position.y - 0.5f);
                forwardDirection = Vector2.left;
            }

            RaycastHit2D[] forwardHits = Physics2D.RaycastAll(forwardVector2, forwardDirection, 0.2f, boundary);
            RaycastHit2D[] downHits = Physics2D.RaycastAll(downVector2, Vector2.down, 0.5f);

            Debug.DrawRay(forwardVector2, Vector2.left * 0.2f, Color.green);
            Debug.DrawRay(downVector2, Vector2.down * 0.5f, Color.green);

            if (forwardHits.Length == 0 && downHits.Length != 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void changeDirection()
        {
            transform.Rotate(new Vector3(0, 180, 0));
            facingRight = !facingRight;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (collision.transform.position.y - transform.position.y > 0.1f)
                {
                    Instantiate(artifact, transform.position, Quaternion.identity, transform.parent);
                    FindObjectOfType<SoundManager>().PlaySound(SoundNames.KillSnake);
                    Destroy(this.gameObject);
                } else
                {
                    StartCoroutine(FindObjectOfType<GameManager>().onDeath());
                }
            }
        }

        private void Update()
        {
            if (!canGoForward())
            {
                changeDirection();
            }
        }

        private void FixedUpdate()
        {
            transform.Translate(
                new Vector2(-speed * Time.fixedDeltaTime, 0)
            );
        }
    }

}

