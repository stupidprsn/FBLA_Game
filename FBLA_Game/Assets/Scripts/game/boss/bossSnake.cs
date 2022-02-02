using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossSnake : MonoBehaviour {

    public int roundOneSnakes;
    public int roundTwoSnakes;
    public int roundThreeSnakes;
    private int totalSnakes;

    // Referances
    public GameObject snake;
    public GameObject warning;
    public GameObject snakes;
    public GameObject door;
    public Rigidbody2D rb;

    //Stats
    public int hp;
    private int phase = 1;
    private List<Transform> positions;

    public void onDamage() {
        hp--;
    }

    private IEnumerator onWin() {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(new Vector2(20 * Time.deltaTime, 50 * Time.deltaTime), ForceMode2D.Impulse);

        yield return new WaitForSeconds(2);

        door.SetActive(true);
    }

    private IEnumerator spawnSnakes(int secondsToWait, int numOfSnakes) {
        yield return new WaitForSeconds(secondsToWait);

        List<int> tempIndexes = new List<int>();
        List<int> indexesToUse = new List<int>();

        List<GameObject> warnings = new List<GameObject>();

        for (int i = 0; i < positions.Count; i++) {
            tempIndexes.Add(i);
        }
        for (int i = 0; i < numOfSnakes; i++) {
            int temp = Random.Range(0, tempIndexes.Count);
            indexesToUse.Add(tempIndexes[temp]);
            tempIndexes.RemoveAt(temp);
        }

        for (int i = 0; i < indexesToUse.Count; i++) {
            warnings.Add(
                Instantiate(warning, new Vector3(positions[indexesToUse[i]].position.x, positions[indexesToUse[i]].position.y + 0.75f, positions[indexesToUse[i]].position.z), Quaternion.identity, snakes.transform)
            );
        }

        yield return new WaitForSeconds(2);

        foreach (var item in warnings) {
            Destroy(item);
        }

        for (int i = 0; i < indexesToUse.Count; i++) {
            warnings.Add(
                Instantiate(snake, new Vector3(positions[indexesToUse[i]].position.x, positions[indexesToUse[i]].position.y + 0.75f, positions[indexesToUse[i]].position.z), Quaternion.identity, snakes.transform)
            );
        }
    }

    private void phaseOne() {
        if (phase == 1) {
            StartCoroutine(spawnSnakes(3, roundOneSnakes));
            phase = 2;
        }
    }

    private void phaseTwo() {
        if (phase == 2) {
            StartCoroutine(spawnSnakes(1, roundTwoSnakes));
            phase = 3;
        }
    }

    private void phaseThree() {
        if (phase == 3) {
            StartCoroutine(spawnSnakes(1, roundThreeSnakes));
            phase = 4;
        }
    }

    private void Start() {
        FindObjectOfType<soundManager>().stopAllSound();
        FindObjectOfType<soundManager>().PlaySound("musicBossLevel");

        hp = totalSnakes = roundOneSnakes + roundTwoSnakes + roundThreeSnakes;

        List<Transform> tempPositions = new List<Transform>(GameObject.Find("ShrinkedEnvironment").transform.Find("Platforms").GetComponentsInChildren<Transform>());
        positions = new List<Transform>();

        for(int i = 0; i < tempPositions.Count; i++) {
            if(tempPositions[i].GetComponent<PolygonCollider2D>() == null) {
                tempPositions.RemoveAt(i);
            }
        }

        foreach (var item in tempPositions) {
            if(item.name != "Square (1)" && item.tag != "Respawn") {
                positions.Add(item);
            }
        }
    }

    private void Update() {
        if (hp > totalSnakes - roundOneSnakes) {
            phaseOne();
        } else if(hp > totalSnakes - roundOneSnakes - roundTwoSnakes) {
            phaseTwo();
        } else if(hp > 0){
            phaseThree();
        } else {
            StartCoroutine(onWin());
        }
    }
}
