using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    public float SecondsPerShot = 2f;
    private float _currentTime;
	
	void Update () {
        // Timer to shoot projectiles in random direction
	    if (_currentTime >= SecondsPerShot)
	    {
            // Teleport projectile
	        StartCoroutine(TeleportObject());

	        // Reset timer
	        _currentTime = 0f;
        }
	    else
	    {
            // Add to the timer
	        _currentTime += Time.deltaTime;
	    }
	}

    private IEnumerator TeleportObject()
    {
        var projectiles = new List<Transform>();
        for (var i = 0; i < 20; i++)
        {
            var randomPosition = new Vector3(Random.Range(transform.position.x - 10, transform.position.x + 10),
                Random.Range(transform.position.y - 10, transform.position.y + 10), 0);

            // Get an existing projectile with name 'Projectile', else create one
            var projectile = ObjectPool.GetCustom<Transform>(f => f.name == "Projectile");
            if (projectile != null)
            {
                // Change position
                projectile.transform.position = randomPosition;
            }
            else
            {
                // Create new sphere projectile
                projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;

                // Interact with projectile
                projectile.name = "Projectile";
                projectile.position = randomPosition;
            }

            projectiles.Add(projectile);
        }

        yield return new WaitForSeconds(1f);

        // Pool the projectiles again because we no longer need them
        foreach (var projectile in projectiles)
            ObjectPool.Add(projectile);
    }
}