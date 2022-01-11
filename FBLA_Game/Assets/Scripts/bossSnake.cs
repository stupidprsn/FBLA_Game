using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossSnake : MonoBehaviour {
    public GameObject snake;
    public GameObject warning;
    public GameObject interactive;

    public int hp = 22;
    private int phase = 1;
    private Transform[] positions;

    private IEnumerator spawnSnakes(int numOfSnakes) {
        List<int> tempIndexes = new List<int>();
        List<int> indexesToUse = new List<int>();

        List<GameObject> warnings = new List<GameObject>();

        for (int i = 0; i < positions.Length; i++) {
            tempIndexes.Add(i);
        }
        for (int i = 0; i < numOfSnakes; i++) {
            int temp = Random.Range(0, tempIndexes.Count);
            indexesToUse.Add(tempIndexes[temp]);
            tempIndexes.RemoveAt(temp);
        }

        for (int i = 0; i < indexesToUse.Count; i++) {
            warnings.Add(
                Instantiate(warning, new Vector3(positions[indexesToUse[i]].position.x, positions[indexesToUse[i]].position.y + 1.5f, positions[indexesToUse[i]].position.z), Quaternion.identity, interactive.transform)
            );
        }

        Debug.Log(positions);

        yield return new WaitForSeconds(1);

        foreach (var item in warnings) {
            Destroy(item);
        }

        for (int i = 0; i < indexesToUse.Count; i++) {
            warnings.Add(
                Instantiate(snake, new Vector3(positions[indexesToUse[i]].position.x, positions[indexesToUse[i]].position.y + 1.5f, positions[indexesToUse[i]].position.z), Quaternion.identity, interactive.transform)
            );
        }
    }

    private void phaseOne() {
        if(phase == 1) {
            StartCoroutine(spawnSnakes(5));
            phase = 2;
        }
    }

    private void phaseTwo() {
        if (phase == 2) {
            StartCoroutine(spawnSnakes(5));
            phase = 3;
        }
    }

    private void phaseThree() {
        if (phase == 3) {
            StartCoroutine(spawnSnakes(5));
            phase = 4;
        }
    }

    private void Start() {
        positions = GameObject.Find("ShrinkedEnvironment").transform.Find("Platforms").GetComponentsInChildren<Transform>();
    }

    private void Update() {
        if (hp > 17) {
            phaseOne();
        } else if (hp > 10) {
            phaseTwo();
        } else {
            phaseThree();
        }
    }
}
