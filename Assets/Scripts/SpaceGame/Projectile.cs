using UnityEngine;
using UnityEngine.Rendering;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private Rigidbody rb = null;
    private Player player = null;
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + (Vector3.forward * speed * Time.fixedDeltaTime));
    }


    private void OnTriggerEnter(Collider other)
    {
        Meteor m = other.gameObject.GetComponentInParent<Meteor>();
        if (m != null)
        {
            m.transform.parent.GetComponent<MeteorSpawner>().DeleteMeteor(m.gameObject);
            player.DeleteProjectile(this.gameObject);
            if (SpaceGameManager.Instance.GameMode() == SpaceGameManager.mode.even) 
            {
                if (m.GetNumber() % 2 == 0)
                {
                    SpaceGameManager.Instance.ModifyPoints(1);
                }
                else
                {
                    SpaceGameManager.Instance.ModifyPoints(-1);
                }
            }
            else if (SpaceGameManager.Instance.GameMode() == SpaceGameManager.mode.odd) 
            {
                if (m.GetNumber() % 2 != 0)
                {
                    SpaceGameManager.Instance.ModifyPoints(1);
                }
                else
                {
                    SpaceGameManager.Instance.ModifyPoints(-1);
                }
            }
        }
        else if (other.gameObject.tag == "Destroyer")
        {
            player.DeleteProjectile(this.gameObject);
        }
    }
    public void SetPlayer(Player p)
    {
        player = p;
    }
}
