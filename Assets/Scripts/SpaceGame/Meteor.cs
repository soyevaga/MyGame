using TMPro;
using UnityEngine;
public class Meteor : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private TextMeshPro text = null;
    private float speed = 3f;
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + (Vector3.back * speed * Time.fixedDeltaTime));
    }
    private void OnTriggerEnter(Collider other)
    {
        Player p = other.gameObject.GetComponentInParent<Player>();

        if(p != null)
        {
            gameObject.SetActive(false);
            SpaceGameManager.Instance.DestroyPlayer(p.gameObject);
        }
        else if(other.gameObject.tag == "Destroyer")
        {
            transform.parent.GetComponent<MeteorSpawner>().DeleteMeteor(gameObject);
        }
    }
    public void SetNumber(int n)
    {
        text.text = n.ToString();
    }

    public int GetNumber()
    {
        return int.Parse(text.text);
    }
    public void SetSpeed(float newspeed)
    {
        speed = newspeed;
    }
}
