using UnityEngine;

public class AutomaticCannon : MonoBehaviour
{
    public GameObject lineBulletPrefab;  // Prefab with a LineRenderer (bullet)
    public Transform firePoint;          // Where the bullet will fire from
    public float bulletSpeed = 20f;      // Speed of the bullet
    public float fireRate = 0.5f;        // Time between shots (fire rate)
    private float lastFireTime = 0f;

    public float fadeDuration = 1f;      // Time it takes for the bullet line to fade away

    void Update()
    {
        // Check if the left mouse button is pressed and fire bullets
        if (Input.GetKey(KeyCode.Mouse0) && Time.time - lastFireTime > fireRate)
        {
            FireLineBullet();
            lastFireTime = Time.time;
        }
    }

    void FireLineBullet()
    {
        // Instantiate a new bullet
        GameObject lineBullet = Instantiate(lineBulletPrefab, firePoint.position, firePoint.rotation);

        // Set the start and end points of the line (bullet trajectory)
        Vector3 targetPosition = firePoint.position + firePoint.forward * 100f;  // Bullet goes 100 units forward
        LineRenderer lineRenderer = lineBullet.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, targetPosition);

        // Start moving the bullet forward
        StartCoroutine(MoveLineBullet(lineBullet, targetPosition, lineRenderer));
    }

    System.Collections.IEnumerator MoveLineBullet(GameObject lineBullet, Vector3 targetPosition, LineRenderer lineRenderer)
    {
        float journeyLength = Vector3.Distance(lineBullet.transform.position, targetPosition);
        float startTime = Time.time;

        // Store the initial color of the line
        Color startColor = lineRenderer.startColor;

        // Moving the bullet forward
        while (Vector3.Distance(lineBullet.transform.position, targetPosition) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * bulletSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            lineBullet.transform.position = Vector3.Lerp(lineBullet.transform.position, targetPosition, fractionOfJourney);

            // Fade out the line over time based on its journey progress
            float fadeAmount = Mathf.Lerp(1f, 0f, fractionOfJourney); // Fading over the journey
            lineRenderer.startColor = new Color(startColor.r, startColor.g, startColor.b, fadeAmount);
            lineRenderer.endColor = new Color(startColor.r, startColor.g, startColor.b, fadeAmount);

            yield return null;
        }

        // Destroy the bullet when it reaches the target
        Destroy(lineBullet);
    }
}
