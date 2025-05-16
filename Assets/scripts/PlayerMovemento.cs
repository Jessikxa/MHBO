using UnityEngine;

public class PlayerMovemento : MonoBehaviour

{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private int _speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(x, -1, y);
        _characterController.Move(move * Time.deltaTime * _speed);

    }
}
