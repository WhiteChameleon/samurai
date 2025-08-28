using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPlayer : MonoBehaviour
{
    public static SpawnPlayer Instance { get; private set; }

    [SerializeField] GameObject player;
    [SerializeField] GameObject cameraObj;
    [SerializeField] Vector2 spawnPosBar;
    [SerializeField] Vector2 spawnPosStreetLeft;
    [SerializeField] Vector2 spawnPosStreetRight;

    private Vector3? lastExitPosition;
    private string lastExitScene;

    private string nameScene;
    private string lastNameScene;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Подписываемся на событие загрузки сцены
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RegisterSceneTransition("");
            SceneManager.LoadScene("Street");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            RegisterSceneTransition("");
            SceneManager.LoadScene("DevMenu");
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newScene = scene.name;
        // Находим игрока в новой сцене
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player not found in the scene!");
            return;
        }
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        // Если пришёл из другой сцены - ставим у точки входа
        if (newScene == "Bar")
        {
            cameraObj.transform.position = new Vector3(0, 3, -50);
            player.transform.position = spawnPosBar;
        }
        else if (newScene == "Street" && lastNameScene == "Bar")
        {
            cameraObj.transform.position = new Vector3(132, 3, -10);
            player.transform.position = spawnPosStreetRight;
        }
        else
        {
            cameraObj.transform.position = new Vector3(0, 3, -10);
            player.transform.position = spawnPosStreetLeft;
        }
    }
    public void RegisterSceneTransition(string lastScene)
    {
        lastNameScene = lastScene;
    }
}
