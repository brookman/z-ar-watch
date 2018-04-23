using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public GameObject goblinPrefab;
    public Transform marker;
    public Clock clock;
    public ParticleSystem sparkEffect;
    public Text gameOverText;

    private float penaltyTime = 0;
    private TimeSpan simulationStartTime;
    private TimeSpan realStartTime;
    private bool gameRunning = false;
    private float secondsBetweenSpawns = 3;
    private float spawnAcceleration = 0.04f;

    void Start()
    {
        realStartTime = DateTime.Now.TimeOfDay;
        simulationStartTime = TimeSpan.FromHours(12) - TimeSpan.FromMinutes(3);
        gameRunning = true;

        StartCoroutine(SpawnGoblin());
    }

    void Update()
    {
        if (!gameRunning)
        {
            return;
        }

        var elapsed = DateTime.Now.TimeOfDay - realStartTime;
        var simulationTime = simulationStartTime + elapsed + TimeSpan.FromSeconds(penaltyTime);

        if (simulationTime >= TimeSpan.FromHours(12))
        {
            gameRunning = false;
            simulationTime = TimeSpan.FromHours(12);

            StopAllCoroutines();
            ClearAllGoblins();

            gameOverText.text = "Game Over!\nScore: " + (int) elapsed.TotalSeconds;
            gameOverText.gameObject.SetActive(true);
        }

        TimeProvider.UpdateOverrideTime(simulationTime);
    }

    private IEnumerator SpawnGoblin()
    {
        while (gameRunning)
        {
            var pos = RandomPointOnUnitCircle() * 0.4f + marker.position;

            var goblin = Instantiate(goblinPrefab, pos, Quaternion.identity, marker);
            goblin.GetComponent<GoblinController>().target = clock.gameObject.transform;
            goblin.GetComponent<GoblinController>().gameController = this;

            secondsBetweenSpawns -= spawnAcceleration;
            yield return new WaitForSeconds(secondsBetweenSpawns);
        }
    }

    private void ClearAllGoblins()
    {
        foreach (var goblin in FindObjectsOfType<GoblinController>())
        {
            goblin.Damage(10000);
        }
    }

    private static Vector3 RandomPointOnUnitCircle()
    {
        var pos = Random.insideUnitCircle.normalized;
        return new Vector3(pos.x, 0, pos.y);
    }


    public void OnDamage(float damage)
    {
        penaltyTime += damage;
        if (sparkEffect)
        {
            sparkEffect.Play();
        }
    }
}