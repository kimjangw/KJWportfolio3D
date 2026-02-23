using UnityEngine;

public class SystemManager : MonoBehaviour
{
    // 클래스 이름이 SystemManager로 바뀌었으므로, instance 타입도 똑같이 맞춰줍니다.
    public static SystemManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LockMouse();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockMouse();
        }

        if (Cursor.visible && Input.GetMouseButtonDown(0))
        {
            LockMouse();
        }
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}