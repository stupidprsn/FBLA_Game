/*
 * Hanlin Zhang
 * Purpose: Script for the boss snake
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossSnake : MonoBehaviour {

    // Number of snakes that spawn each round
    [SerializeField] private int roundOneSnakes;
    [SerializeField] private int roundTwoSnakes;
    [SerializeField] private int roundThreeSnakes;
    private int totalSnakes;

    // Referances
    [SerializeField] private GameObject snake;
    [SerializeField] private GameObject warning;
    [SerializeField] private GameObject snakes;
    [SerializeField] private GameObject door;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Variables
    public int hp;
    private int phase = 1;
    private bool facingRight = false;
    private List<Transform> positions;

    // Method for the boss taking damage
    public void onDamage() {
        // Decrement the hp
        hp--;
        // Show the damage animation
        animator.SetTrigger("damage");
    }

    // Method for winning the game
    // An IEnumerator is used because the method utilizes time
    private IEnumerator onWin() {
        // Allow the boss to move
        rb.constraints = RigidbodyConstraints2D.None;
        // Give the boss a slight nudge
        rb.AddForce(new Vector2(50 * Time.deltaTime, 100 * Time.deltaTime), ForceMode2D.Impulse);

        // Wait 2 seconds
        yield return new WaitForSeconds(2);

        // Show the door
        door.SetActive(true);
    }

    // Method for spawning the snake enemies
    // Takes in an int for the amount of time to wait before spawning the snakes and an int for the number of snakes to spawn
    // An IEnumerator is used because the method utilizes time
    private IEnumerator spawnSnakes(int secondsToWait, int numOfSnakes) {
        // Wait the wait time
        yield return new WaitForSeconds(secondsToWait);

        // Temporary list used to store the possible platforms to spawn on
        List<int> tempIndexes = new List<int>();
        // List of platforms to spawn snakes on
        List<int> indexesToUse = new List<int>();

        // List of warning symbols
        List<GameObject> warnings = new List<GameObject>();

        // Add all platforms to the temporary index
        for (int i = 0; i < positions.Count; i++) {
            tempIndexes.Add(i);
        }
        // Pick random platforms from the temporary list to use
        for (int i = 0; i < numOfSnakes; i++) {
            int temp = Random.Range(0, tempIndexes.Count);
            indexesToUse.Add(tempIndexes[temp]);
            tempIndexes.RemoveAt(temp);
        }

        // For each platform, create a warning symbol
        for (int i = 0; i < indexesToUse.Count; i++) {
            warnings.Add(
                Instantiate(warning, new Vector3(positions[indexesToUse[i]].position.x, positions[indexesToUse[i]].position.y + 0.75f, positions[indexesToUse[i]].position.z), Quaternion.identity, snakes.transform)
            );
        }

        // Wait 2 seconds so the user can respond
        yield return new WaitForSeconds(2);

        // Destroy the warnings
        foreach (var item in warnings) {
            Destroy(item);
        }

        // Spawn snakes
        for (int i = 0; i < indexesToUse.Count; i++) {
            warnings.Add(
                Instantiate(snake, new Vector3(positions[indexesToUse[i]].position.x, positions[indexesToUse[i]].position.y + 0.75f, positions[indexesToUse[i]].position.z), Quaternion.identity, snakes.transform)
            );
        }
    }

    // Method for flipping the boss snake horizontally
    private void flip() {
        if ((player.position.x < 0 && facingRight) || (player.position.x > 0 && !facingRight)) {
            transform.Rotate(new Vector3(0, 180, 0));
            facingRight = !facingRight;
        }
    }

    // Method for phase one of the boss
    private void phaseOne() {
        if (phase == 1) {
            StartCoroutine(spawnSnakes(3, roundOneSnakes));
            phase = 2;
        }
    }

    // Method for phase one of the boss
    private void phaseTwo() {
        if (phase == 2) {
            StartCoroutine(spawnSnakes(1, roundTwoSnakes));
            phase = 3;
        }
    }

    // Method for phase one of the boss
    private void phaseThree() {
        if (phase == 3) {
            StartCoroutine(spawnSnakes(1, roundThreeSnakes));
            phase = 4;
        }
    }

    private void Start() {
        // Find the total number of snakes to spawn
        hp = totalSnakes = roundOneSnakes + roundTwoSnakes + roundThreeSnakes;

        // Find all platforms where the snake enemies can be spawned
        // Find every game object under the Platforms folder
        List<Transform> tempPositions = new List<Transform>(GameObject.Find("ShrinkedEnvironment").transform.Find("Platforms").GetComponentsInChildren<Transform>());
        // List of platforms
        positions = new List<Transform>();

        // For each gameobject
        for(int i = 0; i < tempPositions.Count; i++) {
            // Check that it's a platform, if it's not, delete it
            if(tempPositions[i].GetComponent<PolygonCollider2D>() == null) {
                tempPositions.RemoveAt(i);
            }
        }

        // Copy over the platforms
        foreach (var item in tempPositions) {
            // Square (1) is not cleared so they are manually cleared here
            // Won't run without
            if (item.name != "Square (1)") {
                positions.Add(item);
            }
        }
    }

    private void Update() {
        // Check if the snake should flip horizontally
        flip();

        // Check which phase the snake is on depending on its health
        if (hp > totalSnakes - roundOneSnakes) {
            phaseOne();
        } else if(hp > totalSnakes - roundOneSnakes - roundTwoSnakes) {
            phaseTwo();
        } else if(hp > 0){
            phaseThree();
        } else {
            // Once the snake has ran out of lives, run the win method
            StartCoroutine(onWin());
        }
    }
}
