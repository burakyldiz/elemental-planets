using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [Header("EarthBend Skill")]
    public GameObject earthBendEffectPrefab;
    public float earthBendRadius = 3f;
    public float earthBendDamage = 20f;
    public Transform earthBendPoint;

    [Header("EarthBend Deform")]
    public Terrain terrain;
    public float deformRadius = 5f;
    public float deformHeight = 2f;

    [Header("Throw Rock Skill")]
    public GameObject rockPrefab;
    public Transform throwPoint;
    public float throwForce = 15f;

    [Header("Cooldowns")]
    public float earthBendCooldown = 5f;
    public float throwRockCooldown = 3f;
    private float earthBendCooldownTimer = 0f;
    private float throwRockCooldownTimer = 0f;

    private float[,] originalHeights; // For restoring terrain heights

    void Start()
    {
        // Save the original terrain heights for deform
        if (terrain != null)
        {
            TerrainData terrainData = terrain.terrainData;
            originalHeights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        }
    }

    void Update()
    {
        // Update cooldown timers
        if (earthBendCooldownTimer > 0)
            earthBendCooldownTimer -= Time.deltaTime;

        if (throwRockCooldownTimer > 0)
            throwRockCooldownTimer -= Time.deltaTime;

        // Trigger skills based on input
        if (Input.GetKeyDown(KeyCode.Alpha1) && throwRockCooldownTimer <= 0)
        {
            ThrowRockSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && earthBendCooldownTimer <= 0)
        {
            EarthBendSkill();
        }
    }

    void ThrowRockSkill()
    {
        Debug.Log("Throw Rock used!");
        throwRockCooldownTimer = throwRockCooldown;

        // Instantiate the rock
        GameObject rock = Instantiate(rockPrefab, throwPoint.position, throwPoint.rotation);

        // Apply force
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
    }

    void EarthBendSkill()
    {
        Debug.Log("Earth Bend used!");
        earthBendCooldownTimer = earthBendCooldown;

        // Visual effect
        GameObject effect = Instantiate(earthBendEffectPrefab, earthBendPoint.position, Quaternion.identity);
        Destroy(effect, 2f);

        // Apply damage to nearby enemies
        Collider[] hitColliders = Physics.OverlapSphere(earthBendPoint.position, earthBendRadius);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                // implement in other file
            }
        }

        // Deform the terrain
        BendTerrain();
    }

    void BendTerrain()
    {
        if (terrain == null) return;

        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = GetRelativeTerrainPosition(earthBendPoint.position, terrain, terrainData);

        int terrainX = Mathf.RoundToInt(terrainPos.x);
        int terrainZ = Mathf.RoundToInt(terrainPos.z);
        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;
        int deformRadiusInHeightmap = Mathf.RoundToInt(deformRadius / terrainData.size.x * heightmapWidth);

        float[,] heights = terrainData.GetHeights(
            terrainX - deformRadiusInHeightmap / 2,
            terrainZ - deformRadiusInHeightmap / 2,
            deformRadiusInHeightmap,
            deformRadiusInHeightmap
        );

        for (int x = 0; x < deformRadiusInHeightmap; x++)
        {
            for (int z = 0; z < deformRadiusInHeightmap; z++)
            {
                float distance = Vector2.Distance(new Vector2(x, z), new Vector2(deformRadiusInHeightmap / 2, deformRadiusInHeightmap / 2));
                if (distance <= deformRadiusInHeightmap / 2)
                {
                    float gradient = 1f - (distance / (deformRadiusInHeightmap / 2f));
                    heights[x, z] += gradient * deformHeight / terrainData.size.y;
                }
            }
        }

        terrainData.SetHeights(
            terrainX - deformRadiusInHeightmap / 2,
            terrainZ - deformRadiusInHeightmap / 2,
            heights
        );
    }

    Vector3 GetRelativeTerrainPosition(Vector3 worldPosition, Terrain terrain, TerrainData terrainData)
    {
        Vector3 terrainPosition = worldPosition - terrain.transform.position;
        return new Vector3(
            (terrainPosition.x / terrainData.size.x) * terrainData.heightmapResolution,
            0,
            (terrainPosition.z / terrainData.size.z) * terrainData.heightmapResolution
        );
    }

    void OnApplicationQuit()
    {
        // Restore terrain heights on exit
        if (terrain != null && originalHeights != null)
        {
            TerrainData terrainData = terrain.terrainData;
            terrainData.SetHeights(0, 0, originalHeights);
        }
    }
}
